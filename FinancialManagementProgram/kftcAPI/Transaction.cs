using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public class Transaction
    {
        // TODO: Test. 나중에 삭제예정
        public Transaction() { }

        public Transaction(BankAccount account, JObject obj)
        {
            BankName = account.BankName;
            FintechNum = account.FintechUseNum;
            Category = new TransactionCategory("게임"); // TODO Category realize
            Label = obj.Value<string>("branch_name");
            Amount = obj.Value<long>("tran_amt");
            Description = obj.Value<string>("print_content");
            TransDate = obj.Value<string>("tran_date");
            AfterBalanceAmount = obj.Value<long>("after_balance_amt");
        }

        // TODO Readonly로 만들자. 방법은 constructor에서 json parsing해서 초기화함.
        public string Label { get; set; }
        public TransactionCategory Category { get; set; }
        public long Amount { get; set; }
        public string BankName { get; set; }
        public string FintechNum { get; set; }

        public string Description { get; set; }
        public string TransDate { get; set; }
        public long AfterBalanceAmount { get; set; }
    }
}
