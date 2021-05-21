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

        public TransactionTabVM(TabContainer parent)
            : base(parent)
        {
            Transactions = CollectionViewSource.GetDefaultView(APIContext.Current.Transactions);
            Transactions.GroupDescriptions.Add(new PropertyGroupDescription("TransDate"));
        }
    }
}