using FinancialManagementProgram.kftcAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FinancialManagementProgram.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    class DayTotalSpendingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is string == false)
                return null;

            DayTransaction transaction = APIContext.Current.GetDayTransaction((string)value);
            if (transaction == null)
                return "0원";

            return string.Format("{0:+#,##원;-#,##원;0원}", transaction.TotalIncoming - transaction.TotalSpending);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
