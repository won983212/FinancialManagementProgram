using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public class BankAccount
    {
        public string FintechUseNum { get; set; }
        public string Label { get; set; }
        public string AccountNum { get; set; }
        public string AccountAlias { get; set; }
        public string BankName { get; set; }
        public int BalanceAmount { get; set; }

        public long LastSyncDate { get; set; }
        public string Memo { get; set; }
    }
}
