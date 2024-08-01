using ProcessManager.Resources;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace ProcessManager.Converters;
/// <summary>
/// 枚举类型渲染转换器
/// 用于在渲染下拉框时能够显示Description注解中的内容
/// 源于网络 https://www.cnblogs.com/seabluescn/p/16579315.html
/// </summary>
public class EnumDescriptionConverter : IValueConverter
{
    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Enum myEnum = (Enum)value;
        string description = GetEnumDescription(myEnum);
        return LanguageManager.Instance[description];
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }

    public static string GetEnumDescription(Enum enumObj)
    {
        FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

        object[] attribArray = fieldInfo.GetCustomAttributes(false);

        if (attribArray.Length == 0)
        {
            return enumObj.ToString();
        }
        else
        {
            DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
            return attrib.Description;
        }
    }
}
