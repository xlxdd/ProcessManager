using ProcessManager.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProcessManager.Utils;

public static class JsonUtils
{
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
    public static void WriteToJson(T)
    {
        try
        {
            string outputPath = @"opt.json";
            // 将User对象序列化为JSON字符串
            string jsonOutput = JsonSerializer.Serialize(newcfg);

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
