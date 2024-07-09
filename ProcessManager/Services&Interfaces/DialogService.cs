using ProcessManager.ViewModels.Dialogs;
using ProcessManager.Views.Dialogs;
using System.Windows.Controls;

namespace ProcessManager.Services_Interfaces;

public class DialogService : IDialogService
{
    public Result OpenDialog<View, ViewModel, Result>(string title) where View : UserControl, new() where ViewModel : IDialogResult<Result>, new() where Result:class
    {
        IDialogWindow window = new DialogWindow();
        var vm = new DialogWindowViewModel<View, ViewModel, Result>(title);
        window.DataContext = vm;
        if (window.ShowDialog() == true)
        {
            return vm.DialogResult;
        }
        else return null;
    }
}
