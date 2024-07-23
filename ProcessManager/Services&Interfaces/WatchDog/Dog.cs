using System.Windows.Threading;

namespace ProcessManager.Services_Interfaces.WatchDog;
/// <summary>
/// 来源
/// https://www.cnblogs.com/chonglu/p/16913746.html
/// </summary>
public class Dog
{
    private readonly DispatcherTimer timer;
    public Dog(Action<Object?, EventArgs> action, int interval = 1000)
    {
        timer = new();
        timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
        timer.Tick += new EventHandler(action);
        timer.IsEnabled = true;
    }
    public void AddAction(Action<Object?, EventArgs> action)
    {
        timer.Tick += new EventHandler(action);
    }
    public void Start()
    {
        timer.Start();
    }
    public void Stop()
    {
        timer.Stop();
    }
}

