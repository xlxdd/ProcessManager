using System.Windows.Controls;

namespace ProcessManager.Services_Interfaces;

public interface IDialogService
{
    Result OpenDialog<View, ViewModel, Result>(string title) where View : UserControl, new() where ViewModel : IDialogResult<Result>, new()where Result:class;
}
