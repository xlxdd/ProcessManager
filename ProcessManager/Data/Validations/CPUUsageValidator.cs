using System.Globalization;
using System.Windows.Controls;

namespace ProcessManager.Data.Validations;

public class CPUUsageValidator : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return new ValidationResult(false, "CPU占用不为空。");
        if (!double.TryParse(value.ToString(), out var res) || res < 0 || res > 100) return new ValidationResult(false, "CPU占用需要在1-100之间。");
        return ValidationResult.ValidResult;
    }
}
