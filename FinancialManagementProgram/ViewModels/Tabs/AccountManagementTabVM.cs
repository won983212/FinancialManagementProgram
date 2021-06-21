﻿using FinancialManagementProgram.Data;
using FinancialManagementProgram.Dialog;
using MaterialDesignThemes.Wpf;
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
            if (accounts.Count == 0 || SelectedAccountIndex < 0)
                return null;
            return accounts[SelectedAccountIndex];
        }

        private void OnDeleteDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                DataManager.DeleteAccount(GetSelectedAccount());
                SelectedAccountIndex = 0;
            }
        }

        private void OnAddDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                AddAccountDialog dialog = CommonUtil.GetDialog<AddAccountDialog>(o);
                if (string.IsNullOrWhiteSpace(dialog.Label) || string.IsNullOrWhiteSpace(dialog.BankName))
                    return;

                DataManager.AddAccount(new BankAccount(DataManager.GenerateUniqueAccountID())
                {
                    Label = dialog.Label,
                    BankName = dialog.BankName,
                    Color = (AccountColor)dialog.ColorIndex,
                    Memo = dialog.Memo
                });
            }
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

        public ICommand AddCommand => new RelayCommand(() => CommonUtil.ShowDialog(new AddAccountDialog(), OnAddDialogClosed));
        public ICommand EditCommand => new RelayCommand(() => IsEditing = !IsEditing);
        public ICommand DeleteCommand => new RelayCommand(() => CommonUtil.ShowDialog(
            new MessageDialog("자산을 삭제합니다.", "자산과 관련 거래내역이 영구적으로 삭제되며, 통계에 영향을 줄 수 있습니다. 그래도 삭제하시겠습니까?"), OnDeleteDialogClosed));
    }
}
