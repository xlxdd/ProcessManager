using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using ProcessManager.Resources;
using ProcessManager.Utils;
using System.Globalization;
using System.IO;
using System.Text.Json.Nodes;

namespace ProcessManager.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly IConfigurationRoot _configurationCenter;
    private readonly string execFileName = Environment.ProcessPath!;
    public bool Lang { get; set; }
    public bool AutoLaunch { get; set; }
    public SettingsViewModel(IConfigurationRoot configurationCenter)
    {
        _configurationCenter = configurationCenter;
        Lang = _configurationCenter.GetSection("Lang").Value == "en-US";
        AutoLaunch = _configurationCenter.GetSection("AutoLaunch").Value == "true";
    }
    [RelayCommand]
    public void LangChange(string culture)
    {
        _configurationCenter["Lang"] = culture;
        try
        {
            string json = File.ReadAllText("settings.json");
            JsonNode root = JsonNode.Parse(json)!;
            root![nameof(Lang)] = culture;
            File.WriteAllText("settings.json", root.ToJsonString());
        }
        finally
        {
            LanguageManager.Instance.ChangeLanguage(new CultureInfo(culture));
        }
    }
    [RelayCommand]
    public void Launch()
    {
        _configurationCenter[nameof(AutoLaunch)] = "true";
        try
        {
            string json = File.ReadAllText("settings.json");
            JsonNode root = JsonNode.Parse(json)!;
            root![nameof(AutoLaunch)] = "true";
            File.WriteAllText("settings.json", root.ToJsonString());
        }
        finally
        {
            RKUtils.Set(execFileName);
        }
    }
    [RelayCommand]
    public void NotLaunch()
    {
        _configurationCenter[nameof(AutoLaunch)] = "false";
        try
        {
            string json = File.ReadAllText("settings.json");
            JsonNode root = JsonNode.Parse(json)!;
            root![nameof(AutoLaunch)] = "false";
            File.WriteAllText("settings.json", root.ToJsonString());
        }
        finally
        {
            RKUtils.Delete(execFileName);
        }
    }
}
