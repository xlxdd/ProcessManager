using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        using var cpuCounter = new PerformanceCounter("Process", "% Processor Time", processName);
        using var ramCounter = new PerformanceCounter("Process", "Working Set", processName);
        var cpu = cpuCounter.NextValue();
        var ram = ramCounter.NextValue()/1048576;
        Console.WriteLine($"CPU:{cpu}");
        Console.WriteLine($"RAM:{ram}");
        return;
    }
}
