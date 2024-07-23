using System.Globalization;
using System.Windows.Controls;

namespace ProcessManager.Data.Validations;

public class WindowCountValidator : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return new ValidationResult(false, "窗口数不为空。");
        if (!ushort.TryParse(value.ToString(), out var res) || res == 0) return new ValidationResult(false, "窗口数需要为大于0的整数。");
        return ValidationResult.ValidResult;
    }
}
