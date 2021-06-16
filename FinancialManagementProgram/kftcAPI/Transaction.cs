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
            TransDateTime = DateTime.ParseExact(TransDate + obj.Value<string>("tran_time"), "yyyyMMddHHmmss", null);
            AfterBalanceAmount = obj.Value<long>("after_balance_amt");

            if (obj.Value<string>("inout_type") == "출금")
                Amount = -Amount;
        }

        public string Label { get; }
        public TransactionCategory Category { get; }
        public long Amount { get; }
        public string BankName { get; }
        public string FintechNum { get; }

        public string Description { get; }
        public string TransDate { get; }
        public DateTime TransDateTime { get; }
        public long AfterBalanceAmount { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is Transaction == false)
                return false;
            Transaction target = (Transaction)obj;
            return TransDateTime == target.TransDateTime && Label == target.Label && Amount == target.Amount;
        }

        public override int GetHashCode()
        {
            return TransDateTime.GetHashCode();
        }

        public string FormattedTransDate
        {
            get => string.Format("{0}.{1}.{2}", TransDate.Substring(0, 4), TransDate.Substring(4, 2), TransDate.Substring(6, 2));
        }

        public string FormattedTransDateTime
        {
            get => TransDateTime.ToString();
        }
    }
}
