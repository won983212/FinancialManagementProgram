using FinancialManagementProgram.Data;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class CategoryManagementDialog : UserControl
    {
        public CategoryManagementDialog()
        {
            InitializeComponent();
        }

        private void OnModifyDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                CategoryModifyDialog dialog = CommonUtil.GetDialog<CategoryModifyDialog>(o);
                if (!dialog.HasError)
                    BinaryProperties.Save();
            }
        }

        private void OnDeleteDialogClosed(object o, DialogClosingEventArgs e, long prevKey)
        {
            if ((bool)e.Parameter)
            {
                TransactionCategory.UnregisterCategory(prevKey);
                BinaryProperties.Save();
            }
        }


        public ICommand AddCommand => new RelayCommand(() => CommonUtil.ShowDialog(new CategoryModifyDialog(), "CategoryDialogHost", OnModifyDialogClosed));

        public ICommand EditCommand => new RelayCommand<TransactionCategory>((c) => CommonUtil.ShowDialog(new CategoryModifyDialog(c), "CategoryDialogHost", OnModifyDialogClosed), 
            (c) => c.ID != TransactionCategory.UnknownCategoryID);

        public ICommand DeleteCommand => new RelayCommand<TransactionCategory>((c) => CommonUtil.ShowDialog(new MessageDialog("삭제", c.Label + "을(를) 삭제합니다."), "CategoryDialogHost",
            (o, e) => OnDeleteDialogClosed(o, e, c.ID)),
            (c) => c.ID != TransactionCategory.UnknownCategoryID);
    }
}
