using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ProcessManager.Data;
/// <summary>
/// 功能按键类
/// 目前只定义按钮显示名称
/// 可以拓展属性以定义文字大小、颜色、字体，按钮长度、宽度、颜色等信息
/// </summary>
public partial class FunctionButton : ObservableObject
{
    [ObservableProperty]
    private string? name;
    public IRelayCommand? Command { get; set; }
    public string? ViewName { get; set; }
    public void ChangeCulture()
    {
        OnPropertyChanged(nameof(Name));
    }
}
