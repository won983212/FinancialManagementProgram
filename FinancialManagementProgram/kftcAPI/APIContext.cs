using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public class DayTransaction
    {
        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public int TotalSpending = 0;

        public int TotalIncoming = 0;
    }

    public class APIContext : ObservableObject
    {
        private static readonly Lazy<APIContext> _instance = new Lazy<APIContext>(() => new APIContext());

        private Dictionary<string, DayTransaction> _dayTransactions = new Dictionary<string, DayTransaction>();
        private DateTime _targetDate;
        private int _totalSpending = 0;
        private int _totalIncoming = 0;
        private int _budget = 100000;


        public APIContext()
        {
            TargetDate = DateTime.Now;

            // TODO Test Action
            Transactions.Add(new Transaction()
            {
                Label = "세븐일레븐용인",
                Category = new TransactionCategory() { Label = "편의점", IconName = "CartOutline" },
                Amount = 1200,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.18",
                AfterBalanceAmount = 2993512
            });

            Transactions.Add(new Transaction()
            {
                Label = "GS용인외대",
                Category = new TransactionCategory() { Label = "편의점", IconName = "CartOutline" },
                Amount = -2800,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.18",
                AfterBalanceAmount = 2991245
            });

            Transactions.Add(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory() { Label = "게임", IconName = "GamepadVariantOutline" },
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.17",
                AfterBalanceAmount = 2881512
            });

            Transactions.Add(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory() { Label = "게임", IconName = "GamepadVariantOutline" },
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.17",
                AfterBalanceAmount = 2881512
            });

            Transactions.Add(new Transaction()
            {
                Label = "바이브1PC",
                Category = new TransactionCategory() { Label = "게임", IconName = "GamepadVariantOutline" },
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.17",
                AfterBalanceAmount = 2881512
            });

            Transactions.Add(new Transaction()
            {
                Label = "바이브PC",
                Category = new TransactionCategory() { Label = "게임", IconName = "GamepadVariantOutline" },
                Amount = -6000,
                BankName = "우리은행체크",
                Description = "결제 승인",
                TransDate = "2021.05.18",
                AfterBalanceAmount = 2881512
            });

            AnalyzeTransaction();
        }

        private void AnalyzeTransaction()
        {
            int spend = 0;
            int income = 0;
            _dayTransactions.Clear();

            foreach (Transaction ent in Transactions)
            {
                DayTransaction dayTrans;
                if (!_dayTransactions.TryGetValue(ent.TransDate, out dayTrans))
                    _dayTransactions.Add(ent.TransDate, dayTrans = new DayTransaction());

                dayTrans.Transactions.Add(ent);
                if (ent.Amount < 0)
                {
                    dayTrans.TotalSpending -= ent.Amount;
                    spend -= ent.Amount;
                }
                else
                {
                    dayTrans.TotalIncoming += ent.Amount;
                    income += ent.Amount;
                }
            }

            TotalSpending = spend;
            TotalIncoming = income;
        }

        /// <summary>
        /// date format must be yyyy.mm.dd
        /// </summary>
        public DayTransaction GetDayTransaction(string date)
        {
            DayTransaction result;
            if (_dayTransactions.TryGetValue(date, out result))
                return result;
            return null;
        }

        public ObservableCollection<Transaction> Transactions { get; } = new ObservableCollection<Transaction>();

        public int TotalSpending
        {
            get => _totalSpending;
            private set { _totalSpending = value; OnPropertyChanged(); }
        }

        public int TotalIncoming
        {
            get => _totalIncoming;
            private set { _totalIncoming = value; OnPropertyChanged(); }
        }
        
        public int Budget
        {
            get => _budget;
            private set { _budget = value; OnPropertyChanged(); }
        }

        public DateTime TargetDate
        {
            get => _targetDate;
            private set { _targetDate = value; OnPropertyChanged(); }
        }

        public static APIContext Current
        {
            get => _instance.Value; 
        }
    }
}
