using CommunityToolkit.Mvvm.Input;
using ProcessManager.Resources;
using System.Globalization;

namespace ProcessManager.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel()
    {

    }
    [RelayCommand]
    public void LangChange(string culture)
    {
        LanguageManager.Instance.ChangeLanguage(new CultureInfo(culture));
    }
}
