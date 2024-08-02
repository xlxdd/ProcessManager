using Autofac;
using Microsoft.Extensions.Configuration;
using ProcessManager.Services_Interfaces;
using ProcessManager.ViewModels;
using ProcessManager.Views;
using Serilog;
using System.IO;
using System.Windows;
using Application = System.Windows.Application;
using UserControl = System.Windows.Controls.UserControl;

namespace ProcessManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Autofac.IContainer Container { get; }
        public static new App Current => (App)Application.Current;
        public string? EXEDirectory { get; set; }
        private static System.Threading.Mutex mutex=new(true, "PM");
        #region Exception Handler
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"CurrentDomain_UnhandledException：" + (e.ExceptionObject as Exception).Message);
        }
        static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true; //使用e.Handled能防止程序崩溃
            MessageBox.Show($"Current_DispatcherUnhandledException：" + e.Exception.Message);
        }
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show($"TaskScheduler_UnobservedTaskException：" + e.Exception.Message);
        }
        #endregion
        public App()
        {
            #region Exception Handle
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            #endregion
            #region Configuration
            var configurationBuilder = new ConfigurationBuilder();
            EXEDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()!.Location);
            configurationBuilder.SetBasePath(EXEDirectory!).AddJsonFile("settings.json", false, true);
            var configurationCenter = configurationBuilder.Build();
            #endregion
            #region Log
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            #endregion
            #region IOC
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IConfigurationRoot>(configurationCenter);
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<ProcessView>().Named<UserControl>("Process").SingleInstance();
            builder.RegisterType<ProcessViewModel>().SingleInstance();
            builder.RegisterType<SettingsView>().Named<UserControl>("Settings").SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            Container = builder.Build();
            #endregion
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("程序已经在运行！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                Shutdown();
            }
            Serilog.Log.Information("App starting");
            base.OnStartup(e);
            Serilog.Log.Information("Requie MainWindow");
            MainWindow = Container.Resolve<MainWindow>();
            Serilog.Log.Information("MainWindow aquired");
            Serilog.Log.Information("ShowMainWindow");
            MainWindow.Show();
            Serilog.Log.Information("MainWindowShowed");
        }
    }
}
