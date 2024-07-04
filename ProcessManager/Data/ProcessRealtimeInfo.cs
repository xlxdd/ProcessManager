using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.Data.Enums;

namespace ProcessManager.Data;
/// <summary>
/// 进程实时信息
/// 包含进程启动状态，CPU占有率，内存占有率等实时信息
/// </summary>
public partial class ProcessRealtimeInfo : ObservableObject
{
    /// <summary>
    /// 优先级 同时也视为主键
    /// </summary>
    public ushort Priority { get; set; }
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
