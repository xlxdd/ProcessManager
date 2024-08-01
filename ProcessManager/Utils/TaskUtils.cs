namespace ProcessManager.Utils;

public static class TaskUtils
{
    /// <summary>
    /// 在时间限制内执行一个任务，成功执行返回true，失败就取消任务并返回false
    /// </summary>
    /// <param name="act">需要执行的任务</param>
    /// <param name="timeLimit">时间限制 单位:毫秒</param>
    /// <param name="FailureStrategy">失败策略</param>
    /// <returns></returns>
    public static async Task<bool> FinishInTimeLimit(Action act, Action FailureStrategy, int timeLimit)
    {
        using (var cts = new CancellationTokenSource())
        {
            var task = Task.Run(act, cts.Token);

            // 等待任务完成或时间到
            if (await Task.WhenAny(task, Task.Delay(timeLimit)) == task)
            {
                return true; // 成功启动，退出方法
            }
            else
            {
                // 超时
                cts.Cancel();
                FailureStrategy();
                return false;
            }
        }
    }
    /// <summary>
    /// 重试n次
    /// </summary>
    /// <param name="act">需要执行的任务</param>
    /// <param name="timeLimit">每次的时间限制（时间间隔）</param>
    /// <param name="times">需要尝试的次数</param>
    /// <returns></returns>
    public static async Task<bool> RetryManyTimes(Action act, Action FailureStrategy, int timeLimit, int times)
    {
        for (int i = 0; i < times; i++)
        {
            if (await FinishInTimeLimit(act, FailureStrategy, timeLimit)) return true;
        }
        return false;
    }
}
