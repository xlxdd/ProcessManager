using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Services_Interfaces;

public interface IDialogWindow
{
    public bool? DialogResult { get; set; }
    public Object DataContext { get; set; }
    public bool? ShowDialog();
}
