using ProcessManager.ViewModels;
using UserControl = System.Windows.Controls.UserControl;

namespace ProcessManager.Views
{
    /// <summary>
    /// SettingsView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView(SettingsViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
