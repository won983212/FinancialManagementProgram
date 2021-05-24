using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FinancialManagementProgram.Converters
{
    class IncreasingRateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                return null;

            int spending = (int) values[0];
            int lastWeekSpending = (int)values[1];
            if (lastWeekSpending == 0)
                return SimplifyBudgetUnitConverter.SimplifyBudgetUnit(spending, true);

            double value = spending * 100.0 / lastWeekSpending - 100;
            if (value == 0)
                return "동일";

            return string.Format("{0:+#\\%;-#\\%;비슷}", (int)value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
