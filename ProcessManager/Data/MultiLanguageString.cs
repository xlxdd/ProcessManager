namespace ProcessManager.Data;
/// <summary>
/// 多语言字符串
/// 用于应对进程需要中文名和英文名的需求
/// 若有需要可以拓展出更多的语言类型
/// </summary>
public partial class MultiLanguageString
{
    /// <summary>
    /// 中文
    /// </summary>
    public string? Chinese { get; set; }
    /// <summary>
    /// 英文
    /// </summary>
    public string? English { get; set; }
}
