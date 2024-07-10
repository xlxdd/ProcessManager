using ProcessManager.Data;
using ProcessManager.Data.Enums;
using System.Diagnostics;

namespace ProcessManager.Utils;

public static class ProcessUtils
{
    public static ProcessRealtimeInfo GetProcessRealtimeInfo(int processId)
    {
        var process = Process.GetProcessById(processId); // 获取指定 ID 的进程
        ProcessRealtimeInfo info;
        //没有获取到，说明进程寄了
        if (process == null)
        {
            info = new ProcessRealtimeInfo() { CPUUsage = null, RAMUsage = null, ProcessStatus = ProcessStatus.Closed };
            return info;
        }
        //获取占用
        var processName = process.ProcessName;
        using var cpuCounter = new PerformanceCounter("Process", "% Processor Time", processName);
        using var ramCounter = new PerformanceCounter("Process", "Working Set", processName);
        var cpu = cpuCounter.NextValue();
        var ram = ramCounter.NextValue();
        info = new ProcessRealtimeInfo() { CPUUsage = cpu, RAMUsage = ram, ProcessStatus = ProcessStatus.Closed };
        return info;
    }
}
