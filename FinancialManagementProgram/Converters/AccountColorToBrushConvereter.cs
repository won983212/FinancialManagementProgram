using FinancialManagementProgram.Data;
using System;
using System.Globalization;
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
            return Binding.DoNothing;
        }
    }
}
