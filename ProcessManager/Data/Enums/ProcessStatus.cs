using System.ComponentModel;

namespace ProcessManager.Data.Enums;
/// <summary>
/// 进程状态
/// </summary>
public enum ProcessStatus
{
    /// <summary>
    /// 运行
    /// </summary>
    [Description("运行")]
    Running = 0,
    /// <summary>
    /// 关闭
    /// </summary>
    [Description("关闭")]
    Closed = 1,
    /// <summary>
    /// 阻塞
    /// </summary>
    [Description("阻塞")]
    Blocking = 2,
}
