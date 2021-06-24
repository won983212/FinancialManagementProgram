using FinancialManagementProgram.Data;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;

namespace FinancialManagementProgram.Dialog.ViewModel
{
    class CategoryModifyVM : DialogViewModel
    {
        public CategoryModifyVM()
               : this(TransactionCategory.RegisterCategory("새 카테고리", PackIconKind.HelpCircle))
        { }

        public CategoryModifyVM(TransactionCategory category)
        {
            Category = category;
        }

        private void OnIconSelectionDialogClosed(IconSelectionVM vm, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                Category.Icon = vm.SelectedIcon.PackIcon;
            }
        }

        public bool HasError { get; set; } = false;

        public TransactionCategory Category { get; }

        public ICommand PickIconCommand => new RelayCommand(() => CommonUtil.ShowDialog(new IconSelectionVM(), "CategoryModifyDialogHost", OnIconSelectionDialogClosed));
    }
}
