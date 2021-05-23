using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram
{
    public static class CommonUtil
    {
        private static readonly int[] MonthDays = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };


        #region Date Utils

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

        #endregion


        #region List Utils

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
    }
}
