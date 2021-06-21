using System.Collections.Generic;
using System.IO;

namespace FinancialManagementProgram.Data
{
    public class TransactionGroup : IPropertiesSerializable
    {
        internal void AddTransaction(Transaction transaction)
        {
            if (transaction.Amount < 0)
                TotalSpending -= transaction.Amount;
            else
                TotalIncoming += transaction.Amount;
            Transactions.Add(transaction);
        }

        internal void AddTransactionGroup(TransactionGroup group)
        {
            TotalSpending += group.TotalSpending;
            TotalIncoming += group.TotalIncoming;
            Transactions.AddRange(group.Transactions);
        }

        internal void ClearTransactions()
        {
            Transactions.Clear();
            TotalIncoming = 0;
            TotalSpending = 0;
        }

        public void Deserialize(BinaryReader reader)
        {
            ClearTransactions();
            int len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
                AddTransaction(new Transaction(reader));
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Transactions.Count);
            foreach (Transaction t in Transactions)
                t.Serialize(writer);
        }

        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public long TotalSpending { get; private set; } = 0;

        public long TotalIncoming { get; private set; } = 0;
    }
}
