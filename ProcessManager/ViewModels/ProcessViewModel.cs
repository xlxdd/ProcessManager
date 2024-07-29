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
    public ProcessViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
        ///设置功能列表
        Functions = new List<FunctionButton>() {
            new FunctionButton(){Name="添加进程",Command = AddCommand},
            new FunctionButton(){Name="关闭所有进程"},
            new FunctionButton(){Name="启动所有进程"},
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
            Processes.Add(new ProcessInfo { ProcessName = StringUtils.FullNameToProcessName(opt.Path), ProcessStartingOptions = opt });
        }
        await Task.Run(() =>
        {
            foreach (var p in Processes)
            {
                //这一步会非常耗时
                //添加性能监视
                p.watcher = new ProcessUtils.GeneralProcessWatcher(p.ProcessName, p.ProcessStartingOptions.ProcessCount);
            }
        });
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
            if (null != p && null != p.watcher)
                p.ProcessRealtimeInfo = p.watcher.Watch();
        }
    }
    /// <summary>
    /// 重启超过占用的进程
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RestartProcess(Object? sender, EventArgs e)
    {
        foreach (var p in Processes)
        {
            if (true == p.ProcessStartingOptions!.EnableMaxCPUUsage)
            {
                if (p.ProcessRealtimeInfo!.CPUUsage > p.ProcessStartingOptions.MaxCPUUsage)
                {
                    Stop(p);
                    Start(p);
                }
            }
            if (true == p.ProcessStartingOptions!.EnableMaxRAMUsage)
            {
                if (p.ProcessRealtimeInfo!.RAMUsage > p.ProcessStartingOptions.MaxRAMUsage)
                {
                    Stop(p);
                    Start(p);
                }
            }
        }
    }
    [RelayCommand]
    public void Add()
    {
        //为什么这里传不了null
        var res = _dialogService.OpenDialog<AddDialogView, AddDialogViewModel, ProcessStartingOptions>("添加进程", new object());
        if (res != null)
        {
            Processes.Add(new ProcessInfo { ProcessStartingOptions = res });
        }
        var newcfg = Processes.Select(p => p.ProcessStartingOptions);
        string outputPath = @"opt.json";
        JsonUtils.WriteToJson<IEnumerable<ProcessStartingOptions?>>(outputPath, newcfg);
    }
    /// <summary>
    /// 开启全部
    /// </summary>
    [RelayCommand]
    public void StartAll()
    {
        foreach (var processInfo in Processes)
        {
            Start(processInfo);
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
    public void Start(ProcessInfo processInfo)
    {
        //获取进程数量（考虑多进程的情况）
        int processNum = processInfo.ProcessStartingOptions.ProcessCount;
        for (int i = 0; i < processNum; i++)
        {
            Process p = new();
            processInfo.processes.Add(p);
            p.StartInfo.FileName = processInfo.ProcessStartingOptions.Path;
            p.StartInfo.Arguments = processInfo.ProcessStartingOptions.Parameters;
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
            p.Start();
        }
    }
    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="processInfo"></param>
    [RelayCommand]
    public void Stop(ProcessInfo processInfo)
    {
        foreach (Process p in processInfo.processes)
        {
            //使用kill,只有Kill能关闭没有图形界面的进程
            p.Kill(true);
        }
    }
    /// <summary>
    /// Show和Hide实际上要进行重启 不重启怎么做到显示界面，我再查查
    /// </summary>
    [RelayCommand]
    public void Show(ProcessInfo processInfo)
    {
        foreach (Process p in processInfo.processes)
        {
            p.StartInfo.CreateNoWindow = false;
        }
    }
    [RelayCommand]
    public void Hide(ProcessInfo processInfo)
    {

    }
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
        if (null != processInfo.watcher) processInfo.watcher.Dispose();
        if (null != processInfo.processes) { foreach (var p in processInfo.processes) p.Kill(); }
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
