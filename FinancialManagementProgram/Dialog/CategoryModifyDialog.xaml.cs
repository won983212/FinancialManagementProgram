using FinancialManagementProgram.Dialog.ViewModel;
using System.Windows.Controls;

namespace FinancialManagementProgram.Dialog
{
    public partial class CategoryModifyDialog : UserControl
    {
        public CategoryModifyDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Error(object sender, ValidationErrorEventArgs e)
        {
            if (DataContext != null && DataContext is CategoryModifyVM vm)
                vm.HasError = e.Action == ValidationErrorEventAction.Added;
        }
    }
}
