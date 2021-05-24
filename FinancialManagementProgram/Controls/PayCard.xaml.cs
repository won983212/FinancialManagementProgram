using FinancialManagementProgram.kftcAPI;
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

namespace FinancialManagementProgram.Controls
{
    public partial class PayCard : UserControl
    {
        public static DependencyProperty BankAccountProperty
            = DependencyProperty.Register("BankAccount", typeof(BankAccount), typeof(PayCard));

        public BankAccount BankAccount
        {
            get => (BankAccount)GetValue(BankAccountProperty);
            set => SetValue(BankAccountProperty, value);
        }

        public PayCard()
        {
            InitializeComponent();
        }
    }
}
