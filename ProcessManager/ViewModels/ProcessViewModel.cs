using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.Data;
using ProcessManager.Services_Interfaces;
using ProcessManager.ViewModels.Dialogs;
using ProcessManager.Views.Dialogs;
using System.Collections.ObjectModel;

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
        //TODO:读取json配置add到Processes集合中
    }
    [RelayCommand]
    public void Add()
    {
        var res = _dialogService.OpenDialog<AddDialogView, AddDialogViewModel, ProcessStartingOptions>("添加进程");
        if (res != null)
        {
            Processes.Add(new ProcessInfo { ProcessStartingOptions = res });
            Console.WriteLine("Successfully received");
        }
    }
}
