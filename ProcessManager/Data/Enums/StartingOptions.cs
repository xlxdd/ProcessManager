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
    [Description("开机启动")]
    OpenWhenBoot = 0,
    /// <summary>
    /// 开机不启动
    /// </summary>
    [Description("开机不启动")]
    NotOpenWhenBoot = 1,
}
