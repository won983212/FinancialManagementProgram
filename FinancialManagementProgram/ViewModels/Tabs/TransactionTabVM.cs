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
        // TODO Transaction보여줄 때 그룹은 날짜별로, 아이템은 시간별로 정렬하기.
        public TransactionTabVM(TabContainer parent)
            : base(parent)
        {
            Transactions = CollectionViewSource.GetDefaultView(APIDataManager.Current.Analyzer.MonthlyTransactions);
            Transactions.GroupDescriptions.Add(new PropertyGroupDescription("TransDate"));
        }
    }
}