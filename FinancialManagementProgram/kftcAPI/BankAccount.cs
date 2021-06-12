using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public enum AccountColor
    {
        Blue, Red, Yellow, Green, Black
    }

    public class BankAccount
    {
        public BankAccount(JObject obj)
        {
            FintechUseNum = obj.Value<string>("fintech_use_num");
            Label = obj.Value<string>("account_alias");
            AccountNum = obj.Value<string>("account_num_masked");
            AccountAlias = obj.Value<string>("account_alias");
            BankName = obj.Value<string>("bank_name");
            BalanceAmount = 0;
            Color = AccountColor.Blue;
            LastSyncDate = DateTime.Now;
        }

        public void RetrieveAccountDetail(BankAccount[] prevAccountList, int idOffset)
        {
            DateTime from = DateTime.Now.AddMonths(-5);
            if (prevAccountList.Contains(this))
                from = LastSyncDate;

            try
            {
                BalanceAmount = APIs.GetAccountDetails(this, APIDataManager.Current.AccessToken, idOffset, from.ToString("yyyyMMdd")).Result;
                foreach (Transaction t in Transactions.Transactions)
                    APIDataManager.Current.AddTransactionData(t);
                Transactions.ClearTransactions();
            } 
            catch (AggregateException e)
            {
                Logger.Error(e);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is BankAccount == false)
                return false;
            return FintechUseNum.Equals(((BankAccount)obj).FintechUseNum);
        }

        public override int GetHashCode()
        {
            return FintechUseNum.GetHashCode();
        }

        public string FintechUseNum { get; }
        public string Label { get; }
        public string AccountNum { get; }
        public string AccountAlias { get; }
        public string BankName { get; }
        public long BalanceAmount { get; private set; }
        public AccountColor Color { get; }
        public TransactionGroup Transactions { get; } = new TransactionGroup();

        public DateTime LastSyncDate { get; }
        public string Memo { get; } = ".";
    }
}
