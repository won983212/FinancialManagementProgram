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
            RebuildCategoryList();
            InitializeComponent();
        }

        private void RebuildCategoryList()
        {
            CategoryViewList.Clear();
            foreach (var ent in TransactionCategory.CategoryMap)
                CategoryViewList.Add(ent);
        }

        private void OnModifyDialogClosed(object o, DialogClosingEventArgs e, string prevKey)
        {
            if ((bool)e.Parameter)
            {
                CategoryModifyDialog dialog = CommonUtil.GetDialog<CategoryModifyDialog>(o);
                if (!dialog.HasError)
                {
                    if(prevKey != null)
                        TransactionCategory.CategoryMap.Remove(prevKey);
                    TransactionCategory.CategoryMap[dialog.Label] = dialog.Icon;
                    RebuildCategoryList();
                    BinaryProperties.Save();
                }
            }
        }

        private void OnDeleteDialogClosed(object o, DialogClosingEventArgs e, string prevKey)
        {
            if ((bool)e.Parameter)
            {
                TransactionCategory.CategoryMap.Remove(prevKey);
                RebuildCategoryList();
                BinaryProperties.Save();
            }
        }


        public ObservableCollection<KeyValuePair<string, PackIconKind>> CategoryViewList { get; } = new ObservableCollection<KeyValuePair<string, PackIconKind>>();

        public ICommand AddCommand => new RelayCommand(() => CommonUtil.ShowDialog(new CategoryModifyDialog(), "CategoryDialogHost", 
            (o, e) => OnModifyDialogClosed(o, e, null)));

        public ICommand EditCommand => new RelayCommand<KeyValuePair<string, PackIconKind>>((c) => CommonUtil.ShowDialog(new CategoryModifyDialog(c.Key, c.Value), "CategoryDialogHost",
            (o, e) => OnModifyDialogClosed(o, e, c.Key)), 
            (c) => c.Key != TransactionCategory.UnknownCategoryLabel);

        public ICommand DeleteCommand => new RelayCommand<KeyValuePair<string, PackIconKind>>((c) => CommonUtil.ShowDialog(new MessageDialog("삭제", c.Key + "을(를) 삭제합니다."), "CategoryDialogHost",
            (o, e) => OnDeleteDialogClosed(o, e, c.Key)),
            (c) => c.Key != TransactionCategory.UnknownCategoryLabel);
    }
}
