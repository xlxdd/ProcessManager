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
            new FunctionButton(){Name="添加"},
            new FunctionButton(){Name="启动所有进程"},
            new FunctionButton(){Name="关闭所有进程"},
            new FunctionButton(){Name="信息"},
            new FunctionButton(){Name="升级"},
        };
        ///设置初始页面
        CurrentView = App.Current.Container.ResolveNamed<UserControl>("Process");
    }
}
