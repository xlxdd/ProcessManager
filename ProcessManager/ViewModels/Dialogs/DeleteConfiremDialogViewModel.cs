using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.Data;
using ProcessManager.Data.Enums;
using ProcessManager.Services_Interfaces;

namespace ProcessManager.ViewModels.Dialogs;

public partial class DeleteConfiremDialogViewModel : DialogBase, IDialogResult<DialogResult?>
{
    [ObservableProperty]
    private string? content;
    public DeleteConfiremDialogViewModel()
    {

    }
    public override void GetParam(object? p)
    {
        Content = p as string;
    }
    public DialogResult? GetResult()
    {
        return new DialogResult() { Result = DialogResults.Yes };
    }
}
