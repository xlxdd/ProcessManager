using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.Data;
using ProcessManager.Data.Enums;
using ProcessManager.Resources;
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
    private readonly Dog watchDog;
    private static readonly int times = 10;
    public ProcessViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
        ///设置功能列表
        Functions = [
            new (){Name="f_add",Command=AddCommand},
            new (){Name="f_closeAll",Command=StopAllCommand},
            new (){Name="f_startAll",Command=StartAllCommand},
        ];
        LanguageManager.Instance.PropertyChanged += (s, e) =>
        {
            foreach (FunctionButton button in Functions)
            {
                button.ChangeCulture();
            }
        };
        Processes = [];
        //读配置
        Init();
        //初始化看门狗
        watchDog = new Dog(RefreshProcessRealTimeInfo);
        watchDog.AddAction(RestartProcess);
        watchDog.Start();
    }
    private async void Init()
    {
        Serilog.Log.Information("Reading ProcessInfo From opt.json");
        string filePath = App.Current.EXEDirectory+@"\\opt.json";
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
                        Serilog.Log.Verbose($"{p.ProcessName} is already running");
                        p.Process = processes[0];
                        p.Running = true;
                    }
                    else if (processes.Length == 0 && p.ProcessStartingOptions!.StartingOption == StartingOptions.OpenWhenBoot)
                    {
                        Serilog.Log.Verbose($"{p.ProcessName} need starting");
                        await Task.Delay(p.ProcessStartingOptions.DelayTime.GetValueOrDefault());
                        //超时重启的逻辑写在Start()中，因为理论上每一次启动时都需要超时重启，并不应该是只有第一次启动时需要考虑超时重启
                        await Start(p);
                    }
                }
                catch
                {
                    Serilog.Log.Error($"Failed to Get ProcessByName,Name = {p.ProcessName}");
                }
                //这一步会非常耗时
                //传入实例名称 添加性能监视器
                Serilog.Log.Verbose($"Adding WatchDog to {p.ProcessName}");
                try
                {
                    p.Watcher = new ProcessUtils.GeneralProcessWatcher(p.ProcessName!);
                }
                catch
                {
                    Serilog.Log.Error($"Failed to Add WatchDog to {p.ProcessName}");
                }
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
                    Serilog.Log.Warning($"{p.ProcessName} has reached max CPU usage!");
                    Stop(p);
                    await Start(p);
                }
            }
            if (true == p.ProcessStartingOptions!.EnableMaxRAMUsage)
            {
                if (p.ProcessRealtimeInfo!.RAMUsage > p.ProcessStartingOptions.MaxRAMUsage)
                {
                    Serilog.Log.Warning($"{p.ProcessName} has reached max RAM usage!");
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
        Serilog.Log.Information("Adding new Process...");
        var res = _dialogService.OpenDialog<AddDialogView, AddDialogViewModel, ProcessStartingOptions>("t_add", new object());
        if (res != null)
        {
            var name = StringUtils.FullNameToProcessName(res!.Path!);
            Processes.Add(new ProcessInfo { ProcessName = name, ProcessStartingOptions = res, Running = false, Watcher = new ProcessUtils.GeneralProcessWatcher(name) });
            Serilog.Log.Information($"Added a new Process,nane = {name}");
        }
        else Serilog.Log.Information($"Canceled to add a Process");
        var newcfg = Processes.Select(p => p.ProcessStartingOptions);
        string outputPath = App.Current.EXEDirectory+@"\\opt.json";
        JsonUtils.WriteToJson<IEnumerable<ProcessStartingOptions?>>(outputPath, newcfg);
    }
    /// <summary>
    /// 开启全部
    /// </summary>
    [RelayCommand]
    public async Task StartAll()
    {
        Serilog.Log.Information("Starting all Processes...");
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
        Serilog.Log.Information("Killing all Processes...");
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
    public static async Task<bool> Start(ProcessInfo processInfo)
    {
        Serilog.Log.Information($"Starting Process {processInfo.ProcessName}");
        //先用性能监视器拦截一次
        if (processInfo.Running == true)
        {
            Serilog.Log.Information($"{processInfo.ProcessName} is already running");
            return true;
        }
        //可能程序在跑但是监视器是一秒刷新一次，状态没有更新，因此还要检测一次进程是否已经开启过了
        try
        {
            //获取所有与指定名称匹配的进程 实际上只允许有一个 根据名称匹配确实也做不出多进程
            Process[] processes = Process.GetProcessesByName(processInfo.ProcessName);
            if (processes.Length == 1)
            {
                Serilog.Log.Information($"{processInfo.ProcessName} is already running");
                processInfo.Process = processes[0];
                processInfo.Running = true;
                return true;
            }
        }
        catch
        {
            Serilog.Log.Error($"Failed to Find {processInfo.ProcessName}");
            return false;
        }
        //经过判断 进程确实没有开启
        Process p = new();
        p.StartInfo.FileName = processInfo.ProcessStartingOptions!.Path;
        p.StartInfo.Arguments = processInfo.ProcessStartingOptions!.Parameters;
        processInfo.Process = p;
        var res = await TaskUtils.RetryManyTimes(() =>
        {
            p.Start();
            {
                Serilog.Log.Debug($"Hide {processInfo.ProcessName}");
            }
            Serilog.Log.Information($"Successfully started Process {processInfo.ProcessName}");
        },
        () =>
        {
            Serilog.Log.Warning($"Failed to start Process {processInfo.ProcessName} in time limit");
        },
        processInfo.ProcessStartingOptions.OvertimeTime.GetValueOrDefault() * 1000,
        times);
        if (processInfo.ProcessStartingOptions.ShowingOption == ShowingOptions.Hide)
        {
            //这个Thread.Sleep(1000);困扰了我几乎一天
            Thread.Sleep(1000);
            Hide(processInfo);
        }
        return res;
    }
    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="processInfo"></param>
    [RelayCommand]
    public static void Stop(ProcessInfo processInfo)
    {
        Serilog.Log.Information($"Killing Process {processInfo.ProcessName}");
        if (processInfo.Running == false)
        {
            Serilog.Log.Information($"{processInfo.ProcessName} is not running");
            return;
        }
        //可能程序已经挂了但是监视器是一秒刷新一次，状态没有更新，因此还要检测一次
        try
        {
            //获取所有与指定名称匹配的进程 实际上只允许有一个 根据名称匹配确实也做不出多进程
            Process[] processes = Process.GetProcessesByName(processInfo.ProcessName);
            if (processes.Length != 1)
            {
                processInfo.Running = false;
                Serilog.Log.Information($"{processInfo.ProcessName} is not running");
                return;
            }
        }
        catch
        {
            Serilog.Log.Error($"Failed to Find {processInfo.ProcessName}");
            return;
        }
        processInfo.Watcher!.Dispose();
        processInfo.Process?.Kill();
        Serilog.Log.Information($"Process {processInfo.ProcessName} has been killed");
    }
    #region show&hide
    /// <summary>
    /// Show和Hide要调用WindowsAPI
    /// </summary>
    [LibraryImport("user32.dll", EntryPoint = "ShowWindow")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);
    // 常量定义
    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

    [RelayCommand]
    public static void Show(ProcessInfo processInfo)
    {
        if (processInfo.Process == null) return;
        IntPtr hWnd = processInfo.Process.MainWindowHandle;
        if (hWnd == IntPtr.Zero) return;
        ShowWindow(hWnd, SW_SHOW);
    }
    [RelayCommand]
    public static void Hide(ProcessInfo processInfo)
    {
        if (processInfo.Process == null)
        {
            return;
        }
        IntPtr hWnd = processInfo.Process.MainWindowHandle;
        if (hWnd == IntPtr.Zero)
        {
            return;
        }
        ShowWindow(hWnd, SW_HIDE);
    }
    #endregion
    [RelayCommand]
    public void Edit(ProcessInfo processInfo)
    {
        var res = _dialogService.OpenDialog<EditDialogView, EditDialogViewModel, ProcessStartingOptions>("t_edit", processInfo);
        if (res != null)
        {
            //传的都是引用，直接改
            processInfo.ProcessStartingOptions = res;
        }
        var newcfg = Processes.Select(p => p.ProcessStartingOptions);
        string outputPath =App.Current.EXEDirectory+@"\\opt.json";
        JsonUtils.WriteToJson<IEnumerable<ProcessStartingOptions?>>(outputPath, newcfg);
    }
    [RelayCommand]
    public void Delete(ProcessInfo processInfo)
    {
        var res = _dialogService.OpenDialog<DeleteConfirmDialogView, DeleteConfiremDialogViewModel, DialogResult?>("t_del", "m_del");
        //取消
        if (null == res) return;
        //1关闭进程（要关闭吗？我们只是不监视他了，要不要关闭归我管吗？）2移除监视器 3删除processInfo 4修改json
        processInfo.Watcher?.Dispose();
        processInfo.Process?.Kill();
        Processes.Remove(processInfo);
        var newcfg = Processes.Select(p => p.ProcessStartingOptions);
        string outputPath = App.Current.EXEDirectory + @"\\opt.json";
        JsonUtils.WriteToJson<IEnumerable<ProcessStartingOptions?>>(outputPath, newcfg);
    }
    [RelayCommand]
    public void ShowInfo(ProcessInfo processInfo)
    {
        var _ = _dialogService.OpenDialog<InfoDialogView, InfoDialogViewModel, object>("t_detail", processInfo);
    }
}
