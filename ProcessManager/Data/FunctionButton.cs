using CommunityToolkit.Mvvm.ComponentModel;

namespace ProcessManager.Data;
/// <summary>
/// 功能按键类
/// 目前只定义按钮显示名称
/// 可以拓展属性以定义文字大小、颜色、字体，按钮长度、宽度、颜色等信息
/// </summary>
public partial class FunctionButton : ObservableObject
{
    public string? Name { get; set; }
}
