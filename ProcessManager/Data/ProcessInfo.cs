using CommunityToolkit.Mvvm.ComponentModel;

namespace ProcessManager.Data;
/// <summary>
/// 进程信息类
/// </summary>
public partial class ProcessInfo : ObservableObject
{
    /// <summary>
    /// 进程信息包含实时信息
    /// </summary>
    public ProcessRealtimeInfo? ProcessRealtimeInfo { get; set; }
    /// <summary>
    /// 进程信息包含启动配置
    /// </summary>
    public ProcessStartingOptions? ProcessStartingOptions { get; set; }
}
