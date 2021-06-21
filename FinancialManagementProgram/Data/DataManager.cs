using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace FinancialManagementProgram.Data
{
    public class DayTransactions
    {
        public int Date { get; }
        public TransactionGroup Transactions { get; }

        internal DayTransactions(SortedList<int, TransactionGroup> _allTransactions, int index)
        {
            Date = _allTransactions.Keys[index];
            Transactions = _allTransactions.Values[index];
        }
    }

    public class DataManager : ObservableObject, IPropertiesSerializable
    {
        private static readonly Lazy<DataManager> _instance = new Lazy<DataManager>(() => new DataManager());

        private Random random = new Random();
        private long _budget = 100000;
        private DateTime _targetDate;
        private readonly ObservableCollection<BankAccount> _accounts;
        private readonly SortedList<int, TransactionGroup> _allTransactions;
        private readonly TransactionDataAnalyzer _analyzer;


        private DataManager()
        {
            _targetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _accounts = new ObservableCollection<BankAccount>();
            _allTransactions = new SortedList<int, TransactionGroup>();
            _analyzer = new TransactionDataAnalyzer(this);

            // TODO: Test
            _accounts.Add(new BankAccount(GenerateUniqueAccountID())
            {
                Label = "우리은행주계좌",
                BankName = "우리은행",
                Color = AccountColor.Blue,
                Memo = "Test Memo"
            });

            _accounts.Add(new BankAccount(GenerateUniqueAccountID())
            {
                Label = "농협은행",
                BankName = "농협은행",
                Color = AccountColor.Green,
                Memo = "농협은행이다"
            });

            _accounts.Add(new BankAccount(GenerateUniqueAccountID())
            {
                Label = "청약저축계좌",
                BankName = "우리은행",
                Color = AccountColor.Yellow,
                Memo = "청약저축할 계좌다"
            });
        }

        private bool ContainsIDInAccount(long id)
        {
            foreach (BankAccount account in _accounts)
                if (account.ID == id)
                    return true;
            return false;
        }

        public long GenerateUniqueAccountID()
        {
            long x;
            do
            {
                x = random.Next() << sizeof(int) * 8;
                x += random.Next();
            } 
            while (ContainsIDInAccount(x));
            return x;
        }

        public IEnumerable<DayTransactions> GetTransactionsBetween(DateTime fromDate, DateTime toDate)
        {
            int from = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, CommonUtil.GetIntegerDate(fromDate));
            int to = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, CommonUtil.GetIntegerDate(toDate));
            for (int i = from; i < to; i++)
                yield return new DayTransactions(_allTransactions, i);
        }

        public BankAccount GetAccount(long accountId)
        {
            foreach (BankAccount account in _accounts)
            {
                if (account.ID == accountId)
                    return account;
            }
            return null;
        }

        public void Deserialize(BinaryReader reader)
        {
            // budget
            Budget = reader.ReadInt64();

            // accounts
            // TODO: Test
            /*int account_len = reader.ReadInt32();
            _accounts.Clear();
            for (int i = 0; i < account_len; i++)
                _accounts.Add(new BankAccount(reader));
            OnPropertyChanged(nameof(BankAccounts));*/

            // transactions
            int transaction_len = reader.ReadInt32();
            _allTransactions.Clear();
            for (int i = 0; i < transaction_len; i++)
            {
                int key = reader.ReadInt32();
                TransactionGroup value = new TransactionGroup();
                value.Deserialize(reader);
                _allTransactions.Add(key, value);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            // budget
            writer.Write(Budget);

            // accounts
            writer.Write(_accounts.Count);
            foreach (BankAccount account in _accounts)
                account.Serialize(writer);

            // transactions
            writer.Write(_allTransactions.Count);
            foreach (var ent in _allTransactions)
            {
                writer.Write(ent.Key);
                ent.Value.Serialize(writer);
            }
        }


        public TransactionDataAnalyzer Analyzer
        {
            get => _analyzer;
        }

        public long Budget
        {
            get => _budget;
            private set
            {
                if (_budget != value)
                {
                    _budget = value;
                    OnPropertyChanged();
                    Analyzer.NotifyBudgetChanges();
                }
            }
        }

        public DateTime TargetDate
        {
            get => _targetDate;
            private set
            {
                if (_targetDate != value)
                {
                    _targetDate = value;
                    if (_targetDate.Day != 1)
                        _targetDate = new DateTime(_targetDate.Year, _targetDate.Month, 1);
                    Analyzer.Update();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BankAccounts));
                }
            }
        }

        public IList<BankAccount> BankAccounts
        {
            get => _accounts;
        }

        public static DataManager Current
        {
            get => _instance.Value;
        }
    }
}
