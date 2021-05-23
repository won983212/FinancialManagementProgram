using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public class DayTransactions
    {
        public string Date { get; }
        public TransactionGroup Transactions { get; }

        internal DayTransactions(SortedList<string, TransactionGroup> _allTransactions, int index)
        {
            Date = _allTransactions.Keys[index];
            Transactions = _allTransactions.Values[index];
        }
    }

    public class APIDataAnalyzer : ObservableObject
    {
        private static readonly Lazy<APIDataAnalyzer> _instance = new Lazy<APIDataAnalyzer>(() => new APIDataAnalyzer());

        private SortedList<string, TransactionGroup> _allTransactions = new SortedList<string, TransactionGroup>();
        private Dictionary<string, TransactionGroup> _dayTransactions = new Dictionary<string, TransactionGroup>();
        private Dictionary<TransactionCategory, TransactionGroup> _categorizedTransactions = new Dictionary<TransactionCategory, TransactionGroup>();
        private TransactionGroup _monthTransactions = new TransactionGroup();
        private DateTime _targetDate;
        private int _budget = 100000;


        public APIDataAnalyzer()
        {
            // TODO Test Action
            LoadTransaction();
        }

        private void AddTransactionData(Transaction transaction)
        {
            TransactionGroup value;
            if (!_allTransactions.TryGetValue(transaction.TransDate, out value))
                _allTransactions.Add(transaction.TransDate, value = new TransactionGroup());
            value.AddTransaction(transaction);
        }

        private void PrepareMonthTransaction()
        {
            _monthTransactions = new TransactionGroup();

            foreach (var ent in GetTransactionsBetween(TargetDate, TargetDate.AddMonths(1)))
            {
                _monthTransactions.AddTransactionGroup(ent.Transactions);
                _dayTransactions.Add(ent.Date, ent.Transactions);
            }

            OnPropertyChanged(nameof(Transactions));
        }

        public IEnumerable<DayTransactions> GetTransactionsBetween(DateTime fromDate, DateTime toDate)
        {
            int from = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, fromDate.ToString("yyyy.MM.dd"));
            int to = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, toDate.ToString("yyyy.MM.dd"));
            for (int i = from; i < to; i++)
                yield return new DayTransactions(_allTransactions, i);
        }

        private void LoadTransaction()
        {
            AddTransactionData(new Transaction()
            {
                Label = "세븐일레븐용인",
                Category = new TransactionCategory("편의점"),
                Amount = 1200,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.18",
                AfterBalanceAmount = 2993512
            });

            AddTransactionData(new Transaction()
            {
                Label = "GS용인외대",
                Category = new TransactionCategory("편의점"),
                Amount = -2800,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.18",
                AfterBalanceAmount = 2991245
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.01",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.17",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.17",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.18",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.23",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.06.01",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.06.11",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.04.19",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.03.19",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.04.15",
                AfterBalanceAmount = 2881512
            });

            DateTime now = DateTime.Now;
            TargetDate = new DateTime(now.Year, now.Month, 1);
        }

        private void AnalyzeTransaction()
        {
            _dayTransactions.Clear();
            _categorizedTransactions.Clear();
            foreach (Transaction ent in Transactions)
            {
                TransactionGroup dayTrans;
                if (!_dayTransactions.TryGetValue(ent.TransDate, out dayTrans))
                    _dayTransactions.Add(ent.TransDate, dayTrans = new TransactionGroup());
                dayTrans.AddTransaction(ent);

                if (ent.Amount < 0)
                {
                    TransactionGroup categoryTrans;
                    if (!_categorizedTransactions.TryGetValue(ent.Category, out categoryTrans))
                        _categorizedTransactions.Add(ent.Category, categoryTrans = new TransactionGroup());
                    categoryTrans.AddTransaction(ent);
                }
            }

            OnPropertyChanged(nameof(CategorizedTransactions));
            OnPropertyChanged(nameof(TotalSpending));
            OnPropertyChanged(nameof(TotalIncoming));
            OnPropertyChanged(nameof(BudgetUsagePercent));
        }

        /// <summary>
        /// date format must be yyyy.mm.dd
        /// </summary>
        public TransactionGroup GetDayTransaction(string date)
        {
            TransactionGroup result;
            if (_dayTransactions.TryGetValue(date, out result))
                return result;
            return null;
        }


        public IEnumerable<Transaction> Transactions
        {
            get => _monthTransactions.Transactions;
        }

        public IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>> CategorizedTransactions
        {
            get => _categorizedTransactions.AsEnumerable();
        }

        public int TotalSpending
        {
            get => _monthTransactions.TotalSpending;
        }

        public int TotalIncoming
        {
            get => _monthTransactions.TotalIncoming;
        }
        
        public int Budget
        {
            get => _budget;
            private set 
            {
                if (_budget != value)
                {
                    _budget = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BudgetUsagePercent));
                }
            }
        }

        public int BudgetUsagePercent
        {
            get => (int)(TotalSpending * 100.0 / Budget);
        }

        /// <summary>
        /// Day must be 1.
        /// </summary>
        public DateTime TargetDate
        {
            get => _targetDate;
            private set 
            {
                if (_targetDate != value)
                {
                    _targetDate = value;
                    PrepareMonthTransaction();
                    AnalyzeTransaction();
                    OnPropertyChanged();
                }
            }
        }

        public static APIDataAnalyzer Current
        {
            get => _instance.Value; 
        }
    }
}
