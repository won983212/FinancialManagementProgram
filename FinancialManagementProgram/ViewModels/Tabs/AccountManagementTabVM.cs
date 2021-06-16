using FinancialManagementProgram.kftcAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class AccountManagementTabVM : TabChild
    {
        private int _selectedAccountIndex = 0;

        public AccountManagementTabVM(TabContainer parent)
            : base(parent)
        { }

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

        public ICommand AddCommand => new RelayCommand(() => APIDataManager.Current.AddNewAccount());
    }
}
