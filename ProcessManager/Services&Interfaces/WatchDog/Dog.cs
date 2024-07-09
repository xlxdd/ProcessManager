using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ProcessManager.Services_Interfaces.WatchDog;
/// <summary>
/// 来源
/// https://www.cnblogs.com/chonglu/p/16913746.html
/// </summary>
public class Dog
{
    private readonly DispatcherTimer timer = new ();
    public Dog(int interval = 1000)
    {
        
    }
}

