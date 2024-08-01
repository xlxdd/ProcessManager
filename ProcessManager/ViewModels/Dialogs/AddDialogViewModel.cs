using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.Data;
using ProcessManager.Data.Enums;
using ProcessManager.Services_Interfaces;
using ProcessManager.Utils;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ProcessManager.ViewModels.Dialogs;

public partial class AddDialogViewModel : DialogBase, IDialogResult<ProcessStartingOptions>
{
    [ObservableProperty]
    private ProcessStartingOptions? options;
    [ObservableProperty]
    private List<StartingOptions> startingOptions;
    [ObservableProperty]
    private List<ShowingOptions> showingOptions;
    [ObservableProperty]
    private List<ClosingOptions> closingOptions;

    public AddDialogViewModel()
    {
        Options = new();
        StartingOptions = EnumUtils.EnumToList<StartingOptions>();
        ShowingOptions = EnumUtils.EnumToList<ShowingOptions>();
        ClosingOptions = EnumUtils.EnumToList<ClosingOptions>();
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
        int errors = 0;
        if (Options.EnableMaxCPUUsage != true) errors++;
        if (Options.EnableMaxRAMUsage != true) errors++;
        //ErrorList留着备用，虽然目前是只要知道有没有错误就行了
        if (ErrorList.Count > errors) return null;
        return Options!;
    }
}
