using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.Data;
using ProcessManager.Services_Interfaces;
using ProcessManager.Services_Interfaces.WatchDog;
using ProcessManager.ViewModels.Dialogs;
using ProcessManager.Views.Dialogs;
using System.Collections.ObjectModel;
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
        try
        {
            // 打开并读取文件内容
            using (StreamReader fileReader = new StreamReader(filePath))
            {
                // 从文件中获取JSON字符串
                string jsonContent = fileReader.ReadToEnd();

                // 使用JsonConvert.DeserializeObject反序列化JSON字符串为User对象
                IEnumerable<ProcessStartingOptions?> opts = JsonSerializer.Deserialize<IEnumerable<ProcessStartingOptions?>>(jsonContent)!;
                foreach (var opt in opts)
                {
                    Processes.Add(new ProcessInfo { ProcessStartingOptions = opt });
                }
            }
        }
        catch (FileNotFoundException)
        {
            throw;
        }
        //TODO:开启进程
        //开启后为所有进程创建监测器
        //假设已经读取了配置，并且创建了进程
        //foreach (var p in Processes)
        //{
        //    //这一步会非常耗时
        //    //实际上还需要加判断是否开启了cpu和内存占用的重启监测
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
        try
        {
            string outputPath = @"opt.json";
            // 将User对象序列化为JSON字符串
            string jsonOutput = JsonSerializer.Serialize(newcfg);

            // 将JSON字符串写入文件
            using (StreamWriter fileWriter = new StreamWriter(outputPath))
            {
                fileWriter.Write(jsonOutput);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
