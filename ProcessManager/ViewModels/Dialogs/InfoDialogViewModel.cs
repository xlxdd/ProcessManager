using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.Converters;
using ProcessManager.Data;
using ProcessManager.Services_Interfaces;
using System.Diagnostics;
using System.IO;

namespace ProcessManager.ViewModels.Dialogs;

public partial class InfoDialogViewModel : DialogBase, IDialogResult<object>
{
    [ObservableProperty]
    private ProcessStartingOptions? options;
    public string? ShowingOption { get; set; }
    public string? ClosingOption { get; set; }
    public string? StartingOption { get; set; }
    public InfoDialogViewModel()
    {

    }
    public override void GetParam(object? p)
    {
        var processInfo = p as ProcessInfo;
        Options = processInfo!.ProcessStartingOptions;
        ShowingOption = EnumDescriptionConverter.GetEnumDescription(Options!.ShowingOption);
        ClosingOption = EnumDescriptionConverter.GetEnumDescription(Options!.ClosingOption);
        StartingOption = EnumDescriptionConverter.GetEnumDescription(Options!.StartingOption);
    }
    [RelayCommand]
    public void GetPath()
    {
        var filePath = Options!.Path;
        var folderPath = Path.GetDirectoryName(filePath);

        if (Directory.Exists(folderPath))
        {
            Process.Start("explorer.exe", folderPath);
        }
    }
    public object GetResult()
    {
        return new object();
    }
}
