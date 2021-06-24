using FinancialManagementProgram.Dialog.ViewModel;
using System.Windows.Controls;

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

            if (DataContext != null && DataContext is TransactionModifyVM vm)
                vm.HasError = _errorCount > 0;
        }
    }
}
