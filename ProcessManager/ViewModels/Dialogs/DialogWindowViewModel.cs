using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.Services_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcessManager.ViewModels.Dialogs;
/// <summary>
/// Dialog的容器是window
/// 该类是window的数据上下文类
/// </summary>
/// <typeparam name="T">控件类型</typeparam>
/// <typeparam name="R">返回值类型</typeparam>
public class DialogWindowViewModel<T,R>where T:UserControl
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 控件
    /// </summary>
    public T UserControl { get; set; }
    /// <summary>
    /// 返回值
    /// </summary>
    public R DialogResult { get; set; }
    public DialogWindowViewModel(string title,T userControl)
    {
        Title = title;
        UserControl = userControl;
    }
    public void CloseDialogWithResult(IDialogWindow dialog, R result)
    {
        DialogResult = result;
        if (dialog != null)
        {
            dialog.DialogResult = true;
        }
    }
}
