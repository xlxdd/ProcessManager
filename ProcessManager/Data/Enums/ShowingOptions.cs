using System.ComponentModel;

namespace ProcessManager.Data.Enums;
/// <summary>
/// 显示设置
/// </summary>
public enum ShowingOptions
{
    /// <summary>
    /// 隐藏
    /// </summary>
    [Description("隐藏")]
    Hide = 0,
    /// <summary>
    /// 显示
    /// </summary>
    [Description("显示")]
    Show = 1,
    /// <summary>
    /// 程序内部显示
    /// </summary>
    [Description("程序内部显示")]
    ShowInternal = 2,
}
