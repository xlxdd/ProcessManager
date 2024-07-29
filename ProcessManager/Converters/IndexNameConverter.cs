using ProcessManager.Resources;
using System.Globalization;
using System.Windows.Data;

namespace ProcessManager.Converters;
/// <summary>
/// 由于程序中为了拓展性，将菜单列表在ViewModel中用List方式实现
/// UI中的button需要绑定vm中List中的元素，因而不能绑定资源字典，无法实现多语言字符串
/// 因此写一个转换器，将name视为index，在LanguageManager单例中寻找当前语言字符串
/// 但是这样通知更改就实现不了
/// </summary>
public class IndexNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return LanguageManager.Instance[value.ToString()!];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}
