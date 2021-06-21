using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FinancialManagementProgram.Controls
{
    public partial class DashboardCard : UserControl
    {
        private static DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DashboardCard));

        private static DependencyProperty ValueTextProperty =
            DependencyProperty.Register("ValueText", typeof(string), typeof(DashboardCard));

        private static DependencyProperty IconKindProperty =
            DependencyProperty.Register("IconKind", typeof(string), typeof(DashboardCard));

        private static DependencyProperty IconBackgroundBrushProperty =
            DependencyProperty.Register("IconBackgroundBrush", typeof(Brush), typeof(DashboardCard));

        private static DependencyProperty SubcontentProperty =
            DependencyProperty.Register("SubcontentText", typeof(string), typeof(DashboardCard));

        private static DependencyProperty SubIconKindProperty =
            DependencyProperty.Register("SubIconKind", typeof(string), typeof(DashboardCard));

        public DashboardCard()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string ValueText
        {
            get => (string)GetValue(ValueTextProperty);
            set => SetValue(ValueTextProperty, value);
        }

        public Brush IconBackgroundBrush
        {
            get => (Brush)GetValue(IconBackgroundBrushProperty);
            set => SetValue(IconBackgroundBrushProperty, value);
        }

        public string IconKind
        {
            get => (string)GetValue(IconKindProperty);
            set => SetValue(IconKindProperty, value);
        }

        public string SubcontentText
        {
            get => (string)GetValue(SubcontentProperty);
            set => SetValue(SubcontentProperty, value);
        }

        public string SubIconKind
        {
            get => (string)GetValue(SubIconKindProperty);
            set => SetValue(SubIconKindProperty, value);
        }
    }
}
