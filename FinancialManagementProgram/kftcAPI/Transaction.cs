using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public class Transaction
    {
        public string Label { get; set; }
        public TransactionCategory Category { get; set; }
        public int Amount { get; set; }
        public string BankName { get; set; }
        public string AccountFintechNum { get; set; }

        public string Description { get; set; }
        public string TransDate { get; set; }
        public int AfterBalanceAmount { get; set; }
    }
}
