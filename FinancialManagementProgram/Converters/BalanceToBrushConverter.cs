using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FinancialManagementProgram.Converters
{
    [ValueConversion(typeof(int), typeof(Brush))]
    class BalanceToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is int == false)
                return null;

            int amount = (int)value;
            if (amount < 0)
                return App.Current.FindResource("ErrorColor");
            else
                return App.Current.FindResource("Primary");
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
