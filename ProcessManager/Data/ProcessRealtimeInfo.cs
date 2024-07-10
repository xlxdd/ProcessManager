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
    [ObservableProperty]
    private float? cPUUsage;
    /// <summary>
    /// 内存使用量
    /// 单位MB
    /// </summary>
    [ObservableProperty]
    private float? rAMUsage;
    /// <summary>
    /// 进程状态
    /// </summary>
    [ObservableProperty]
    private ProcessStatus processStatus;
}
