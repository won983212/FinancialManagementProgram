using FinancialManagementProgram.Data;
using FinancialManagementProgram.Dialog;
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

        private void MarkProcess(TransactionModifyDialog dialog, Transaction t)
        {
            bool oldMarked = DataManager.HasCategoryMark(t.Label);
            TransactionCategory oldCategory = DataManager.GetDefaultCategory(t.Label);
            if (oldMarked != dialog.ApplySameTypeAllTransaction || oldCategory != t.Category)
            {
                if (dialog.ApplySameTypeAllTransaction)
                    DataManager.MarkAsAllCategoryAffect(t.Label, t.Category.ID);
                else
                    DataManager.UnmarkAsAllCategoryAffect(t.Label);
            }
        }

        private void OnAddDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                TransactionModifyDialog dialog = CommonUtil.GetDialog<TransactionModifyDialog>(o);
                if (!dialog.HasError)
                {
                    Transaction t = dialog.TransactionObj;
                    DataManager.AddTransaction(t);
                    MarkProcess(dialog, t);
                    BinaryProperties.Save();
                }
            }
        }

        private void OnEditDialogClosed(object o, DialogClosingEventArgs e, Transaction t)
        {
            if ((bool)e.Parameter)
            {
                TransactionModifyDialog dialog = CommonUtil.GetDialog<TransactionModifyDialog>(o);
                if (!dialog.HasError)
                {
                    t.Copy(dialog.TransactionObj);
                    MarkProcess(dialog, t);
                    DataManager.RevalidateTransactionDatas();
                    DataManager.Analyzer.Update();
                    BinaryProperties.Save();
                }
            }
        }

        private void OnDeleteDialogClosed(object o, DialogClosingEventArgs e, Transaction t)
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
                CommonUtil.ShowDialog(new TransactionModifyDialog(DataManager, null), OnAddDialogClosed);
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
                CommonUtil.ShowDialog(new TransactionModifyDialog(DataManager, t), (o, e) => OnEditDialogClosed(o, e, t));
            }
            catch (InvalidOperationException e)
            {
                Logger.Error(e);
            }
        });

        public ICommand DeleteCommand => new RelayCommand<Transaction>((t) => CommonUtil.ShowDialog(
            new MessageDialog("자산을 삭제합니다.", "자산과 관련 거래내역이 영구적으로 삭제되며, 통계에 영향을 줄 수 있습니다. 그래도 삭제하시겠습니까?"), 
            (o, e) => OnDeleteDialogClosed(o, e, t)));
    }
}