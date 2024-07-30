using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.Utils;
using System.Diagnostics;

namespace ProcessManager.Data;
/// <summary>
/// 进程信息类
/// </summary>
public partial class ProcessInfo : ObservableObject
{
    /// <summary>
    /// 进程名称
    /// </summary>
    public string ProcessName { get; set; }
    public Process? Process { get; set; }
    /// <summary>
    /// 进程是否在运行 用于绑定button的enable状态
    /// </summary>
    [ObservableProperty]
    private bool? running;
    /// <summary>
    /// 监视器
    /// </summary>
    public ProcessUtils.GeneralProcessWatcher? Watcher { get; set; }
    /// <summary>
    /// 进程信息包含实时信息 该信息为统计信息 即子进程的情况下统计求和
    /// </summary>
    [ObservableProperty]
    private ProcessRealtimeInfo? processRealtimeInfo;
    /// <summary>
    /// 进程信息包含启动配置
    /// </summary>
    public ProcessStartingOptions? ProcessStartingOptions { get; set; }
}
