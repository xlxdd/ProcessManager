﻿using System.Globalization;
using System.Windows.Data;

namespace ProcessManager.Converters;

public class BoolReverseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue; // 取反
        }
        return value; // 返回原值
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue; // 取反
        }
        return value; // 返回原值
    }
}
