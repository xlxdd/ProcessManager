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
    [Description("enum_running")]
    Running = 0,
    /// <summary>
    /// 关闭
    /// </summary>
    [Description("enum_closed")]
    Closed = 1,
}
