using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.Converters;
using ProcessManager.Data;
using ProcessManager.Services_Interfaces;

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
    public object GetResult()
    {
        return new object();
    }
}
