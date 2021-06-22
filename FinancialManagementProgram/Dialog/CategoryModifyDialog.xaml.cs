using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class CategoryModifyDialog : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryModifyDialog()
            : this("", PackIconKind.HelpCircle)
        { }

        public CategoryModifyDialog(string label, PackIconKind icon)
        {
            Label = label;
            Icon = icon;
            InitializeComponent();
        }

        private void OnIconSelectionDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                PackIconKind icon = CommonUtil.GetDialog<IconSelectionDialog>(o).SelectedIcon.PackIcon;
                Icon = icon;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Icon)));
            }
        }

        public bool HasError
        {
            get => Validation.GetHasError(tbxLabel);
        }

        public string Label { get; set; }

        public PackIconKind Icon { get; set; }

        public ICommand PickIconCommand => new RelayCommand(() => CommonUtil.ShowDialog(new IconSelectionDialog(), "CategoryModifyDialogHost", OnIconSelectionDialogClosed));
    }
}
