using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcessManager.Services_Interfaces;

public interface IDialogService
{
    R OpenDialog<T,R>()where T:UserControl ;
}
