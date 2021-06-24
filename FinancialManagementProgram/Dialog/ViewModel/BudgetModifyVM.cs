using FinancialManagementProgram.Data;

namespace FinancialManagementProgram.Dialog.ViewModel
{
    class BudgetModifyVM : DialogViewModel
    {
        private DataManager _dataManager;

        public BudgetModifyVM(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public string Budget
        {
            get => _dataManager.Budget.ToString();
            set => _dataManager.Budget = long.Parse(value);
        }
    }
}
