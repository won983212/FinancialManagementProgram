using FinancialManagementProgram.Data;

namespace FinancialManagementProgram.ViewModels
{
    class TabContainer : TabChild
    {
        private TabChild _currentPage;

        public TabChild CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public TabContainer(TabContainer parent)
            : base(parent)
        { }

        public TabContainer(TabContainer parent, DataManager dataManager)
            : base(parent, dataManager)
        { }
    }
}
