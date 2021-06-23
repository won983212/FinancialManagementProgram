using FinancialManagementProgram.Data;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinancialManagementProgram.Dialog.ViewModel
{
    class CategoryManagementVM : DialogViewModel
    {
        private DataManager _dataManager;

        public CategoryManagementVM(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        private void OnModifyDialogClosed(CategoryModifyVM vm, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                if (!vm.HasError)
                    BinaryProperties.Save();
                else
                    Logger.Error(new InvalidOperationException("빈칸을 모두 올바르게 채워주세요."));
            }
        }

        private void OnDeleteDialogClosed(MessageVM vm, DialogClosingEventArgs e, long prevKey)
        {
            if ((bool)e.Parameter)
            {
                TransactionCategory.UnregisterCategory(prevKey);
                _dataManager.ReplaceAllCategory(prevKey, 0);
                BinaryProperties.Save();
            }
        }


        public ICommand AddCommand => new RelayCommand(() => CommonUtil.ShowDialog(new CategoryModifyVM(), "CategoryDialogHost", OnModifyDialogClosed));

        public ICommand EditCommand => new RelayCommand<TransactionCategory>((c) => CommonUtil.ShowDialog(new CategoryModifyVM(c), "CategoryDialogHost", OnModifyDialogClosed),
            (c) => c.ID != TransactionCategory.UnknownCategoryID);

        public ICommand DeleteCommand => new RelayCommand<TransactionCategory>((c) => CommonUtil.ShowDialog(new MessageVM("삭제", c.Label + "을(를) 삭제합니다."), "CategoryDialogHost",
            (o, e) => OnDeleteDialogClosed(o, e, c.ID)),
            (c) => c.ID != TransactionCategory.UnknownCategoryID);
    }
}
