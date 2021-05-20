using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FinancialManagementProgram.Converters
{
    class BalanceChangesToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is int == false)
                return null;

            int amount = (int) value;
            if (amount < 0)
                return App.Current.FindResource("ErrorColor");
            else
                return App.Current.FindResource("Primary");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
