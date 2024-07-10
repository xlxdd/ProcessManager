using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.Data;
using System.Windows.Controls;

namespace ProcessManager.ViewModels;
public partial class MainViewModel : ViewModelBase
{
    /// <summary>
    /// 控件容器
    /// </summary>
    [ObservableProperty]
    UserControl currentView;
    /// <summary>
    /// 功能列表
    /// </summary>
    [ObservableProperty]
    IEnumerable<FunctionButton> functions;
    public MainViewModel()
    {
        ///设置功能列表
        Functions = new List<FunctionButton>() {
            new FunctionButton(){Name="进程管理"},
            new FunctionButton(){Name="设置"},
        };
        ///设置初始页面
        CurrentView = App.Current.Container.ResolveNamed<UserControl>("Process");
    }
}
