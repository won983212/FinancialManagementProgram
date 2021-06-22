using FinancialManagementProgram.Data;
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
            : this(TransactionCategory.RegisterCategory("새 카테고리", PackIconKind.HelpCircle))
        { }

        public CategoryModifyDialog(TransactionCategory category)
        {
            Category = category;
            InitializeComponent();
        }

        private void OnIconSelectionDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                Category.Icon = CommonUtil.GetDialog<IconSelectionDialog>(o).SelectedIcon.PackIcon;
            }
        }

        public bool HasError
        {
            get => Validation.GetHasError(tbxLabel);
        }

        public TransactionCategory Category { get; }

        public ICommand PickIconCommand => new RelayCommand(() => CommonUtil.ShowDialog(new IconSelectionDialog(), "CategoryModifyDialogHost", OnIconSelectionDialogClosed));
    }
}
