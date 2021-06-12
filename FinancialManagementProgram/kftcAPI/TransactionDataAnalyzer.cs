using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public class TransactionDataAnalyzer : ObservableObject
    {
        private readonly Dictionary<string, TransactionGroup> _dayTransactions = new Dictionary<string, TransactionGroup>();
        private readonly Dictionary<TransactionCategory, TransactionGroup> _categorizedTransactions = new Dictionary<TransactionCategory, TransactionGroup>();
        private TransactionGroup _monthTransactions = new TransactionGroup();

        private static long TotalSpendingBetween(DateTime from, DateTime to)
        {
            return APIDataManager.Current.GetTransactionsBetween(from, to).Sum((e) => e.Transactions.TotalSpending);
        }

        private void PrepareMonthTransaction()
        {
            Dictionary<string, BankAccount> accounts = new Dictionary<string, BankAccount>();
            foreach (BankAccount account in APIDataManager.Current.BankAccounts)
                accounts.Add(account.FintechUseNum, account);

            _monthTransactions = new TransactionGroup();
            foreach (var ent in APIDataManager.Current.GetTransactionsBetween(TargetDate, TargetDate.AddMonths(1)))
            {
                _monthTransactions.AddTransactionGroup(ent.Transactions);
                _dayTransactions.Add(ent.Date, ent.Transactions);
                foreach(Transaction t in ent.Transactions.Transactions)
                {
                    BankAccount acc = null;
                    if (accounts.TryGetValue(t.FintechNum, out acc))
                        acc.Transactions.AddTransaction(t);
                    else
                        Logger.Warn("There is no account: " + t.BankName + "(" + t.FintechNum + ")");
                }
            }

            OnPropertyChanged(nameof(MonthlyTransactions));
            OnPropertyChanged(nameof(MonthlyTotalSpending));
            OnPropertyChanged(nameof(MonthlyTotalIncoming));
        }

        /// <summary>
        /// Date format must be yyyy.mm.dd<br/>
        /// Can get transaction in <b>this month</b> only
        /// </summary>
        public TransactionGroup GetDayTransaction(string date)
        {
            TransactionGroup result;
            if (_dayTransactions.TryGetValue(date, out result))
                return result;
            return null;
        }

        private void CategorizeTransaction()
        {
            _categorizedTransactions.Clear();
            foreach (Transaction ent in _monthTransactions.Transactions)
            {
                if (ent.Amount < 0)
                {
                    TransactionGroup categoryTrans;
                    if (!_categorizedTransactions.TryGetValue(ent.Category, out categoryTrans))
                        _categorizedTransactions.Add(ent.Category, categoryTrans = new TransactionGroup());
                    categoryTrans.AddTransaction(ent);
                }
            }

            OnPropertyChanged(nameof(CategorizedTransactions));
        }

        public void Update()
        {
            PrepareMonthTransaction();
            CategorizeTransaction();
            NotifyBudgetChanges();

            OnPropertyChanged(nameof(PredictSpendingThisMonth));
            OnPropertyChanged(nameof(SpendingLastMonth));
            OnPropertyChanged(nameof(PredictSpendingThisWeek));
            OnPropertyChanged(nameof(SpendingLastWeek));
        }

        public void NotifyBudgetChanges()
        {
            OnPropertyChanged(nameof(RemainingBudget));
            OnPropertyChanged(nameof(RecommendedSpendingInDay));
            OnPropertyChanged(nameof(BudgetUsagePercent));
        }

        public IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>> CategorizedTransactions
        {
            get => _categorizedTransactions.AsEnumerable();
        }

        public IEnumerable<Transaction> MonthlyTransactions
        {
            get => _monthTransactions.Transactions;
        }

        public long MonthlyTotalSpending
        {
            get => _monthTransactions.TotalSpending;
        }

        public long MonthlyTotalIncoming
        {
            get => _monthTransactions.TotalIncoming;
        }

        public long RemainingBudget
        {
            get => APIDataManager.Current.Budget - MonthlyTotalSpending;
        }

        public int BudgetUsagePercent
        {
            get => (int)(MonthlyTotalSpending * 100.0 / APIDataManager.Current.Budget);
        }

        public long RecommendedSpendingInDay
        {
            get
            {
                DateTime now = DateTime.Now;
                if (TargetDate.Year != now.Year || TargetDate.Month != now.Month)
                    return 0;
                return RemainingBudget / (CommonUtil.GetTotalDays(TargetDate) - now.Day + 1);
            }
        }


        public long PredictSpendingThisMonth
        {
            get
            {
                DateTime now = DateTime.Now;
                int totalDays = CommonUtil.GetTotalDays(TargetDate);
                if (TargetDate.Year != now.Year || TargetDate.Month != now.Month)
                    totalDays = now.Day;
                return MonthlyTotalSpending * totalDays / now.Day;
            }
        }

        public long SpendingLastMonth
        {
            get => TotalSpendingBetween(TargetDate.AddMonths(-1), TargetDate);
        }

        public long PredictSpendingThisWeek
        {
            get
            {
                DateTime now = DateTime.Now;
                if (TargetDate.Year != now.Year || TargetDate.Month != now.Month)
                    return 0;

                int offset = ((int)now.DayOfWeek + 1) % 7;
                long spendingThisWeek = TotalSpendingBetween(now.AddDays(-offset), now.AddDays(1));
                return spendingThisWeek * 7 / (offset + 1);
            }
        }

        public long SpendingLastWeek
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
