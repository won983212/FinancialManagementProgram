using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace FinancialManagementProgram.Data
{
    public class Transaction : ObservableObject
    {
        private string _label;
        private TransactionCategory _category;
        private long _amount;
        private BankAccount _account;
        private string _description = "";
        private DateTime _transDateTime;


        public Transaction()
        { }

        public Transaction(DataManager dataManager, BinaryReader reader)
        {
            Label = reader.ReadString();
            Category = TransactionCategory.GetCategory(reader.ReadInt64());
            Amount = reader.ReadInt64();
            Account = dataManager.FindAccount(reader.ReadInt64());
            Description = reader.ReadString();
            TransDateTime = new DateTime(reader.ReadInt64());
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Label);
            writer.Write(Category.ID);
            writer.Write(Amount);
            writer.Write(_account.ID);
            writer.Write(Description);
            writer.Write(TransDateTime.Ticks);
        }

        /// <summary>
        /// Amount는 값이 수정되면 Analyzed Data가 망가지므로 Revalidate해야 한다.
        /// </summary>
        /// <param name="src"></param>
        public void Copy(Transaction src)
        {
            Label = src.Label;
            Category = src.Category;
            Amount = src.Amount;
            Account = src.Account;
            Description = src.Description;
            TransDateTime = src.TransDateTime;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is Transaction == false)
                return false;
            Transaction target = (Transaction)obj;
            return TransDateTime == target.TransDateTime && Label == target.Label && Amount == target.Amount;
        }

        public override int GetHashCode()
        {
            return TransDateTime.GetHashCode();
        }


        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }

        public TransactionCategory Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        public long Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public BankAccount Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string TransDate { get; set; }

        public DateTime TransDateTime
        {
            get => _transDateTime;
            set
            {
                _transDateTime = value;
                TransDate = _transDateTime.ToString("yyyyMMdd");
                OnPropertyChanged();
                OnPropertyChanged(nameof(TransDate));
            }
        }


        public string FormattedTransDate
        {
            get => string.Format("{0}.{1}.{2}", TransDate.Substring(0, 4), TransDate.Substring(4, 2), TransDate.Substring(6, 2));
        }

        public string FormattedTransDateTime
        {
            get => TransDateTime.ToString();
        }
    }
}
