using System;
using System.Globalization;
using System.Windows.Controls;

namespace FinancialManagementProgram
{
    public class DatetimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out _))
                return new ValidationResult(false, "잘못된 입력입니다.");
            return ValidationResult.ValidResult;
        }
    }
}
