using System;
using System.IO;

namespace FinancialManagementProgram.Data
{
    public class Transaction
    {
        public Transaction(string label, long amount, string description, DateTime datetime, TransactionCategory category, BankAccount account)
        {
            Label = label;
            Category = category;
            Amount = amount;
            AccountID = account.ID;
            Description = description;
            TransDate = datetime.ToString("yyyyMMdd");
            TransDateTime = datetime;
        }

        public Transaction(BinaryReader reader)
        {
            Label = reader.ReadString();
            Category = new TransactionCategory(reader.ReadString());
            Amount = reader.ReadInt64();
            AccountID = reader.ReadInt64();
            Description = reader.ReadString();
            TransDateTime = new DateTime(reader.ReadInt64());
            TransDate = TransDateTime.ToString("yyyyMMdd");
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Label);
            writer.Write(Category.Label);
            writer.Write(Amount);
            writer.Write(AccountID);
            writer.Write(Description);
            writer.Write(TransDateTime.Ticks);
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


        public string Label { get; }
        public TransactionCategory Category { get; }
        public long Amount { get; }
        public long AccountID { get; }
        public string Description { get; }
        public string TransDate { get; }
        public DateTime TransDateTime { get; }

        public string BankName
        {
            get => DataManager.Current.GetAccount(AccountID).Label;
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
