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
    [Description("enum_hide")]
    Hide = 0,
    /// <summary>
    /// 显示
    /// </summary>
    [Description("enum_show")]
    Show = 1,
    ///// <summary>
    ///// 程序内部显示
    ///// </summary>
    //[Description("程序内部显示")]
    //ShowInternal = 2,
}
