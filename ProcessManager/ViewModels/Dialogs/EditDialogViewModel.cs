using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.Data;
using ProcessManager.Data.Enums;
using ProcessManager.Services_Interfaces;
using ProcessManager.Utils;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ProcessManager.ViewModels.Dialogs;

public partial class EditDialogViewModel : DialogBase, IDialogResult<ProcessStartingOptions>
{
    [ObservableProperty]
    private ProcessStartingOptions? options;
    [ObservableProperty]
    private List<StartingOptions> startingOptions;
    [ObservableProperty]
    private List<ShowingOptions> showingOptions;
    [ObservableProperty]
    private List<ClosingOptions> closingOptions;
    public EditDialogViewModel()
    {
        Options = new();
        StartingOptions = EnumUtils.EnumToList<StartingOptions>();
        ShowingOptions = EnumUtils.EnumToList<ShowingOptions>();
        ClosingOptions = EnumUtils.EnumToList<ClosingOptions>();
    }
    public override void GetParam(object? p)
    {
        var param = p as ProcessInfo;
        if (null != param)
        {
            Options = param.ProcessStartingOptions;
        }
    }
    [RelayCommand]
    public void GetPath()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
        if (openFileDialog.ShowDialog() == true)
        {
            Options!.Path = openFileDialog.FileName;
        }
    }
    public ProcessStartingOptions GetResult()
    {
        return Options!;
    }
}
