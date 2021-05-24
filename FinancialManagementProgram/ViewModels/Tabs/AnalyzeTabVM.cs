using FinancialManagementProgram.Converters;
using FinancialManagementProgram.kftcAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class AnalyzeTabVM : TabChild
    {
        public AnalyzeTabVM(TabContainer parent)
            : base(parent)
        { }

        public void OnDataHandlerPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(APIDataManager.Budget))
            {
                OnPropertyChanged(nameof(RemainingBudget));
                OnPropertyChanged(nameof(RecommendedSpendingInDay));
            }
            else if (e.PropertyName == nameof(APIDataManager.TotalSpending))
            {
                OnPropertyChanged(nameof(RemainingBudget));
            }
            else if (e.PropertyName == nameof(APIDataManager.TargetDate))
            {
                OnPropertyChanged(nameof(RecommendedSpendingInDay));
                OnPropertyChanged(nameof(PredictSpendingThisMonth));
                OnPropertyChanged(nameof(SpendingLastMonth));
                OnPropertyChanged(nameof(PredictSpendingThisWeek));
                OnPropertyChanged(nameof(SpendingLastWeek));
            }
        }

        private static int TotalSpendingBetween(DateTime from, DateTime to)
        {
            return APIDataManager.Current.GetTransactionsBetween(from, to).Sum((e) => e.Transactions.TotalSpending);
        }


        public int RemainingBudget
        {
            get => APIDataManager.Current.Budget - APIDataManager.Current.TotalSpending;
        }

        public int RecommendedSpendingInDay
        {
            get
            {
                DateTime now = DateTime.Now;
                if (TargetDate.Year != now.Year || TargetDate.Month != now.Month)
                    return 0;
                return RemainingBudget / (CommonUtil.GetTotalDays(TargetDate) - now.Day + 1);
            }
        }

        public int PredictSpendingThisMonth
        {
            get
            {
                DateTime now = DateTime.Now;
                int totalDays = CommonUtil.GetTotalDays(TargetDate);
                if (TargetDate.Year != now.Year || TargetDate.Month != now.Month)
                    totalDays = now.Day;
                return APIDataManager.Current.TotalSpending * totalDays / now.Day;
            }
        }

        public int SpendingLastMonth
        {
            get => TotalSpendingBetween(TargetDate.AddMonths(-1), TargetDate);
        }

        public int PredictSpendingThisWeek
        {
            get
            {
                DateTime now = DateTime.Now;
                if (TargetDate.Year != now.Year || TargetDate.Month != now.Month)
                    return 0;

                int offset = ((int)now.DayOfWeek + 1) % 7;
                int spendingThisWeek = TotalSpendingBetween(now.AddDays(-offset), now.AddDays(1));
                return spendingThisWeek * 7 / (offset + 1);
            }
        }

        public int SpendingLastWeek
        {
            get
            {
                DateTime now = DateTime.Now;
                int offset = ((int)now.DayOfWeek + 1) % 7 + 1;
                return TotalSpendingBetween(now.AddDays(-offset - 7), now.AddDays(-offset));
            }
        }

        private DateTime TargetDate
        {
            get => APIDataManager.Current.TargetDate;
        }
    }
}
