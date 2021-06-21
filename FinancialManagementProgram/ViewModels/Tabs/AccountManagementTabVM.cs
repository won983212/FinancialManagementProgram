using FinancialManagementProgram.Data;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class AccountManagementTabVM : TabChild
    {
        private bool _isEditing = false;
        private int _selectedAccountIndex = 0;

        public AccountManagementTabVM(TabContainer parent)
            : base(parent)
        { }

        private BankAccount GetSelectedAccount()
        {
            IList<BankAccount> accounts = DataManager.BankAccounts;
            if (accounts.Count == 0)
                return null;
            return accounts[SelectedAccountIndex];
        }

        public long AccountTotalSpending
        {
            get
            {
                BankAccount account = GetSelectedAccount();
                if (account == null || account.MonthlyTransactions == null)
                    return 0;
                return account.MonthlyTransactions.TotalSpending;
            }
        }

        public long AccountTotalIncoming
        {
            get
            {
                BankAccount account = GetSelectedAccount();
                if (account == null || account.MonthlyTransactions == null)
                    return 0;
                return account.MonthlyTransactions.TotalIncoming;
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

        public int SelectedAccountColor
        {
            get 
            {
                BankAccount account = GetSelectedAccount();
                return account == null ? 0 : (int)account.Color;
            }

            set
            {
                BankAccount account = GetSelectedAccount();
                if (account != null)
                    account.Color = (AccountColor)value;
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set 
            { 
                _isEditing = value; 
                OnPropertyChanged();
            }
        }

        public string[] AccountColorItems
        {
            get => Enum.GetNames(typeof(AccountColor));
        }

        public ICommand AddCommand => new RelayCommand(() => Console.WriteLine("ADD")); // TODO implement
        public ICommand EditCommand => new RelayCommand(() => IsEditing = !IsEditing);
    }
}
