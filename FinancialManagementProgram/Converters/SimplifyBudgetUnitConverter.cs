using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FinancialManagementProgram.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    class SimplifyBudgetUnitConverter : IValueConverter
    {
        private static readonly string[] Postfixes = { "", "만", "억", "조", "경", "해" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SimplifyBudgetUnit((int)value, false);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public static string SimplifyBudgetUnit(int amount, bool appendPlusSign)
        {
            int significant = amount;
            int postfixIndex = 0;

            for (int i = 0; i < Postfixes.Length && significant >= 10000; i++)
            {
                significant /= 10000;
                postfixIndex++;
            }

            string formatText = appendPlusSign ? "{0:+#,##;-#,##;0}{1}원" : "{0:#,##;-#,##;0}{1}원";
            return string.Format(formatText, significant, Postfixes[postfixIndex]);
        }
    }
}
