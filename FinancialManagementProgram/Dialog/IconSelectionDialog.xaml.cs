using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class IconSelectionDialog : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly Lazy<IEnumerable<IconGroup>> _preCachedPackIcons;
        private IconGroup _selectedIcon;
        private IEnumerable<IconGroup> _icons;

        public IconSelectionDialog()
        {
            InitializeComponent();
            FilterIcons(null);
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private async void FilterIcons(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                Icons = _preCachedPackIcons.Value;
            }
            else
            {
                Icons = await Task.Run(() => _preCachedPackIcons.Value
                    .Where(x => x.Aliases.Any(t => t.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) != -1))
                    .ToList());
            }
            SelectedIcon = Icons.First();
        }

        public IconGroup SelectedIcon
        {
            get => _selectedIcon;
            set
            {
                _selectedIcon = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<IconGroup> Icons
        {
            get => _icons;
            private set
            {
                _icons = value;
                OnPropertyChanged();
            }
        }


        static IconSelectionDialog()
        {
            _preCachedPackIcons = new Lazy<IEnumerable<IconGroup>>(() =>
                Enum.GetNames(typeof(PackIconKind))
                .GroupBy(k => (PackIconKind)Enum.Parse(typeof(PackIconKind), k))
                .Select(x => new IconGroup(x))
                .OrderBy(x => x.Kind)
                .ToList()
            );
        }

        private void SearchingBox_KeyDown(object sender, KeyEventArgs e)
        {
            FilterIcons(((TextBox)sender).Text);
        }
    }

    public class IconGroup
    {
        internal IconGroup(IEnumerable<string> group)
        {
            Kind = group.First();
            Aliases = group
                .OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
                .ToArray();
        }

        public PackIconKind PackIcon
        {
            get => (PackIconKind)Enum.Parse(typeof(PackIconKind), Kind);
        }

        public string Kind { get; }

        public string[] Aliases { get; }
    }
}
