using CommunityToolkit.Mvvm.Input;
using ProcessManager.Services_Interfaces;
using System.Windows.Controls;

namespace ProcessManager.ViewModels.Dialogs;
public partial class DialogWindowViewModel<View, ViewModel, Result> where View : UserControl, new() where ViewModel : IDialogResult<Result>, new() where Result : class
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 用户控件
    /// </summary>
    public View UserControl { get; set; }
    /// <summary>
    /// 数据上下文
    /// </summary>
    public ViewModel DataContext { get; set; }
    /// <summary>
    /// 返回值
    /// </summary>
    public Result? DialogResult { get; set; }
    public DialogWindowViewModel(string title)
    {
        Title = title;
        UserControl = new View();
        DataContext = new ViewModel();
        UserControl.DataContext = DataContext;
    }
    [RelayCommand]
    public void Confirm(IDialogWindow dialog)
    {
        DialogResult = DataContext.GetResult();
        if (dialog != null)
        {
            dialog.DialogResult = true;
        }
    }
    [RelayCommand]
    public void Cancel(IDialogWindow dialog)
    {
        DialogResult = DataContext.GetResult();
        if (dialog != null)
        {
            dialog.DialogResult = true;
        }
    }
}
