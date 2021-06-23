using FinancialManagementProgram.Data;
using FinancialManagementProgram.Dialog;
using FinancialManagementProgram.Dialog.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class TransactionTabVM : TabChild
    {
        public TransactionTabVM(TabContainer parent)
            : base(parent)
        { }

        private void MarkProcess(TransactionModifyVM vm, Transaction t)
        {
            bool oldMarked = DataManager.HasCategoryMark(t.Label);
            TransactionCategory oldCategory = DataManager.GetDefaultCategory(t.Label);
            if (oldMarked != vm.ApplySameTypeAllTransaction || oldCategory != t.Category)
            {
                if (vm.ApplySameTypeAllTransaction)
                    DataManager.MarkAsAllCategoryAffect(t.Label, t.Category.ID);
                else
                    DataManager.UnmarkAsAllCategoryAffect(t.Label);
            }
        }

        private void OnAddDialogClosed(TransactionModifyVM vm, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                if (!vm.HasError)
                {
                    Transaction t = vm.TransactionObj;
                    DataManager.AddTransaction(t);
                    MarkProcess(vm, t);
                    BinaryProperties.Save();
                }
                else
                {
                    Logger.Error(new InvalidOperationException("빈칸을 모두 알맞게 채워주세요."));
                }
            }
        }

        private void OnEditDialogClosed(TransactionModifyVM vm, DialogClosingEventArgs e, Transaction t)
        {
            if ((bool)e.Parameter)
            {
                if (!vm.HasError)
                {
                    t.Copy(vm.TransactionObj);
                    MarkProcess(vm, t);
                    DataManager.RevalidateTransactionDatas();
                    DataManager.Analyzer.Update();
                    BinaryProperties.Save();
                }
                else
                {
                    Logger.Error(new InvalidOperationException("빈칸을 모두 알맞게 채워주세요."));
                }
            }
        }

        private void OnDeleteDialogClosed(MessageVM o, DialogClosingEventArgs e, Transaction t)
        {
            if ((bool)e.Parameter)
            {
                DataManager.DeleteTransaction(t);
                BinaryProperties.Save();
            }
        }

        public ICommand AddCommand => new RelayCommand(() => 
        {
            try
            {
                CommonUtil.ShowDialog(new TransactionModifyVM(DataManager, null), OnAddDialogClosed);
            }
            catch (InvalidOperationException e)
            {
                Logger.Error(e);
            }
        });

        public ICommand EditCommand => new RelayCommand<Transaction>((t) => 
        {
            try
            {
                CommonUtil.ShowDialog(new TransactionModifyVM(DataManager, t), (o, e) => OnEditDialogClosed(o, e, t));
            }
            catch (InvalidOperationException e)
            {
                Logger.Error(e);
            }
        });

        public ICommand DeleteCommand => new RelayCommand<Transaction>((t) => CommonUtil.ShowDialog(
            new MessageVM("자산을 삭제합니다.", "자산과 관련 거래내역이 영구적으로 삭제되며, 통계에 영향을 줄 수 있습니다. 그래도 삭제하시겠습니까?"), 
            (o, e) => OnDeleteDialogClosed(o, e, t)));
    }
}