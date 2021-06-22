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

        private void OnAddDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                TransactionModifyDialog dialog = CommonUtil.GetDialog<TransactionModifyDialog>(o);
                if (!dialog.HasError)
                    DataManager.AddTransaction(dialog.TransactionObj);
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
                    DataManager.RevalidateTransactionDatas();
                    DataManager.Analyzer.Update();
                }
            }
        }

        private void OnDeleteDialogClosed(object o, DialogClosingEventArgs e, Transaction t)
        {
            if ((bool)e.Parameter)
            {
                DataManager.DeleteTransaction(t);
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
                CommonUtil.ShowDialog(new MessageDialog("오류", e.Message), null);
            }
        });

        public ICommand EditCommand => new RelayCommand<Transaction>((t) => CommonUtil.ShowDialog(new TransactionModifyDialog(DataManager, t),
            (o, e) => OnEditDialogClosed(o, e, t)));

        public ICommand DeleteCommand => new RelayCommand<Transaction>((t) => CommonUtil.ShowDialog(
            new MessageDialog("자산을 삭제합니다.", "자산과 관련 거래내역이 영구적으로 삭제되며, 통계에 영향을 줄 수 있습니다. 그래도 삭제하시겠습니까?"), 
            (o, e) => OnDeleteDialogClosed(o, e, t)));
    }
}