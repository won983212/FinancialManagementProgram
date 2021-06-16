using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
            BankName = obj.Value<string>("bank_name");
            BalanceAmount = 0;
            Color = AccountColor.Blue;
            LastSyncDate = DateTime.Now.AddMonths(-5);
        }

        public BankAccount(BinaryReader reader)
        {
            FintechUseNum = reader.ReadString();
            Label = reader.ReadString();
            AccountNum = reader.ReadString();
            BankName = reader.ReadString();
            BalanceAmount = reader.ReadInt64();
            Color = (AccountColor) reader.ReadInt16();
            LastSyncDate = new DateTime(reader.ReadInt64());
        }

        internal void RetrieveAccountDetail(int idOffset)
        {
            try
            {
                BalanceAmount = APIs.GetAccountDetails(this, APIDataManager.Current.AccessToken, idOffset, LastSyncDate.ToString("yyyyMMdd")).Result;
                LastSyncDate = DateTime.Now;
            } 
            catch (AggregateException e)
            {
                Logger.Error(e);
            }
        }

        public void CopyMetadataFrom(BankAccount src)
        {
            LastSyncDate = src.LastSyncDate;
            Color = src.Color;
            Memo = src.Memo;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(FintechUseNum);
            writer.Write(Label);
            writer.Write(AccountNum);
            writer.Write(BankName);
            writer.Write(BalanceAmount);
            writer.Write((short)Color);
            writer.Write(LastSyncDate.Ticks);
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
        public string BankName { get; }
        public long BalanceAmount { get; private set; }
        public TransactionGroup Transactions { get; } = new TransactionGroup();

        public DateTime LastSyncDate { get; private set; }
        public AccountColor Color { get; private set; } // TODO (Later): PropertyChanged안해서 바로 업데이트 안될텐데 체크해보자.
        public string Memo { get; private set; } = ".";
    }
}
