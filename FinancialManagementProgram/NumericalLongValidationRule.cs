using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FinancialManagementProgram
{
    class NumericalLongValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? "").ToString();

            if (string.IsNullOrWhiteSpace(input))
                return new ValidationResult(false, "이 필드는 반드시 입력해야합니다.");

            if (!long.TryParse(input, out long _))
                return new ValidationResult(false, "정수로 기입하세요");

            return ValidationResult.ValidResult;
        }
    }
}
