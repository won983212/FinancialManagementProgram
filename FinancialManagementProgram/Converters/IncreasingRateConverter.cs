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

            long spending = (long) values[0];
            long lastSpending = (long)values[1];
            if (lastSpending == 0)
                return SimplifyBudgetUnitConverter.SimplifyBudgetUnit(spending, true);

            double value = spending * 100.0 / lastSpending - 100;
            int percent = (int) value;
            if (percent == 0 && percent != value)
                percent = (int) (value / Math.Abs(value));

            return string.Format("{0:+#\\%;-#\\%;동일}", percent);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
