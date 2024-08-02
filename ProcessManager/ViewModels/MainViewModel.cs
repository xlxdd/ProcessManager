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
        Functions = [
            new(){Name="f_process",Command=NavigateCommand,ViewName="Process"},
            new(){Name="f_setting",Command=NavigateCommand,ViewName="Settings"},
        ];
        ///设置初始页面
        Serilog.Log.Information("Directing to ProcessView");
        CurrentView = App.Current.Container.ResolveNamed<UserControl>("Process");
        Serilog.Log.Information("ProcessView aquired");
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
        Serilog.Log.Information($"Directing to {viewName}View");
        CurrentView = App.Current.Container.ResolveNamed<UserControl>(viewName);
        Serilog.Log.Information($"{viewName}View aquired");
    }
    [RelayCommand]
    public static void Hide()
    {
        App.Current.MainWindow.Hide();
        Serilog.Log.Information("MainWindowHided");
    }
    [RelayCommand]
    public static void Minimize()
    {
        App.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        Serilog.Log.Information("MainWindowMinimized");
    }
    [RelayCommand]
    public static void Show()
    {
        App.Current.MainWindow.Show();
        Serilog.Log.Information("MainWindowShowed");
    }
    [RelayCommand]
    public static void Exit()
    {
        Serilog.Log.Information("-----------Exit-----------");
        //Serilog.Log.CloseAndFlush();
        App.Current.Shutdown();
    }
}
