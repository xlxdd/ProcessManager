using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.Data.Enums;

namespace ProcessManager.Data;
/// <summary>
/// 进程实时信息
/// 包含进程启动状态，CPU占用率，内存占用率等实时信息
/// </summary>
public partial class ProcessRealtimeInfo : ObservableObject
{
    /// <summary>
    /// CPU占用率
    /// 单位 %
    /// </summary>
    public double? CPUUsage { get; set; }
    /// <summary>
    /// 内存使用量
    /// 单位MB
    /// </summary>
    public ushort? RAMUsage { get; set; }
    /// <summary>
    /// 进程状态
    /// </summary>
    public ProcessStatus ProcessStatus { get; set; }
}
