using FinancialManagementProgram.Data;
using FinancialManagementProgram.Dialog;
using FinancialManagementProgram.Dialog.ViewModel;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Input;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class TransactionTabVM : TabChild
    {
        // TODO 달력 월 바꿀 수 있게 해야지;
        public TransactionTabVM(TabContainer parent)
            : base(parent)
        { }

        private void LoadTransactionCSV()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV 파일 (*.csv)|*.csv";
            if (dialog.ShowDialog() != true)
                return;

            try
            {
                using (FileStream stream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string buffer;
                    while ((buffer = reader.ReadLine()) != null)
                    {
                        if (!buffer.StartsWith("#"))
                        {
                            Transaction t = new Transaction();
                            t.DeserializeFromCSVLine(DataManager, buffer);
                            DataManager.AddTransaction(t);
                        }
                    }
                }
                DataManager.Analyzer.Update();
                BinaryProperties.Save();
                Logger.Info("거래내역 CSV 불러오기가 완료되었습니다.");
            } 
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

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
                DataManager.Analyzer.Update();
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
                    DataManager.Analyzer.Update();
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
                DataManager.Analyzer.Update();
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

        public ICommand AddCSVCommand => new RelayCommand(LoadTransactionCSV);

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