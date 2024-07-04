using ProcessManager.ViewModels;
using System.Windows.Controls;

namespace ProcessManager.Views;

/// <summary>
/// ProcessView.xaml 的交互逻辑
/// </summary>
public partial class ProcessView : UserControl
{
    public ProcessView(ProcessViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
    }
}
