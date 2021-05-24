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

    public class APIDataManager : ObservableObject
    {
        private static readonly Lazy<APIDataManager> _instance = new Lazy<APIDataManager>(() => new APIDataManager());

        private DateTime _targetDate;
        private int _budget = 100000;
        private List<BankAccount> _accounts = new List<BankAccount>(); // TODO 나중에 추가 및 삭제작업시 Update가 필요해보임
        private SortedList<string, TransactionGroup> _allTransactions = new SortedList<string, TransactionGroup>();
        private TransactionDataAnalyzer _analyzer = new TransactionDataAnalyzer();

        // monthly data
        private Dictionary<string, TransactionGroup> _accountTransactions = new Dictionary<string, TransactionGroup>();
        private Dictionary<string, TransactionGroup> _dayTransactions = new Dictionary<string, TransactionGroup>();
        private Dictionary<TransactionCategory, TransactionGroup> _categorizedTransactions = new Dictionary<TransactionCategory, TransactionGroup>();
        private TransactionGroup _monthTransactions = new TransactionGroup();


        public APIDataManager()
        {
            // TODO Test Action
            LoadTransaction();
            LoadAccounts();
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

                foreach (Transaction t in ent.Transactions.Transactions)
                {
                    TransactionGroup group;
                    if (!_accountTransactions.TryGetValue(t.AccountFintechNum, out group))
                        _accountTransactions.Add(t.AccountFintechNum, group = new TransactionGroup());
                    group.AddTransaction(t);
                }
            }

            OnPropertyChanged(nameof(Transactions));
            OnPropertyChanged(nameof(TotalSpending));
            OnPropertyChanged(nameof(TotalIncoming));
            OnPropertyChanged(nameof(BudgetUsagePercent));
        }

        public IEnumerable<DayTransactions> GetTransactionsBetween(DateTime fromDate, DateTime toDate)
        {
            int from = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, fromDate.ToString("yyyy.MM.dd"));
            int to = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, toDate.ToString("yyyy.MM.dd"));
            for (int i = from; i < to; i++)
                yield return new DayTransactions(_allTransactions, i);
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

        /// <summary>
        /// Can get transaction in <b>this month</b> only
        /// </summary>
        public TransactionGroup GetAccountTransaction(string fintechNum)
        {
            TransactionGroup result;
            if (_accountTransactions.TryGetValue(fintechNum, out result))
                return result;
            return null;
        }

        private void LoadAccounts()
        {
            _accounts.Add(new BankAccount()
            {
                FintechUseNum = "10103812401289408",
                Label = "알뜰계좌",
                AccountAlias = "알뜰적금상품",
                AccountNum = "1002 - 356 - ******",
                BalanceAmount = 3000000,
                BankName = "우리은행",
                LastSyncDate = DateTime.Now,
                Color = AccountColor.Blue,
                Memo = ""
            });

            _accounts.Add(new BankAccount()
            {
                FintechUseNum = "92385716893257236",
                Label = "주거래계좌",
                AccountAlias = "주거래계좌상품",
                AccountNum = "3261 - 437 - ******",
                BalanceAmount = 1251256,
                BankName = "하나은행",
                LastSyncDate = DateTime.Now,
                Color = AccountColor.Red,
                Memo = ""
            });

            _accounts.Add(new BankAccount()
            {
                FintechUseNum = "193285761239567821",
                Label = "청약통장",
                AccountAlias = "청년청약저축",
                AccountNum = "1361 - 361 - ******",
                BalanceAmount = 120000,
                BankName = "농협은행",
                LastSyncDate = DateTime.Now,
                Color = AccountColor.Green,
                Memo = ""
            });
        }

        private void LoadTransaction()
        {
            AddTransactionData(new Transaction()
            {
                Label = "세븐일레븐용인",
                Category = new TransactionCategory("편의점"),
                Amount = 1200,
                BankName = "우리은행체크",
                AccountFintechNum = "10103812401289408",
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
                AccountFintechNum = "10103812401289408",
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
                AccountFintechNum = "10103812401289408",
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
                AccountFintechNum = "10103812401289408",
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
                AccountFintechNum = "10103812401289408",
                Description = "결제 승인",
                TransDate = "2021.05.17",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "농협은행체크",
                AccountFintechNum = "193285761239567821",
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
                AccountFintechNum = "10103812401289408",
                Description = "결제 승인",
                TransDate = "2021.05.23",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "농협은행체크",
                AccountFintechNum = "193285761239567821",
                Description = "결제 승인",
                TransDate = "2021.06.01",
                AfterBalanceAmount = 2881512
            });

            AddTransactionData(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory("게임"),
                Amount = -6000,
                BankName = "농협은행체크",
                AccountFintechNum = "193285761239567821",
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
                AccountFintechNum = "10103812401289408",
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
                AccountFintechNum = "10103812401289408",
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
                AccountFintechNum = "10103812401289408",
                Description = "결제 승인",
                TransDate = "2021.04.15",
                AfterBalanceAmount = 2881512
            });

            DateTime now = DateTime.Now;
            TargetDate = new DateTime(now.Year, now.Month, 1);
        }

        private void CategorizeTransaction()
        {
            _categorizedTransactions.Clear();
            foreach (Transaction ent in Transactions)
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

        public TransactionDataAnalyzer Analyzer
        {
            get => _analyzer;
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
                    CategorizeTransaction();
                    Analyzer.Update();
                    OnPropertyChanged();
                }
            }
        }

        public IList<BankAccount> BankAccounts
        {
            get => _accounts;
        }

        public static APIDataManager Current
        {
            get => _instance.Value; 
        }
    }
}
