namespace ProcessManager.Utils;

public static class StringUtils
{
    /// <summary>
    /// 全文件名到进程名称
    /// </summary>
    /// <param name="FullName"></param>
    /// <returns></returns>
    public static string FullNameToProcessName(string fullName)
    {
        string fileName = fullName.Substring(fullName.LastIndexOf("\\") + 1); // 文件名
        string processName = fileName.Substring(0, fileName.LastIndexOf("."));
        return processName;
    }
}
