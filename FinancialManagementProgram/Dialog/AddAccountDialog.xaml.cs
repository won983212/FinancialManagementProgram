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
    public partial class AddAccountDialog : UserControl
    {
        public AddAccountDialog()
        {
            InitializeComponent();
        }

        public string Label { get; set; }
        public string BankName { get; set; } // TODO: 은행이름 자동완성 하도록 하기
        public int ColorIndex { get; set; } = 0;
        public string Memo { get; set; }
    }
}
