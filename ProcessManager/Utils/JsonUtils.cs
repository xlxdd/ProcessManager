using System.IO;
using System.Text.Json;

namespace ProcessManager.Utils;
/// <summary>
/// json工具类
/// </summary>
public static class JsonUtils
{
    /// <summary>
    /// 从文件读取字符串并反序列化为对象
    /// </summary>
    /// <typeparam name="T">目标对象类型</typeparam>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    public static T ReadfromJson<T>(string filePath)
    {
        try
        {
            // 打开并读取文件内容
            using (StreamReader fileReader = new StreamReader(filePath))
            {
                // 从文件中获取JSON字符串
                string jsonContent = fileReader.ReadToEnd();

                // 使用JsonConvert.DeserializeObject反序列化JSON字符串为User对象
                T opts = JsonSerializer.Deserialize<T>(jsonContent)!;
                return opts;
            }
        }
        catch (FileNotFoundException)
        {
            throw;
        }
    }
    /// <summary>
    /// 将对象序列化并写入文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputPath">目标文件路径</param>
    /// <param name="content">对象</param>
    public static void WriteToJson<T>(string outputPath, T content)
    {
        try
        {
            // 将User对象序列化为JSON字符串
            string jsonOutput = JsonSerializer.Serialize<T>(content);

            // 将JSON字符串写入文件
            using (StreamWriter fileWriter = new StreamWriter(outputPath))
            {
                fileWriter.Write(jsonOutput);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
