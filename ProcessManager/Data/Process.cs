using CommunityToolkit.Mvvm.ComponentModel;

namespace ProcessManager.Data;

/// <summary>
/// 进程信息类
/// 可以将所有进程信息展示给使用者，该类不需要设计DTO，
/// </summary>
public partial class Process : ObservableObject
{
    /// <summary>
    /// 编号，在我的理解里是优先级？
    /// </summary>
    public ushort? Priority { get; private set; }
    /// <summary>
    /// 名称
    /// </summary>
    public MultiLanguageString? Name { get; set; }
    /// <summary>
    /// 启动路径
    /// </summary>
    public string? Path { get; set; }
    /// <summary>
    /// 启动参数 虽然我不明白为什么他的参数能是文件路口 在我理解里启动参数都是-a -b这样的
    /// </summary>
    public string? Parameters { get; set; }
}
