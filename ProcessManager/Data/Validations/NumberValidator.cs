using System.Globalization;
using System.Windows.Controls;

namespace ProcessManager.Data.Validations;

public class NumberValidator : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return new ValidationResult(false, "e_numbernotnull");
        if (!ushort.TryParse(value.ToString(), out var res) || res == 0) return new ValidationResult(false, "e_numberrange");
        return ValidationResult.ValidResult;
    }
}
