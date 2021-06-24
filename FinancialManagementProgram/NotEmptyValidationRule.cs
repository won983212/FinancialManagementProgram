using System.Globalization;
using System.Windows.Controls;

namespace FinancialManagementProgram
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "이 필드는 반드시 입력해야합니다.")
                : ValidationResult.ValidResult;
        }
    }
}
