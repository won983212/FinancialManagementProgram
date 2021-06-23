using FinancialManagementProgram.Data;
using FinancialManagementProgram.Dialog.ViewModel;
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
    public partial class TransactionModifyDialog : UserControl
    {
        private int _errorCount = 0;

        public TransactionModifyDialog()
        {
            InitializeComponent();
        }

        private void DialogRoot_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errorCount++;
            else
                _errorCount--;

            if(DataContext != null && DataContext is TransactionModifyVM vm)
                vm.HasError = _errorCount > 0;
        }
    }
}
