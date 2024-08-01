using Microsoft.Win32;
using System.IO;

namespace ProcessManager.Utils;
/// <summary>
/// 注册表帮助类
/// </summary>
public static class RKUtils
{
    public static void Set(string execPath, string regPath = @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run")
    {
        string fileName = Path.GetFileName(execPath);
        var rk = Registry.CurrentUser.OpenSubKey(regPath, true);
        rk ??= Registry.CurrentUser.CreateSubKey(regPath);
        rk.SetValue(fileName, execPath);
        rk.Close();
    }
    public static void Delete(string execPath, string regPath = @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run")
    {
        string fileName = Path.GetFileName(execPath);
        var rk = Registry.CurrentUser.OpenSubKey(regPath, true);
        rk ??= Registry.CurrentUser.CreateSubKey(regPath);
        rk.DeleteValue(fileName, false);
        rk.Close();
    }
}
