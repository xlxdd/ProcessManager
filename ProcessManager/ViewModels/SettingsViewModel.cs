using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using ProcessManager.Resources;
using System.Globalization;
using System.IO;
using System.Text.Json.Nodes;

namespace ProcessManager.ViewModels;

public partial class SettingsViewModel: ViewModelBase
{
    private readonly IConfigurationRoot _configurationCenter;
    public bool Lang { get;set; }
    public bool Launch { get; set; }
    public SettingsViewModel(IConfigurationRoot configurationCenter)
    {
        _configurationCenter = configurationCenter;
        Lang = _configurationCenter.GetSection("Lang").Value == "en-US";
        Launch = _configurationCenter.GetSection("AutoLaunch").Value == false;
    }
    [RelayCommand]
    public void LangChange(string culture)
    {
        _configurationCenter["Lang"] = culture;
        try
        {
            string json = File.ReadAllText("settings.json");
            JsonNode root = JsonNode.Parse(json)!;
            root!["Lang"] = culture;
            File.WriteAllText("settings.json", root.ToJsonString());
        }
        finally
        {
            LanguageManager.Instance.ChangeLanguage(new CultureInfo(culture));
        }
    }
    [RelayCommand]
    public void AutoLaunch()
    {

    }
    [RelayCommand]
    public void NotLaunch()
    {

    }
}
