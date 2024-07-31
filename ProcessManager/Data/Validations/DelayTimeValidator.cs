using System.Globalization;
using System.Windows.Controls;

namespace ProcessManager.Data.Validations;
public class DelayTimeValidator : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return new ValidationResult(false, "e_delaynotnull");
        if (!ushort.TryParse(value.ToString(), out var res) || res < 0) return new ValidationResult(false, "e_delayrange");
        return ValidationResult.ValidResult;
    }
}
