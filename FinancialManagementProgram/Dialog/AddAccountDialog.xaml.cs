using FinancialManagementProgram.Dialog.ViewModel;
using System.Windows.Controls;

namespace FinancialManagementProgram.Dialog
{
    public partial class AddAccountDialog : UserControl
    {
        public AddAccountDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Error(object sender, ValidationErrorEventArgs e)
        {
            if (DataContext != null && DataContext is AddAccountVM vm)
                vm.HasError = e.Action == ValidationErrorEventAction.Added;
        }
    }
}
