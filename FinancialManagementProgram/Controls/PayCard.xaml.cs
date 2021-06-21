using FinancialManagementProgram.Data;
using System.Windows;
using System.Windows.Controls;

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
