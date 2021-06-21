using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FinancialManagementProgram.Converters
{
    [ValueConversion(typeof(double), typeof(Visibility))]
    class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double.TryParse((value ?? "").ToString(), out double val);
            if (parameter == null || parameter is string == false || ((string)parameter).Length == 0)
                return val == 0 ? Visibility.Visible : Visibility.Collapsed;
            return val == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
