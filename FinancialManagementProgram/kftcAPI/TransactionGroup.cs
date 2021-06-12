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

        internal void ClearTransactions()
        {
            Transactions.Clear();
            TotalIncoming = 0;
            TotalSpending = 0;
        }

        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public long TotalSpending { get; private set; } = 0;

        public long TotalIncoming { get; private set; } = 0;
    }
}
