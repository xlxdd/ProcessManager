using ProcessManager.ViewModels.Dialogs;
using ProcessManager.Views.Dialogs;
using UserControl = System.Windows.Controls.UserControl;

namespace ProcessManager.Services_Interfaces;

public class DialogService : IDialogService
{
    public Result OpenDialog<View, ViewModel, Result>(string title, object p) where View : UserControl, new() where ViewModel : DialogBase, IDialogResult<Result>, new() where Result : class?
    {
        IDialogWindow window = new DialogWindow();
        var vm = new DialogWindowViewModel<View, ViewModel, Result>(title, p);
        window.DataContext = vm;
        if (window.ShowDialog() == true)
        {
            return vm.DialogResult;
        }
        else return null;
    }
}
