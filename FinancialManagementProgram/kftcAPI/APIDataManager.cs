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
        private volatile bool usingAPISession = false;


        public IEnumerable<DayTransactions> GetTransactionsBetween(DateTime fromDate, DateTime toDate)
        {
            int from = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, fromDate.ToString("yyyyMMdd"));
            int to = CommonUtil.FindSortedInsertionIndex(_allTransactions.Keys, toDate.ToString("yyyyMMdd"));
            for (int i = from; i < to; i++)
                yield return new DayTransactions(_allTransactions, i);
        }

        public async void AddNewAccount()
        {
            if (!usingAPISession)
            {
                usingAPISession = true;
                AccessToken = await APIs.GenerateToken(false);
                BinaryProperties.Save();
                usingAPISession = false;
            }
        }

        public async void RefreshAccountData()
        {
            if (usingAPISession)
                return;

            usingAPISession = true;
            try
            {
                if (AccessToken == null) // accesstoken이 없을 때
                    AccessToken = await APIs.GenerateToken(true); // TODO 회원등록 안한 상태에서 gentoken하면 어떻게 될까?

                if (AccessToken != null && AccessToken.ExpiresAt - DateTime.Now < TimeSpan.FromDays(7)) // accesstoken이 만료되었을 때
                    AccessToken = await APIs.RefreshAccessToken(AccessToken);

                if (AccessToken == null) // accesstoken 생성 및 갱신에 실패했을 때
                {
                    usingAPISession = false;
                    return;
                }

                Tuple<string, BankAccount[]> info = await APIs.GetUserInfo(AccessToken);
                _user_ci = info.Item1;

                List<int> account_indexes = new List<int>();
                foreach (BankAccount account in info.Item2)
                {
                    int idx = _accounts.IndexOf(account);
                    if (idx != -1)
                        account.CopyMetadataFrom(_accounts[idx]);
                    account_indexes.Add(idx);
                }

                // load account balance amount and transactions.
                // TODO 지금은 API가 DAte반영을 안하지만, 나중에 date반영할 때 다시 테스트
                int i = 0;
                await Task.Factory.StartNew(() => info.Item2.AsParallel().ForAll((acc) =>
                {
                    string prevLastUpdate = acc.LastSyncDate.ToString("yyyyMMdd");
                    acc.RetrieveAccountDetail(i++);

                    foreach (Transaction t in acc.Transactions.Transactions)
                    {
                        if (!_allTransactions.TryGetValue(t.TransDate, out TransactionGroup value))
                            _allTransactions.Add(t.TransDate, value = new TransactionGroup());
                        if (prevLastUpdate != t.TransDate || !value.Transactions.Contains(t))
                            value.AddTransaction(t);
                    }
                    acc.Transactions.ClearTransactions();
                }));

                for (i = 0; i < account_indexes.Count; i++)
                {
                    int idx = account_indexes[i];
                    BankAccount acc = info.Item2[i];
                    if (idx != -1)
                    {
                        _accounts.RemoveAt(idx);
                        _accounts.Insert(idx, acc);
                    }
                    else
                    {
                        _accounts.Add(acc);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            BinaryProperties.Save();
            Analyzer.Update();
            OnPropertyChanged(nameof(BankAccounts));
            usingAPISession = false;
        }

        // UNDONE: Transactions도 저장하자
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

            // accounts
            int account_len = reader.ReadInt32();
            _accounts.Clear();
            for (int i = 0; i < account_len; i++)
                _accounts.Add(new BankAccount(reader));
            OnPropertyChanged(nameof(BankAccounts));
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

            // accounts
            writer.Write(_accounts.Count);
            foreach (BankAccount account in _accounts)
                account.Serialize(writer);
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
