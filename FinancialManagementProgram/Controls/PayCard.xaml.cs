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

        public static DependencyProperty CardBackgroundProperty
            = DependencyProperty.Register("CardBackground", typeof(Brush), typeof(PayCard), new PropertyMetadata(App.Current.FindResource("GradientBlueColor")));

        public BankAccount BankAccount
        {
            get => (BankAccount)GetValue(BankAccountProperty);
            set => SetValue(BankAccountProperty, value);
        }

        public Brush CardBackground
        {
            get => (Brush)GetValue(CardBackgroundProperty);
            set => SetValue(CardBackgroundProperty, value);
        }

        public PayCard()
        {
            InitializeComponent();
            BankAccount = new BankAccount()
            {
                FintechUseNum = "10103812401289408",
                Label = "알뜰계좌",
                AccountAlias = "알뜰적금상품",
                AccountNum = "1002 - 356 - ******",
                BalanceAmount = 3000000,
                BankName = "우리은행",
                LastSyncDate = DateTime.Now.Ticks,
                Memo = ""
            };
        }
    }
}
