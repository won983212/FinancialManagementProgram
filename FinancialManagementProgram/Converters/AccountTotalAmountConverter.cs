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
    [ValueConversion(typeof(IList<BankAccount>), typeof(long))]
    class AccountTotalAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is IList<BankAccount> == false)
                return null;
            return ((IList<BankAccount>)value).Sum((e) => e.BalanceAmount);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
