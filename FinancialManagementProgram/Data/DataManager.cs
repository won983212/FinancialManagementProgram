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
        private readonly Dictionary<long, BankAccount> _accountsIDMap;
        private readonly ReadOnlyObservableCollection<BankAccount> _accountsReadonly;
        private readonly SortedList<int, TransactionGroup> _allTransactions;
        private readonly TransactionDataAnalyzer _analyzer;


        private DataManager()
        {
            _accounts = new ObservableCollection<BankAccount>();
            _accountsIDMap = new Dictionary<long, BankAccount>();
            _accountsReadonly = new ReadOnlyObservableCollection<BankAccount>(_accounts);
            _allTransactions = new SortedList<int, TransactionGroup>();
            _analyzer = new TransactionDataAnalyzer(this);
            TargetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public void AddTransaction(Transaction t)
        {
            int date = CommonUtil.GetIntegerDate(t.TransDateTime);
            if (!_allTransactions.TryGetValue(date, out TransactionGroup group))
                _allTransactions.Add(date, group = new TransactionGroup());
            group.AddTransaction(t);
            Analyzer.Update();
            BinaryProperties.Save();
        }

        public void DeleteTransaction(Transaction t)
        {
            int date = CommonUtil.GetIntegerDate(t.TransDateTime);
            if(_allTransactions.TryGetValue(date, out TransactionGroup group))
            {
                if (group.DeleteTransaction(t))
                    _allTransactions.Remove(date);
            }
            Analyzer.Update();
            BinaryProperties.Save();
        }

        public void AddAccount(BankAccount account)
        {
            _accounts.Add(account);
            _accountsIDMap.Add(account.ID, account);
            BinaryProperties.Save();
        }

        public void DeleteAccount(BankAccount account)
        {
            _accounts.Remove(account);
            _accountsIDMap.Remove(account.ID);
            List<int> deletionList = new List<int>();
            foreach (var ent in _allTransactions)
            {
                if (ent.Value.DeleteTransactionsByAccount(account))
                    deletionList.Add(ent.Key);
            }
            foreach (int key in deletionList)
                _allTransactions.Remove(key);
            Analyzer.Update();
            BinaryProperties.Save();
        }

        public void RevalidateTransactionDatas()
        {
            foreach (var ent in _allTransactions)
                ent.Value.RevalidateTotalSum();
        }

        private bool ContainsIDInAccount(long id)
        {
            return _accountsIDMap.ContainsKey(id);
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

        public BankAccount FindAccount(long accountId)
        {
            if(_accountsIDMap.TryGetValue(accountId, out BankAccount value))
                return value;
            return null;
        }

        public void Deserialize(BinaryReader reader)
        {
            // budget
            Budget = reader.ReadInt64();

            // accounts
            int account_len = reader.ReadInt32();
            _accounts.Clear();
            for (int i = 0; i < account_len; i++)
            {
                BankAccount account = new BankAccount(reader);
                _accounts.Add(account);
                _accountsIDMap.Add(account.ID, account);
            }
            OnPropertyChanged(nameof(BankAccounts));

            // transactions
            int transaction_len = reader.ReadInt32();
            _allTransactions.Clear();
            for (int i = 0; i < transaction_len; i++)
            {
                int key = reader.ReadInt32();
                TransactionGroup value = new TransactionGroup();
                value.Deserialize(this, reader);
                _allTransactions.Add(key, value);
            }
            Analyzer.Update();
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
            get => _accountsReadonly;
        }

        public static DataManager Current
        {
            get => _instance.Value;
        }
    }
}
