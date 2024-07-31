using System.ComponentModel;

namespace ProcessManager.Data.Enums;
/// <summary>
/// 启动选项
/// </summary>
public enum StartingOptions
{
    /// <summary>
    /// 开机启动
    /// </summary>
    [Description("enum_bootstart")]
    OpenWhenBoot = 0,
    /// <summary>
    /// 开机不启动
    /// </summary>
    [Description("enum_bootnotstart")]
    NotOpenWhenBoot = 1,
}
