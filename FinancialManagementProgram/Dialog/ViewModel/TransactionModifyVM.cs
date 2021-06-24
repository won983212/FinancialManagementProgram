using FinancialManagementProgram.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialManagementProgram.Dialog.ViewModel
{
    /// <summary>
    /// 반드시 Account와 TransactionCategory가 1개 이상 존재해야 한다.
    /// </summary>
    class TransactionModifyVM : DialogViewModel
    {
        private readonly DataManager _dataManager;
        private bool _applySameTypeAllTransaction = false;

        public TransactionModifyVM(DataManager dataManager, Transaction transaction)
        {
            if (dataManager.BankAccounts.Count == 0)
                throw new InvalidOperationException("가용한 자산이 없습니다.");

            if (TransactionCategory.Categories.Count() == 0)
                throw new InvalidOperationException("가용한 카테고리가 없습니다.");

            _dataManager = dataManager;
            TransactionObj = new Transaction()
            {
                TransDateTime = DateTime.Now,
                Category = TransactionCategory.GetCategory(TransactionCategory.UnknownCategoryID),
                Account = dataManager.BankAccounts.First()
            };

            if (transaction != null)
                TransactionObj.Copy(transaction);

            ApplySameTypeAllTransaction = dataManager.HasCategoryMark(TransactionObj.Label);
        }


        public bool HasError { get; set; }

        public string Label
        {
            get => TransactionObj.Label;
            set
            {
                TransactionObj.Label = value;
                ApplySameTypeAllTransaction = _dataManager.HasCategoryMark(value);
                if (ApplySameTypeAllTransaction)
                    TransactionObj.Category = _dataManager.GetDefaultCategory(value);
                OnPropertyChanged();
            }
        }

        public Transaction TransactionObj { get; }

        public bool ApplySameTypeAllTransaction
        {
            get => _applySameTypeAllTransaction;
            set
            {
                _applySameTypeAllTransaction = value;
                OnPropertyChanged();
            }
        }

        public DateTime TransDate
        {
            get => TransactionObj.TransDateTime;
            set
            {
                DateTime prev = TransactionObj.TransDateTime;
                TransactionObj.TransDateTime = new DateTime(value.Year, value.Month, value.Day, prev.Hour, prev.Minute, prev.Second);
            }
        }

        public DateTime TransTime
        {
            get => TransactionObj.TransDateTime;
            set
            {
                DateTime prev = TransactionObj.TransDateTime;
                TransactionObj.TransDateTime = new DateTime(prev.Year, prev.Month, prev.Day, value.Hour, value.Minute, value.Second);
            }
        }

        public IEnumerable<BankAccount> AccountsList
        {
            get => _dataManager.BankAccounts;
        }
    }
}
