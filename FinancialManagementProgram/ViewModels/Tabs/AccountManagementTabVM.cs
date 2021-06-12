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
                OnPropertyChanged("AccountTotalSpending");
                OnPropertyChanged("AccountTotalIncoming");
            }
        }

        private TransactionGroup GetSelectedAccountTransactions()
        {
            APIDataManager dataManager = APIDataManager.Current;
            BankAccount account = dataManager.BankAccounts[SelectedAccountIndex];
            return dataManager.GetAccountTransaction(account.FintechUseNum); // TODO* Monthly bank account transaction.
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
