using CommunityToolkit.Mvvm.ComponentModel;

namespace ProcessManager.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public List<string> ErrorList { get; set; } = new List<string>();
}
