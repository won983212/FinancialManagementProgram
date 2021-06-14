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
    [ValueConversion(typeof(long), typeof(Brush))]
    class BalanceToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is long == false)
                return null;

            long amount = (long)value;
            if (amount < 0)
                return App.Current.FindResource("ErrorColor");
            else
                return App.Current.FindResource("Primary");
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
