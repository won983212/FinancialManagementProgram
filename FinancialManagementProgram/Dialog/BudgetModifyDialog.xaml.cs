using FinancialManagementProgram.Data;
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

namespace FinancialManagementProgram.Dialog
{
    public partial class BudgetModifyDialog : UserControl
    {
        private DataManager _dataManager;

        public BudgetModifyDialog(DataManager dataManager)
        {
            _dataManager = dataManager;
            InitializeComponent();
        }

        public string Budget
        {
            get => _dataManager.Budget.ToString();
            set => _dataManager.Budget = long.Parse(value);
        }
    }
}
