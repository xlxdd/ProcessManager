using Microsoft.Xaml.Behaviors;
using ProcessManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ProcessManager.Data.Validations;
/// <summary>
/// 使用触发器在页面校验发现错误时，将错误传到后台
/// </summary>
public class ValidationExceptionBehavior : Behavior<FrameworkElement>
{

    protected override void OnAttached()
    {
        //添加 Validation.Error 事件监听
        this.AssociatedObject.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(OnValidationError));
    }

    private void OnValidationError(Object sender, ValidationErrorEventArgs e)
    {
        ViewModelBase mainModel = null;
        if (AssociatedObject.DataContext is ViewModelBase)
        {
            mainModel = this.AssociatedObject.DataContext as ViewModelBase;
        }
        if (mainModel == null) return;

        //OriginalSource 触发事件的元素
        var element = e.OriginalSource as UIElement;
        if (element == null) return;

        //ValidationErrorEventAction.Added  表示新产生的行为
        if (e.Action == ValidationErrorEventAction.Added)
        {
            mainModel.ErrorList.Add(e.Error.ErrorContent.ToString());
        }
        else if (e.Action == ValidationErrorEventAction.Removed) //ValidationErrorEventAction.Removed  该行为被移除，即代表验证通过
        {
            mainModel.ErrorList.Remove(e.Error.ErrorContent.ToString());
        }
    }

    protected override void OnDetaching()
    {
        //移除 Validation.Error 事件监听
        this.AssociatedObject.RemoveHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(OnValidationError));
    }
}
