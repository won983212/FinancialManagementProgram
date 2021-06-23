using FinancialManagementProgram.Data;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinancialManagementProgram.Dialog
{
    /// <summary>
    /// 반드시 Account와 TransactionCategory가 1개 이상 존재해야 한다.
    /// </summary>
    public partial class TransactionModifyDialog : UserControl, INotifyPropertyChanged
    {
        // TODO Dialog도 전부 VM만들어봐
        public event PropertyChangedEventHandler PropertyChanged;

        private DataManager _dataManager;
        private bool _applySameTypeAllTransaction = false;
        private int _errorCount = 0;

        public TransactionModifyDialog(DataManager dataManager, Transaction transaction)
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
            InitializeComponent();
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void DialogRoot_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errorCount++;
            else
                _errorCount--;
        }


        public bool HasError
        {
            get => _errorCount > 0;
        }

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
