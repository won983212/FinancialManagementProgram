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
        public Transaction(BankAccount account, JObject obj)
        {
            BankName = account.BankName;
            FintechNum = account.FintechUseNum;
            Category = new TransactionCategory("게임"); // TODO Category realize
            Label = obj.Value<string>("print_content");
            Amount = obj.Value<long>("tran_amt");
            Description = obj.Value<string>("branch_name");
            TransDate = obj.Value<string>("tran_date");
            AfterBalanceAmount = obj.Value<long>("after_balance_amt");
        }

        public string Label { get; }
        public TransactionCategory Category { get; }
        public long Amount { get; }
        public string BankName { get; }
        public string FintechNum { get; }

        public string Description { get; }
        public string TransDate { get; }
        public long AfterBalanceAmount { get; }


        // TODO Transaction보여줄 때 그룹은 날짜별로, 아이템은 시간별로 정렬하기.
        public string FormattedTransDate
        {
            get => string.Format("{0}.{1}.{2}", TransDate.Substring(0, 4), TransDate.Substring(4, 2), TransDate.Substring(6, 2));
        }
    }
}
