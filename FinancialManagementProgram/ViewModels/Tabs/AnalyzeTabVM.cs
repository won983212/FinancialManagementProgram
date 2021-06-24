using FinancialManagementProgram.Dialog.ViewModel;
using System.Windows.Input;

namespace FinancialManagementProgram.ViewModels.Tabs
{
    class AnalyzeTabVM : TabChild
    {
        public AnalyzeTabVM(TabContainer parent)
            : base(parent)
        { }

        public ICommand EditBudgetCommand => new RelayCommand(() => CommonUtil.ShowDialog(new BudgetModifyVM(DataManager), null));

        public ICommand EditCategoryCommand => new RelayCommand(() => CommonUtil.ShowDialog(new CategoryManagementVM(DataManager), null));

        public ICommand PrevMonthCommand => new RelayCommand(() => DataManager.TargetDate = DataManager.TargetDate.AddMonths(-1));

        public ICommand NextMonthCommand => new RelayCommand(() => DataManager.TargetDate = DataManager.TargetDate.AddMonths(1));
    }
}
