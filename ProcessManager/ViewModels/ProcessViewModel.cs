using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ProcessManager.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls;

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
    public ProcessViewModel()
    {
        ///设置功能列表
        Functions = new List<FunctionButton>() {
            new FunctionButton(){Name="添加进程"},
            new FunctionButton(){Name="关闭所有进程"},
            new FunctionButton(){Name="启动所有进程"},
        };
        Processes = new ObservableCollection<ProcessInfo>();
        //TODO:读取json配置add到Processes集合中
    }
}
