using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

    public class APIDataManager : ObservableObject, IPropertiesSerializable
    {
        private static readonly Lazy<APIDataManager> _instance = new Lazy<APIDataManager>(() => new APIDataManager());

        private long _budget = 100000;
        private UserAccessToken _accessToken = null;
        private string _user_ci = null;

        private DateTime _targetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        private readonly ObservableCollection<BankAccount> _accounts = new ObservableCollection<BankAccount>();
        private readonly SortedList<string, TransactionGroup> _allTransactions = new SortedList<string, TransactionGroup>();
        private readonly TransactionDataAnalyzer _analyzer = new TransactionDataAnalyzer();


        internal void AddTransactionData(Transaction transaction)
        {
            TransactionGroup value;
            if (!_allTransactions.TryGetValue(transaction.TransDate, out value))
                _allTransactions.Add(transaction.TransDate, value = new TransactionGroup());
            value.AddTransaction(transaction);
        }

        public IEnumerable<DayTransactions> GetTransactionsBetween(DateTime fromDate, DateTime toDate)
        {
            int from = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, fromDate.ToString("yyyyMMdd"));
            int to = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, toDate.ToString("yyyyMMdd"));
            for (int i = from; i < to; i++)
                yield return new DayTransactions(_allTransactions, i);
        }

        public async void RefreshAccountData()
        {
            AccessToken = new UserAccessToken(JObject.Parse("{" +
                "\"access_token\": \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiIxMTAwNzczNDcxIiwic2NvcGUiOlsiaW5xdWlyeSIsImxvZ2luIl0sImlzcyI6Imh0dHBzOi8vd3d3Lm9wZW5iYW5raW5nLm9yLmtyIiwiZXhwIjoxNjMxMTk3NzIwLCJqdGkiOiIwMDQ5ODVlNC1hYmIzLTQwNjktOTJmNi05Njg3ZTQ2YjVmYWIifQ.bERqN3qBxgXF-xAI70iOCSKsjJreE8o3FB8mycbPKfM\", " +
                "\"token_type\": \"Bearer\", " +
                "\"refresh_token\": \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiIxMTAwNzczNDcxIiwic2NvcGUiOlsiaW5xdWlyeSIsImxvZ2luIl0sImlzcyI6Imh0dHBzOi8vd3d3Lm9wZW5iYW5raW5nLm9yLmtyIiwiZXhwIjoxNjMyMDYxNzIwLCJqdGkiOiIwZWVmNjI3MS1mMzFmLTRiYmYtOTE4YS04ODEyZjNlNmM3NGMifQ.yZOFcCEHDrrB4Eh8Aq3-O6H8bHF96Pm6hK4SQA-QkUM\", " +
                "\"expires_in\": 7775999, " +
                "\"scope\": \"inquiry login\", " +
                "\"user_seq_no\": \"1100773471\"" +
            "}"));

            if (AccessToken != null)
            {
                Tuple<string, BankAccount[]> info = null;
                try
                {
                    info = await APIs.GetUserInfo(AccessToken);
                    _user_ci = info.Item1;

                    // load account balance amount and transactions.
                    int i = 0;
                    await Task.Factory.StartNew(() => info.Item2.AsParallel().ForAll((acc) => acc.RetrieveAccountDetail(info.Item2, i++)));
                } 
                catch (Exception e)
                {
                    Logger.Error(e);
                }

                // merge loaded accounts to list
                if (info != null)
                {
                    foreach (BankAccount account in info.Item2)
                    {
                        int idx = _accounts.IndexOf(account);
                        if (idx != -1)
                        {
                            _accounts.RemoveAt(idx);
                            _accounts.Insert(idx, account);
                        }
                        else
                        {
                            _accounts.Add(account);
                        }
                    }
                }

                Analyzer.Update();
                OnPropertyChanged(nameof(BankAccounts));
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            // access token
            if (reader.ReadBoolean())
                AccessToken = new UserAccessToken(reader);

            // budget
            Budget = reader.ReadInt64();

            // user ci
            if (reader.ReadBoolean())
                _user_ci = reader.ReadString();

            RefreshAccountData();
        }

        public void Serialize(BinaryWriter writer)
        {
            // access token
            writer.Write(AccessToken != null);
            if (AccessToken != null)
                AccessToken.Serialize(writer);
            
            // budget
            writer.Write(Budget);

            // user ci
            writer.Write(_user_ci != null);
            if (_user_ci != null)
                writer.Write(_user_ci);
        }


        public TransactionDataAnalyzer Analyzer
        {
            get => _analyzer;
        }

        public UserAccessToken AccessToken
        {
            get => _accessToken;
            private set => _accessToken = value;
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

        public static APIDataManager Current
        {
            get => _instance.Value; 
        }
    }
}
