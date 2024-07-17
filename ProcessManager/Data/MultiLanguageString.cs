using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProcessManager.Data;
/// <summary>
/// 多语言字符串
/// 用于应对进程需要中文名和英文名的需求
/// 若有需要可以拓展出更多的语言类型
/// </summary>
public partial class MultiLanguageString : ObservableValidator
{
    /// <summary>
    /// 中文
    /// </summary>
    [ObservableProperty]
    [Required]
    private string? chinese;
    /// <summary>
    /// 英文
    /// </summary>
    [ObservableProperty]
    [Required]
    private string? english;
}
