using FinancialManagementProgram.ViewModels.Tabs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinancialManagementProgram.ViewModels
{
    class MainViewModel : TabContainer
    {
        private TabChild[] _tabs;

        public static SnackbarMessageQueue SnackMessageQueue { get; private set; }

        private int _selectedTabItemIndex = 0;
        public int SelectedTabItemIndex
        {
            get => _selectedTabItemIndex;
            set
            {
                _selectedTabItemIndex = value;
                CurrentPage = _tabs[_selectedTabItemIndex];
                OnPropertyChanged();
            }
        }

        public ICommand CloseAppCommand = new RelayCommand(() => Console.WriteLine("!!!"));

        public MainViewModel()
        {
            _tabs = new TabChild[] // Tab이 만들어지면 여기에 추가
            {
                new DashboardTabVM(this), new AccountManagementTabVM(this), new TransactionTabVM(this), new AnalyzeTabVM(this), new SettingsTabVM(this)
            };

            SelectedTabItemIndex = 0;
        }

        static MainViewModel()
        {
            SnackMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        }
    }
}
