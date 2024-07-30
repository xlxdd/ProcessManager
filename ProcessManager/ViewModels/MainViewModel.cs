using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.Data;
using ProcessManager.Resources;
using UserControl = System.Windows.Controls.UserControl;

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
            new(){Name="f_process",Command=NavigateCommand,ViewName="Process"},
            new(){Name="f_setting",Command=NavigateCommand,ViewName="Settings"},
        };
        ///设置初始页面
        CurrentView = App.Current.Container.ResolveNamed<UserControl>("Process");
        LanguageManager.Instance.PropertyChanged += (s, e) =>
        {
            foreach (FunctionButton button in Functions)
            {
                button.ChangeCulture();
            }
        };
    }
    [RelayCommand]
    public void Navigate(string viewName)
    {
        CurrentView = App.Current.Container.ResolveNamed<UserControl>(viewName);
    }
    [RelayCommand]
    public void Hide()
    {
        App.Current.MainWindow.Hide();
    }
    [RelayCommand]
    public void Minimize()
    {
        App.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
    }
    [RelayCommand]
    public void Show()
    {
        App.Current.MainWindow.Show();
    }
    [RelayCommand]
    public void Exit()
    {
        App.Current.Shutdown();
    }
}
