using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.Data.Enums;

namespace ProcessManager.Data;
/// <summary>
/// 进程启动信息，将会持久化保存在配置文件中（我的理解）
/// </summary>
public partial class ProcessStartingOptions : ObservableObject
{
    /// <summary>
    /// 编号，在我的理解里是优先级？
    /// </summary>
    public ushort Priority { get; private set; }
    /// <summary>
    /// 名称
    /// </summary>
    public MultiLanguageString? Name { get; set; }
    /// <summary>
    /// 启动路径
    /// </summary>
    public string? Path { get; set; }
    /// <summary>
    /// 是否启用启动参数
    /// 这个显然用一个下拉框加勾选框更合理
    /// </summary>
    public bool? EnableParamaters { get; set; }
    /// <summary>
    /// 启动参数 虽然我不明白为什么他的参数能是文件路径 在我理解里启动参数都是-a xxx -b xxx这样的
    /// </summary>
    public string? Parameters { get; set; }
    /// <summary>
    /// 延迟时间
    /// 该程序启动后等待多久启动下一个程序，默认为0
    /// 单位ms
    /// </summary>
    public ushort? DelayTime { get; set; }
    /// <summary>
    /// 超时时间
    /// 超过该时间程序没有启动，视为超时
    /// 弹出“无法获取句柄”窗口 我换个提示吧，这个太爆了
    /// </summary>
    public ushort? OvertimeTime { get; set; }
    /// <summary>
    /// 是否启用最大CPU占用重启
    /// </summary>
    public bool? EnableMaxCPUUsage { get; set; }
    /// <summary>
    /// 最大CPU占用
    /// 单位 %
    /// </summary>
    public double? MaxCPUUsage { get; set; }
    /// <summary>
    /// 是否启用最大内存占用重启
    /// </summary>
    public bool? EnableMaxRAMUsage { get; set; }
    /// <summary>
    /// 最大内存占用
    /// 单位MB
    /// </summary>
    public UInt32? MaxRAMUsage { get; set; }
    /// <summary>
    /// 窗口数 什么是窗口数？令人感到费解
    /// </summary>
    public ushort WindowCount { get; set; }
    /// <summary>
    /// 进程数 什么是进程数？同样令人感到费解
    /// </summary>
    public ushort ProcessCount { get; set; }
    /// <summary>
    /// 启动设置
    /// </summary>
    public StartingOptions StartingOption { get; set; }
    /// <summary>
    /// 显示设置
    /// </summary>
    public ShowingOptions ShowingOption { get; set; }
    /// <summary>
    /// 关闭设置
    /// </summary>
    public ClosingOptions ClosingOption { get; set; }
    //TODO:可能不全
}
