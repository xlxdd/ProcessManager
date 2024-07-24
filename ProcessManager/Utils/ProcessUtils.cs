using ProcessManager.Data;
using ProcessManager.Data.Enums;
using System.Diagnostics;

namespace ProcessManager.Utils;

public static class ProcessUtils
{
    /// <summary>
    /// 进程监测类
    /// 一个进程监测类实例只能对应一个进程实例（instance）
    /// </summary>
    private class ProcessWatcher
    {
        private string instanceName;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        public ProcessWatcher(string name)
        {
            instanceName = name;
            cpuCounter = new PerformanceCounter("Process", "% Processor Time", instanceName);
            ramCounter = new PerformanceCounter("Process", "Working Set - Private", instanceName);
        }
        public (float cpu, float ram,bool running) Watch()
        {
            try
            {
                return (cpuCounter.NextValue(), ramCounter.NextValue() / (1024 * 1024),true);
            }
            catch
            {
                return (0f, 0f, false);
            }
        }
        public void Dispose()
        {
            cpuCounter.Dispose();
            ramCounter.Dispose();
        }
    }
    /// <summary>
    /// 性能监测器
    /// </summary>
    public class GeneralProcessWatcher
    {
        private List<ProcessWatcher> watcher = new();
        public GeneralProcessWatcher(string name, int count)
        {
            for (int i = 0; i < count; i++)
            {
                ProcessWatcher wch;
                if (i == 0)
                {
                    wch = new ProcessWatcher(name);
                }
                else
                {
                    wch = new ProcessWatcher($"{name}#{i}");
                }
                watcher.Add(wch);
            }
        }
        public ProcessRealtimeInfo Watch()
        {
            var info = new ProcessRealtimeInfo() { CPUUsage = 0, RAMUsage = 0, ProcessStatus = ProcessStatus.Running };
            foreach (ProcessWatcher watcher in watcher)
            {
                var res = watcher.Watch();
                info.CPUUsage += res.cpu;
                info.RAMUsage += res.ram;
                if(res.running == false)info.ProcessStatus = ProcessStatus.Closed;
            }
            return info;
        }
        public void Dispose()
        {
            foreach (ProcessWatcher watcher in watcher)
            {
                watcher.Dispose();
            }
        }
    }
}
