using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.Utils;
using System.Diagnostics;

namespace ProcessManager.Data;
/// <summary>
/// 进程信息类
/// </summary>
public partial class ProcessInfo : ObservableObject
{
    public string ProcessName { get; set; }
    public List<Process> processes { get; set; } = new();
    public ProcessUtils.GeneralProcessWatcher? watcher { get; set; }
    /// <summary>
    /// 进程信息包含实时信息
    /// </summary>
    [ObservableProperty]
    private ProcessRealtimeInfo? processRealtimeInfo;
    /// <summary>
    /// 进程信息包含启动配置
    /// </summary>
    public ProcessStartingOptions? ProcessStartingOptions { get; set; }
}
