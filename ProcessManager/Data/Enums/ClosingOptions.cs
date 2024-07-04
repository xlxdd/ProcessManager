using System.ComponentModel;

namespace ProcessManager.Data.Enums;
/// <summary>
/// 关闭方式
/// </summary>
public enum ClosingOptions
{
    /// <summary>
    /// 强制关闭
    /// </summary>
    [Description("强制关闭")]
    ForceClose = 0,
    /// <summary>
    /// 信号关闭
    /// </summary>
    [Description("信号关闭")]
    SignalClose = 1,
    /// <summary>
    /// SendMessage关闭
    /// </summary>
    [Description("SendMessage关闭")]
    SendMessageClose = 2,
    /// <summary>
    /// 程序自行控制关闭
    /// </summary>
    [Description("程序自行控制关闭")]
    SelfControlClose = 3,
    /// <summary>
    /// 不进行控制
    /// </summary>
    [Description("不进行控制")]
    NoControl = 4,
}
