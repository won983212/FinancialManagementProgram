using FinancialManagementProgram.Data;
using FinancialManagementProgram.Dialog.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
