using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public class TransactionGroup
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

        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public int TotalSpending { get; private set; } = 0;

        public int TotalIncoming { get; private set; } = 0;
    }
}
