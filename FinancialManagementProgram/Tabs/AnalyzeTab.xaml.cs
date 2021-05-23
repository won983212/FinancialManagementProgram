using FinancialManagementProgram.kftcAPI;
using FinancialManagementProgram.ViewModels.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinancialManagementProgram.Tabs
{
    public partial class AnalyzeTab : UserControl
    {
        public AnalyzeTab()
        {
            InitializeComponent();
        }

        // event 추가 제거 문제는 mvvm 범위 내에서 해결할 경우 코드가 난잡해짐.
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            APIDataAnalyzer.Current.PropertyChanged += Current_PropertyChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            APIDataAnalyzer.Current.PropertyChanged -= Current_PropertyChanged;
        }

        private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AnalyzeTabVM vm = DataContext as AnalyzeTabVM;
            if (vm != null)
                vm.OnDataHandlerPropertyChanged(e);
        }
    }
}
