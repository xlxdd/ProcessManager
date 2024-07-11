using System.Diagnostics;

namespace Tests;

public static class processUtilTest
{
    public static void GetInfo(int processId)
    {
        var process = Process.GetProcessById(processId); // 获取指定 ID 的进程
        //没有获取到，说明进程寄了
        if (process == null)
        {
            Console.WriteLine("process not running");
            return;
        }
        //获取占用
        var processName = process.ProcessName;
        Console.WriteLine($"Name:{processName}");
        //var processTime = process.
        var stw = new Stopwatch();

        PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
        string[] instances = cat.GetInstanceNames();
        foreach (var instance in instances)
        {
            if (instance.Contains("chrome"))
                Console.WriteLine(instance);
        }

        stw.Reset();
        stw.Start();
        using var cpuCounter = new PerformanceCounter("Process", "% Processor Time", processName);
        stw.Stop();
        Console.WriteLine(stw.ElapsedMilliseconds.ToString());
        stw.Reset();
        stw.Start();
        using var ramCounter = new PerformanceCounter("Process", "Working Set - Private", processName);
        stw.Stop();
        Console.WriteLine(stw.ElapsedMilliseconds.ToString());
        stw.Reset();
        stw.Start();
        var cpu = cpuCounter.NextValue();
        var ram = ramCounter.NextValue() / (1024 * 1024);
        stw.Stop();
        Console.WriteLine(stw.ElapsedMilliseconds.ToString());
        Console.WriteLine($"CPU:{cpu}");
        Console.WriteLine($"RAM:{ram}");
        return;
    }
}
