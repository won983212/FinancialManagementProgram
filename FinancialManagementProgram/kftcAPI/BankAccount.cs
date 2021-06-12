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

        // TODO 이 생성자는 사용하지 않을 예정임. 테스트가 끝나면 삭제
        public BankAccount() { }

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

        // TODO Readonly로 만들자. 방법은 constructor에서 json parsing해서 초기화함.
        public string FintechUseNum { get; set; }
        public string Label { get; set; }
        public string AccountNum { get; set; }
        public string AccountAlias { get; set; }
        public string BankName { get; set; }
        public long BalanceAmount { get; set; }
        public AccountColor Color { get; set; }
        public TransactionGroup Transactions { get; } = new TransactionGroup();

        public DateTime LastSyncDate { get; set; }
        public string Memo { get; set; } = ".";
    }
}
