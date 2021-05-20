using FinancialManagementProgram.kftcAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class TransactionTabVM : TabChild
    {
        public ICollectionView Transactions { get; private set; }
        private ObservableCollection<Transaction> _transactions = new ObservableCollection<Transaction>();

        public TransactionTabVM(TabContainer parent)
            : base(parent)
        {
            Transactions = CollectionViewSource.GetDefaultView(_transactions);
            Transactions.GroupDescriptions.Add(new PropertyGroupDescription("TransDate"));

            _transactions.Add(new Transaction()
            {
                Label = "세븐일레븐용인",
                Category = new TransactionCategory() { Label = "편의점", IconName = "CartOutline" },
                Amount = 1200,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.18",
                AfterBalanceAmount = 2993512
            });

            _transactions.Add(new Transaction()
            {
                Label = "GS용인외대",
                Category = new TransactionCategory() { Label = "편의점", IconName = "CartOutline" },
                Amount = -2800,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.18",
                AfterBalanceAmount = 2991245
            });

            _transactions.Add(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory() { Label = "게임", IconName = "GamepadVariantOutline" },
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.17",
                AfterBalanceAmount = 2881512
            });

            _transactions.Add(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory() { Label = "게임", IconName = "GamepadVariantOutline" },
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.17",
                AfterBalanceAmount = 2881512
            });

            _transactions.Add(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory() { Label = "게임", IconName = "GamepadVariantOutline" },
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.17",
                AfterBalanceAmount = 2881512
            });

            _transactions.Add(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory() { Label = "게임", IconName = "GamepadVariantOutline" },
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.18",
                AfterBalanceAmount = 2881512
            });
        }
    }
}