using ProcessManager.ViewModels.Dialogs;
using UserControl = System.Windows.Controls.UserControl;

namespace ProcessManager.Services_Interfaces;
/// <summary>
/// 这个对话框服务写的很失败
/// 复杂度很高
/// 想避免messenger的复杂度结果写的比messenger更复杂，拓展性还很差，稍微有点变动就要改接口
/// prism用messenger确实是经过深思熟虑的
/// </summary>
public interface IDialogService
{
    Result OpenDialog<View, ViewModel, Result>(string title, object param) where View : UserControl, new() where ViewModel : DialogBase, IDialogResult<Result>, new() where Result : class?;
}
