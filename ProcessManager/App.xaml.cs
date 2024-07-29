using Autofac;
using ProcessManager.Services_Interfaces;
using ProcessManager.ViewModels;
using ProcessManager.Views;
using System.Windows;
using Application = System.Windows.Application;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Controls;

namespace ProcessManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IContainer Container { get; }
        public static new App Current => (App)Application.Current;
        public App()
        {
            #region Configuration
            #endregion
            #region IOC
            var builder = new ContainerBuilder();
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
            base.OnStartup(e);
            MainWindow = Container.Resolve<MainWindow>();
            MainWindow.Show();
        }
    }
}
