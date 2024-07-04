using Autofac;
using Microsoft.Extensions.Configuration;
using ProcessManager.ViewModels;
using ProcessManager.Views;
using System.Windows;
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
            var manager = new ConfigurationManager();
            var configBuilder = (IConfigurationBuilder)manager;
            var configProvider = (IConfigurationRoot)manager;
            #endregion
            #region IOC
            var builder = new ContainerBuilder();
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<ProcessView>().Named<UserControl>("Process").SingleInstance();
            builder.RegisterType<ProcessViewModel>().SingleInstance();
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
