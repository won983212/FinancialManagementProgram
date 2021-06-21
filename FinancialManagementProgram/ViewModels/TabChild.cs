using FinancialManagementProgram.Data;

namespace FinancialManagementProgram.ViewModels
{
    class TabChild : ObservableObject
    {
        public readonly TabContainer Parent;
        public DataManager DataManager { get; private set; }

        public TabChild(TabContainer parent) : this(parent, parent.DataManager)
        { }

        public TabChild(TabContainer parent, DataManager dataManager)
        {
            Parent = parent;
            DataManager = dataManager;
        }
    }
}
