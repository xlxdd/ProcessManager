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
        public App()
        {
            #region Configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("settings.json", false, true);
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
