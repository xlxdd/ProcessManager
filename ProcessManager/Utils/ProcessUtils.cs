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
        public (float cpu, float ram) Watch()
        {
            return (cpuCounter.NextValue(), ramCounter.NextValue() / (1024 * 1024));
        }
        public void Dispose()
        {
            cpuCounter.Dispose();
            ramCounter.Dispose();
        }
    }
    /// <summary>
    /// 单进程进程
    /// 性能监测器
    /// </summary>
    public class GeneralProcessWatcher
    {
        private ProcessWatcher watcher;
        public GeneralProcessWatcher(string name)
        {
            watcher = new ProcessWatcher(name);
        }
        public ProcessRealtimeInfo Watch()
        {
            var res = watcher.Watch();
            return new ProcessRealtimeInfo() { CPUUsage = res.cpu, RAMUsage = res.ram, ProcessStatus = ProcessStatus.Running };
        }
        public void Dispose()
        {
            watcher.Dispose();
        }
    }
    /// <summary>
    /// 多进程进程
    /// 性能监测器
    /// </summary>
    public class MultiProcessWatcher
    {
        private List<ProcessWatcher> watchers;

    }
}
