using FinancialManagementProgram.Data;
using FinancialManagementProgram.Dialog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
