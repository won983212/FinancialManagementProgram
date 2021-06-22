using FinancialManagementProgram.Data;
using FinancialManagementProgram.ViewModels.Tabs;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;

namespace FinancialManagementProgram.ViewModels
{
    class TabItem
    {
        public string Title { get; set; }
        public string IconName { get; set; }
        public TabChild ViewModel { get; set; }
    }

    class MainViewModel : TabContainer
    {
        public static SnackbarMessageQueue SnackMessageQueue { get; private set; }

        private int _selectedTabItemIndex = 0;
        public int SelectedTabItemIndex
        {
            get => _selectedTabItemIndex;
            set
            {
                _selectedTabItemIndex = value;
                CurrentPage = _tabs[_selectedTabItemIndex].ViewModel;
                OnPropertyChanged();
            }
        }

        public ICommand ExitCommand => new RelayCommand(() => App.Current.Shutdown());

        private readonly TabItem[] _tabs;
        public TabItem[] TabItems
        {
            get => _tabs;
        }

        public MainViewModel() : base(null, DataManager.Current)
        {
            _tabs = new TabItem[] // Tab이 만들어지면 여기에 추가
            {
                new TabItem(){ Title = "대시보드", IconName = "ViewDashboard", ViewModel = new DashboardTabVM(this) },
                new TabItem(){ Title = "자산 관리", IconName = "CreditCardOutline", ViewModel = new AccountManagementTabVM(this) },
                new TabItem(){ Title = "수입 및 지출", IconName = "History", ViewModel = new TransactionTabVM(this) },
                new TabItem(){ Title = "지출 분석", IconName = "ChartTimelineVariantShimmer", ViewModel = new AnalyzeTabVM(this) }
                //, new TabItem(){ Title = "설정", IconName = "Cog", ViewModel = new SettingsTabVM(this) }
            };

            SelectedTabItemIndex = 0;
        }

        // TODO Snackbar 추가해서 logger.error, warn 처리하기
        static MainViewModel()
        {
            SnackMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        }
    }
}
