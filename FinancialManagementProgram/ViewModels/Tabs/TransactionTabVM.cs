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

        // TODO 아마 변경사항이 적용안될텐데 테스트해봐
        public TransactionTabVM(TabContainer parent)
            : base(parent)
        {
            Transactions = CollectionViewSource.GetDefaultView(APIDataAnalyzer.Current.Transactions);
            Transactions.GroupDescriptions.Add(new PropertyGroupDescription("TransDate"));
        }
    }
}