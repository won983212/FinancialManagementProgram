using FinancialManagementProgram.Dialog.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialManagementProgram
{
    public static class CommonUtil
    {
        public delegate void DialogCompleteEventHandler<T>(T vm, DialogClosingEventArgs eventArgs) where T : DialogViewModel;

        private static readonly int[] MonthDays = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private static readonly Random random = new Random();


        public static long GenerateUniqueID(Predicate<long> confilctCondition)
        {
            long x;
            do
            {
                x = random.Next() << sizeof(int) * 8;
                x += random.Next();
            }
            while (confilctCondition(x));
            return x;
        }

        #region Date

        public static int GetTotalDays(int year, int month)
        {
            int days = MonthDays[month - 1];

            // handle february 29 days
            if (IsLeapYear(year) && month == 2)
                days++;

            return days;
        }

        public static int GetTotalDays(DateTime date)
        {
            return GetTotalDays(date.Year, date.Month);
        }

        public static bool IsLeapYear(int year)
        {
            return year % 400 == 0 || year % 4 == 0 && year % 100 > 0;
        }

        public static int GetIntegerDate(DateTime date)
        {
            return date.Year * 10000 + date.Month * 100 + date.Day;
        }

        public static DateTime ParseDatetimeFromIntDate(int date)
        {
            return new DateTime(date / 10000, (date / 100) % 100, date % 100);
        }

        #endregion


        #region List

        public static int FindSortedInsertionIndex<T>(IList<T> list, T value, IComparer<T> comparer = null)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            comparer = comparer ?? Comparer<T>.Default;

            int left = 0;
            int right = list.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int comp = comparer.Compare(list[mid], value);
                if (comp < 0)
                    left = mid + 1;
                else if (comp > 0)
                    right = mid - 1;
                else
                    return mid;
            }

            return left;
        }

        #endregion


        #region Dialog

        public static Task<object> ShowDialog<T>(T content, DialogCompleteEventHandler<T> closingHandler) where T : DialogViewModel
        {
            return ShowDialog(content, "RootDialogHost", closingHandler);
        }

        public static Task<object> ShowDialog<T>(T content, string dialog, DialogCompleteEventHandler<T> closingHandler) where T : DialogViewModel
        {
            return DialogHost.Show(content, dialog, (o, e) => closingHandler?.Invoke((T)((DialogHost)o).DialogContent, e));
        }

        #endregion
    }
}
