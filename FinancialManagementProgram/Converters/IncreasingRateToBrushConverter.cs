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
    class IncreasingRateToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                return null;

            int spending = (int)values[0];
            int lastWeekSpending = (int)values[1];
            string colorResource = "Primary";

            if (lastWeekSpending != 0 && spending > lastWeekSpending)
                colorResource = "ErrorColor";

            return App.Current.FindResource(colorResource);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
