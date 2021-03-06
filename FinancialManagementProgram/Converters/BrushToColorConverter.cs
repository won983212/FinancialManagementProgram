using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FinancialManagementProgram.Converters
{
    [ValueConversion(typeof(SolidColorBrush), typeof(Color))]
    class BrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is SolidColorBrush)
                return ((SolidColorBrush)value).Color;

            throw new InvalidOperationException("Unsupported brush type: " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
