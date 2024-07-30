using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.Data;
using ProcessManager.Data.Enums;
using ProcessManager.Services_Interfaces;
using ProcessManager.Services_Interfaces.WatchDog;
using ProcessManager.Utils;
using ProcessManager.ViewModels.Dialogs;
using ProcessManager.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DialogResult = ProcessManager.Data.DialogResult;

namespace ProcessManager.ViewModels;

public partial class ProcessViewModel : ViewModelBase
{
    /// <summary>
    /// 进程集合
    /// </summary>
    [ObservableProperty]
    ObservableCollection<ProcessInfo> processes;
    /// <summary>
    /// 功能列表
    /// </summary>
    [ObservableProperty]
    IEnumerable<FunctionButton> functions;
    private readonly IDialogService _dialogService;
    private Dog watchDog;
    private static int times = 10;
    public ProcessViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
        ///设置功能列表
        Functions = new List<FunctionButton>() {
            new FunctionButton(){Name="添加进程",Command=AddCommand},
            new FunctionButton(){Name="关闭所有进程",Command=StopAllCommand},
            new FunctionButton(){Name="启动所有进程",Command=StartAllCommand},
        };
        Processes = new ObservableCollection<ProcessInfo>();
        //读配置
        Init();
        //初始化看门狗
        watchDog = new Dog(RefreshProcessRealTimeInfo);
        watchDog.AddAction(RestartProcess);
        watchDog.Start();
    }
    private async void Init()
    {
        string filePath = @"opt.json";
        var opts = JsonUtils.ReadfromJson<IEnumerable<ProcessStartingOptions?>>(filePath);
        foreach (var opt in opts)
        {
            Processes.Add(new ProcessInfo { ProcessName = StringUtils.FullNameToProcessName(opt!.Path!), ProcessStartingOptions = opt, Running = false });
        }
        var tasks = new List<Task>();
        foreach (var p in Processes)
        {
            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    //获取所有与指定名称匹配的进程 实际上只允许有一个 根据名称匹配确实也做不出多进程
                    Process[] processes = Process.GetProcessesByName(p.ProcessName);
                    if (processes.Length == 1)
                    {
                        p.Process = processes[0];
                        p.Running = true;
                    }
                    else if (processes.Length == 0 && p.ProcessStartingOptions!.StartingOption == StartingOptions.OpenWhenBoot)
                    {
                        await Task.Delay(p.ProcessStartingOptions.DelayTime.GetValueOrDefault());
                        //超时重启的逻辑写在Start()中，因为理论上每一次启动时都需要超时重启，并不应该是只有第一次启动时需要考虑超时重启
                        await Start(p);
                    }
                }
                catch
                {
                    //TODO:这里最好有点响应 但是还没想好响应什么
                }
                //这一步会非常耗时
                //传入实例名称 添加性能监视器
                p.Watcher = new ProcessUtils.GeneralProcessWatcher(p.ProcessName!);
            }));
        }
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// 刷新进程实时信息并且更新到UI
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RefreshProcessRealTimeInfo(Object? sender, EventArgs e)
    {
        foreach (var p in Processes)
        {
            if (null != p && null != p.Watcher)
            {
                p.ProcessRealtimeInfo = p.Watcher.Watch();
                p.Running = (p.ProcessRealtimeInfo.ProcessStatus == ProcessStatus.Running);
            }
        }
    }
    /// <summary>
    /// 重启超过占用的进程
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void RestartProcess(Object? sender, EventArgs e)
    {
        foreach (var p in Processes)
        {
            if (true == p.ProcessStartingOptions!.EnableMaxCPUUsage)
            {
                if (p.ProcessRealtimeInfo!.CPUUsage > p.ProcessStartingOptions.MaxCPUUsage)
                {
                    Stop(p);
                    await Start(p);
                }
            }
            if (true == p.ProcessStartingOptions!.EnableMaxRAMUsage)
            {
                if (p.ProcessRealtimeInfo!.RAMUsage > p.ProcessStartingOptions.MaxRAMUsage)
                {
                    Stop(p);
                    await Start(p);
                }
            }
        }
    }
    /// <summary>
    /// 新增的需要手动启动
    /// </summary>
    [RelayCommand]
    public void Add()
    {
        var res = _dialogService.OpenDialog<AddDialogView, AddDialogViewModel, ProcessStartingOptions>("添加进程", new object());
        if (res != null)
        {
            var name = StringUtils.FullNameToProcessName(res!.Path!);
            Processes.Add(new ProcessInfo { ProcessName = name, ProcessStartingOptions = res, Running = false, Watcher = new ProcessUtils.GeneralProcessWatcher(name)});
        }
        var newcfg = Processes.Select(p => p.ProcessStartingOptions);
        string outputPath = @"opt.json";
        JsonUtils.WriteToJson<IEnumerable<ProcessStartingOptions?>>(outputPath, newcfg);
    }
    /// <summary>
    /// 开启全部
    /// </summary>
    [RelayCommand]
    public async Task StartAll()
    {
        foreach (var processInfo in Processes)
        {
            await Start(processInfo);
        }
    }
    /// <summary>
    /// 关闭全部
    /// </summary>
    [RelayCommand]
    public void StopAll()
    {
        foreach (var processInfo in Processes)
        {
            Stop(processInfo);
        }
    }
    /// <summary>
    /// 开启
    /// </summary>
    /// <param name="processInfo"></param>
    [RelayCommand]
    public async Task<bool> Start(ProcessInfo processInfo)
    {
        //先用性能监视器拦截一次
        if (processInfo.Running == true) return false;
        //可能程序在跑但是监视器是一秒刷新一次，状态没有更新，因此还要检测一次进程是否已经开启过了
        try
        {
            //获取所有与指定名称匹配的进程 实际上只允许有一个 根据名称匹配确实也做不出多进程
            Process[] processes = Process.GetProcessesByName(processInfo.ProcessName);
            if (processes.Length == 1)
            {
                processInfo.Process = processes[0];
                processInfo.Running = true;
                return true;
            }
        }
        catch
        {
            return false;
        }
        //经过判断 进程确实没有开启
        Process p = new();
        p.StartInfo.FileName = processInfo.ProcessStartingOptions!.Path;
        p.StartInfo.Arguments = processInfo.ProcessStartingOptions!.Parameters;
        //设置显示方式
        switch (processInfo.ProcessStartingOptions.ShowingOption)
        {
            case ShowingOptions.Hide:
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                break;
            case ShowingOptions.Show:
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                break;
            default:
                break;
        }
        processInfo.Process = p;
        return await TaskUtils.RetryManyTimes(()=> { p.Start(); }, processInfo.ProcessStartingOptions.OvertimeTime.GetValueOrDefault()*1000,times);
    }
    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="processInfo"></param>
    [RelayCommand]
    public void Stop(ProcessInfo processInfo)
    {
        if (processInfo.Running == false) return;
        //可能程序已经挂了但是监视器是一秒刷新一次，状态没有更新，因此还要检测一次
        try
        {
            //获取所有与指定名称匹配的进程 实际上只允许有一个 根据名称匹配确实也做不出多进程
            Process[] processes = Process.GetProcessesByName(processInfo.ProcessName);
            if (processes.Length != 1)
            {
                processInfo.Running = false;
                return;
            }
        }
        catch
        {
            return;
        }
        processInfo.Process?.Kill();
    }
    #region show&hide
    /// <summary>
    /// Show和Hide要调用WindowsAPI
    /// </summary>
    // P/Invoke 声明
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    // 常量定义
    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

    [RelayCommand]
    public void Show(ProcessInfo processInfo)
    {
        if (processInfo.Process == null) return;
        IntPtr hWnd = processInfo.Process.MainWindowHandle;
        if (hWnd == IntPtr.Zero) return;
        ShowWindow(hWnd, SW_SHOW);
    }
    [RelayCommand]
    public void Hide(ProcessInfo processInfo)
    {
        if (processInfo.Process == null) return;
        IntPtr hWnd = processInfo.Process.MainWindowHandle;
        if (hWnd == IntPtr.Zero) return;
        ShowWindow(hWnd, SW_HIDE);
    }
    #endregion
    [RelayCommand]
    public void Edit(ProcessInfo processInfo)
    {
        var res = _dialogService.OpenDialog<EditDialogView, EditDialogViewModel, ProcessStartingOptions>("修改信息", processInfo);
        if (res != null)
        {
            //传的都是引用，直接改
            processInfo.ProcessStartingOptions = res;
        }
        var newcfg = Processes.Select(p => p.ProcessStartingOptions);
        string outputPath = @"opt.json";
        JsonUtils.WriteToJson<IEnumerable<ProcessStartingOptions?>>(outputPath, newcfg);
    }
    [RelayCommand]
    public void Delete(ProcessInfo processInfo)
    {
        var res = _dialogService.OpenDialog<DeleteConfirmDialogView, DeleteConfiremDialogViewModel, DialogResult?>("警告", "确认删除？");
        //取消
        if (null == res) return;
        //1关闭进程（要关闭吗？我们只是不监视他了，要不要关闭归我管吗？）2移除监视器 3删除processInfo 4修改json
        if (null != processInfo.Watcher) processInfo.Watcher.Dispose();
        if (null != processInfo.Process) processInfo.Process.Kill();
        Processes.Remove(processInfo);
        var newcfg = Processes.Select(p => p.ProcessStartingOptions);
        string outputPath = @"opt.json";
        JsonUtils.WriteToJson<IEnumerable<ProcessStartingOptions?>>(outputPath, newcfg);
    }
    [RelayCommand]
    public void ShowInfo(ProcessInfo processInfo)
    {
        var _ = _dialogService.OpenDialog<InfoDialogView, InfoDialogViewModel, object>("进程信息", processInfo);
    }
}
