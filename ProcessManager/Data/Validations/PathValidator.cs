using System.Globalization;
using System.Windows.Controls;

namespace ProcessManager.Data.Validations;

public class PathValidator : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return new ValidationResult(false, "启动路径不为空。");
        return ValidationResult.ValidResult;
    }
}
