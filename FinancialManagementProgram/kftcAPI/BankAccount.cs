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
        // TODO Readonly로 만들자. 방법은 constructor에서 json parsing해서 초기화함.
        public string FintechUseNum { get; set; }
        public string Label { get; set; }
        public string AccountNum { get; set; }
        public string AccountAlias { get; set; }
        public string BankName { get; set; }
        public int BalanceAmount { get; set; }
        public AccountColor Color { get; set; }

        public DateTime LastSyncDate { get; set; }
        public string Memo { get; set; } = ".";
    }
}
