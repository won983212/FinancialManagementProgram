using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FinancialManagementProgram.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    class DatetimeDifferenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is DateTime == false)
                return null;

            string postfix = "전";
            TimeSpan time = DateTime.Now - ((DateTime)value);

            if (time.Ticks < 0)
            {
                time = time.Negate();
                postfix = "후";
            }

            if (time.Days > 30)
                return (time.Days / 30) + "달" + postfix;
            if (time.Days > 0)
                return time.Days + "일" + postfix;
            if (time.Hours > 0)
                return time.Hours + "시간" + postfix;
            if (time.Minutes > 0)
                return time.Minutes + "분" + postfix;
            if (time.Seconds > 5)
                return time.Seconds + "초" + postfix;
            return "방금전";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
