using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialManagementProgram.Data
{
    public class TransactionDataAnalyzer : ObservableObject
    {
        private readonly DataManager _dataManager;
        private readonly Dictionary<int, TransactionGroup> _dayTransactions = new Dictionary<int, TransactionGroup>();
        private Dictionary<TransactionCategory, TransactionGroup> _categorizedTransactions = new Dictionary<TransactionCategory, TransactionGroup>();
        private TransactionGroup _monthTransactions = new TransactionGroup();

        public TransactionDataAnalyzer(DataManager datamanager)
        {
            _dataManager = datamanager;
        }

        private long TotalSpendingBetween(DateTime from, DateTime to)
        {
            return _dataManager.GetTransactionsBetween(from, to).Sum((e) => e.Transactions.TotalSpending);
        }

        private void PrepareMonthTransaction()
        {
            foreach (BankAccount account in _dataManager.BankAccounts)
                account.MonthlyTransactions.ClearTransactions();

            _monthTransactions = new TransactionGroup();
            _dayTransactions.Clear();
            foreach (var ent in _dataManager.GetTransactionsBetween(TargetDate, TargetDate.AddMonths(1)))
            {
                _monthTransactions.AddTransactionGroup(ent.Transactions);
                _dayTransactions.Add(ent.Date, ent.Transactions);
                foreach (Transaction t in ent.Transactions.Transactions)
                {
                    if (t.Account != null)
                        t.Account.MonthlyTransactions.AddTransaction(t);
                    else
                        Logger.Warn("Account가 NULL입니다: " + t.Label);
                }
            }

            OnPropertyChanged(nameof(MonthlyTransactions));
            OnPropertyChanged(nameof(MonthlyTotalSpending));
            OnPropertyChanged(nameof(MonthlyTotalIncoming));
        }

        public TransactionGroup GetDayTransaction(int date)
        {
            if (_dayTransactions.TryGetValue(date, out TransactionGroup result))
                return result;
            return null;
        }

        private void CategorizeTransaction()
        {
            _categorizedTransactions = new Dictionary<TransactionCategory, TransactionGroup>();
            foreach (Transaction ent in _monthTransactions.Transactions)
            {
                if (ent.Amount < 0)
                {
                    if (!_categorizedTransactions.TryGetValue(ent.Category, out TransactionGroup categoryTrans))
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
            get => _categorizedTransactions;
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
            get => _dataManager.Budget - MonthlyTotalSpending;
        }

        public int BudgetUsagePercent
        {
            get => (int)(MonthlyTotalSpending * 100.0 / _dataManager.Budget);
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
            get => _dataManager.TargetDate;
        }
    }
}
