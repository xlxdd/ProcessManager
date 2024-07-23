using System.Globalization;
using System.Windows.Controls;

namespace ProcessManager.Data.Validations;

public class NameValidator : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return new ValidationResult(false, "名称不为空。");
        return ValidationResult.ValidResult;
    }
}
