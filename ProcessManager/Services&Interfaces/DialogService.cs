using ProcessManager.ViewModels.Dialogs;
using ProcessManager.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcessManager.Services_Interfaces;

public class DialogService : IDialogService
{
    public R OpenDialog<T,R>(T control)where T:UserControl
    {
        IDialogWindow window = new DialogWindow();
        window.DataContext = new DialogWindowViewModel<T>();
        window.ShowDialog();
        return 
    }
}
