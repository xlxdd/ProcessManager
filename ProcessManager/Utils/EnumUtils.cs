namespace ProcessManager.Utils;
/// <summary>
/// 枚举类型工具类合集
/// </summary>
public static class EnumUtils
{
    /// <summary>
    /// 获取枚举类型的每一种枚举，转化为List返回
    /// </summary>
    /// <typeparam name="T">类型参数，要转化的枚举类型</typeparam>
    /// <returns></returns>
    public static List<T> EnumToList<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
}
