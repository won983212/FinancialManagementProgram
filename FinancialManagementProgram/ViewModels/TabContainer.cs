using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public TabContainer()
            : this(null)
        { }

        public TabContainer(TabContainer parent)
            : base(parent)
        { }
    }
}
