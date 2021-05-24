using FinancialManagementProgram.kftcAPI;
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
    [ValueConversion(typeof(AccountColor), typeof(Brush))]
    class AccountColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Brush)App.Current.FindResource("Gradient" + ((AccountColor)value).ToString() + "Color");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
