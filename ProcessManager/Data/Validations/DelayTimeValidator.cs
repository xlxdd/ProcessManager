﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcessManager.Data.Validations;
public class DelayTimeValidator : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return new ValidationResult(false, "延迟时间不为空。");
        if (!ushort.TryParse(value.ToString(), out var res) || res == 0) return new ValidationResult(false, "延迟时间为大于0的整数。");
        return ValidationResult.ValidResult;
    }
}