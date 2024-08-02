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
    [ObservableProperty]
    private ushort priority = 1;
    /// <summary>
    /// 名称
    /// </summary>
    [ObservableProperty]
    private MultiLanguageString? name = new();
    /// <summary>
    /// 启动路径
    /// </summary>
    [ObservableProperty]
    private string? path;
    /// <summary>
    /// 是否启用启动参数
    /// 这个显然用一个下拉框加勾选框更合理
    /// </summary>
    [ObservableProperty]
    private bool? enableParamaters = false;
    /// <summary>
    /// 启动参数 虽然我不明白为什么他的参数能是文件路径 在我理解里启动参数都是-a xxx -b xxx这样的
    /// </summary>
    [ObservableProperty]
    public string? parameters;
    /// <summary>
    /// 延迟时间
    /// 该程序启动后等待多久启动下一个程序，默认为0
    /// 单位ms
    /// </summary>
    [ObservableProperty]
    public ushort? delayTime = 0;
    /// <summary>
    /// 超时时间
    /// 超过该时间程序没有启动，视为超时
    /// 弹出“无法获取句柄”窗口 换个提示吧 又说程序要防止船员误操作，又给这样的提示，船员能看懂什么是句柄吗
    /// </summary>
    [ObservableProperty]
    private ushort? overtimeTime = 100;
    /// <summary>
    /// 是否启用最大CPU占用重启
    /// </summary>
    [ObservableProperty]
    private bool? enableMaxCPUUsage = false;
    /// <summary>
    /// 最大CPU占用
    /// 单位 %
    /// </summary>
    [ObservableProperty]
    private double? maxCPUUsage;
    /// <summary>
    /// 是否启用最大内存占用重启
    /// </summary>
    [ObservableProperty]
    private bool? enableMaxRAMUsage = false;
    /// <summary>
    /// 最大内存占用
    /// 单位MB
    /// </summary>
    [ObservableProperty]
    private UInt32? maxRAMUsage;
    /// <summary>
    /// 窗口数
    /// </summary>
    [ObservableProperty]
    private ushort windowCount = 1;
    /// <summary>
    /// 进程数 默认为1
    /// </summary>
    [ObservableProperty]
    private ushort processCount = 1;
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
