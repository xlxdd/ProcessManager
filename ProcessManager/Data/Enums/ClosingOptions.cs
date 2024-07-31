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
    [Description("enum_forceclose")]
    ForceClose = 0,
    /// <summary>
    /// 信号关闭
    /// </summary>
    [Description("enum_signalclose")]
    SignalClose = 1,
    /// <summary>
    /// SendMessage关闭
    /// </summary>
    [Description("enum_messageclose")]
    SendMessageClose = 2,
    /// <summary>
    /// 程序自行控制关闭
    /// </summary>
    [Description("enum_selfclose")]
    SelfControlClose = 3,
    /// <summary>
    /// 不进行控制
    /// </summary>
    [Description("enum_donothing")]
    NoControl = 4,
}
