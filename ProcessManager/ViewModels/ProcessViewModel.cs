using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.Data;
using ProcessManager.Services_Interfaces;
using ProcessManager.Services_Interfaces.WatchDog;
using ProcessManager.Utils;
using ProcessManager.ViewModels.Dialogs;
using ProcessManager.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ProcessManager.Data.Enums;
using System.IO;
using System.Text.Json;

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
        //初始化看门狗
        watchDog = new Dog(RefreshProcessRealTimeInfo);
        Processes = new ObservableCollection<ProcessInfo>();
        watchDog.Start();
        Init();
    }
    private void Init()
    {
        string filePath = @"opt.json";
        var opts = JsonUtils.ReadfromJson<IEnumerable<ProcessStartingOptions?>>(filePath);
        foreach (var opt in opts)
        {
            Processes.Add(new ProcessInfo { ProcessStartingOptions = opt });
        }
        //TODO:开启进程（好像这一步不开启，在“开启全部”开启）
        //开启后为所有进程创建监测器
        //这里就有一个问题了，是开启进程后开始监测还是一直监测? 一直监测似乎比较方便
        //假设已经读取了配置，并且创建了进程
        //foreach (var p in Processes)
        //{
        //    //这一步会非常耗时
        //    p.watcher = new ProcessUtils.GeneralProcessWatcher(p.ProcessName);
        //}
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
            if (null != p.watcher)
                p.ProcessRealtimeInfo = p.watcher.Watch();
        }
    }
    [RelayCommand]
    public void Add()
    {
        var res = _dialogService.OpenDialog<AddDialogView, AddDialogViewModel, ProcessStartingOptions>("添加进程");
        if (res != null)
        {
            Processes.Add(new ProcessInfo { ProcessStartingOptions = res });
        }
        var newcfg = Processes.Select(p => p.ProcessStartingOptions);
        string outputPath = @"opt.json";
        JsonUtils.WriteToJson<IEnumerable<ProcessStartingOptions?>>(outputPath,newcfg);
    }
    /// <summary>
    /// 开启全部
    /// </summary>
    [RelayCommand]
    public void StartAll()
    {

    }
    [RelayCommand]
    public void StopAll()
    {

    }
    /// <summary>
    /// 开启
    /// </summary>
    [RelayCommand]
    public void Start(ProcessInfo processInfo)
    {
        //获取进程数量
        int processNum = processInfo.ProcessStartingOptions.ProcessCount;
        for(int i = 0; i < processNum; i++)
        {
            Process p = new Process();
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
    [RelayCommand]
    public void Stop(ProcessInfo processInfo)
    {
        foreach(Process p in processInfo.processes)
        {
            //这几个关闭方式我也看不出多大区别
            //Close方法会先尝试模拟用户操作进行关闭，如果不能，则强制关闭
            //大概是封装了CloseMainWindow和Kill方法
            p.Close();
        }
    }
    [RelayCommand]
    public void Show()
    {
        //这个有点抽象
    }
    [RelayCommand]
    public void Hide()
    {

    }
    [RelayCommand]
    public void Edit()
    {

    }
    [RelayCommand]
    public void Delete()
    {

    }
    [RelayCommand]
    public void ShowInfo()
    {

    }
}
