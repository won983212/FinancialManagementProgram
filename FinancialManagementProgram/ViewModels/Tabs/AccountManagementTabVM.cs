using FinancialManagementProgram.kftcAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class AccountManagementTabVM : TabChild
    {
        private int _selectedAccountIndex = 0;

        // TODO Account가 없는 경우도 handling 해줘라. 없으면 창이 아얘 다른화면으로 바뀌게!
        public AccountManagementTabVM(TabContainer parent)
            : base(parent)
        { }


        public int SelectedAccountIndex
        {
            get => _selectedAccountIndex;
            set
            {
                _selectedAccountIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AccountTotalSpending));
                OnPropertyChanged(nameof(AccountTotalIncoming));
            }
        }

        private TransactionGroup GetSelectedAccountTransactions()
        {
            IList<BankAccount> accounts = APIDataManager.Current.BankAccounts;
            if (accounts.Count == 0)
                return null;
            return accounts[SelectedAccountIndex].Transactions;
        }

        public long AccountTotalSpending
        {
            get
            {
                TransactionGroup transactions = GetSelectedAccountTransactions();
                if (transactions == null)
                    return 0;
                return transactions.TotalSpending;
            }
        }

        public long AccountTotalIncoming
        {
            get
            {
                TransactionGroup transactions = GetSelectedAccountTransactions();
                if (transactions == null)
                    return 0;
                return transactions.TotalIncoming;
            }
        }
    }
}
