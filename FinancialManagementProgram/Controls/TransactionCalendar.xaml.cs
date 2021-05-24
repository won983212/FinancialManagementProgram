using FinancialManagementProgram.kftcAPI;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
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
    public partial class TransactionCalendar : Grid
    {
        private static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(TransactionCalendar), new PropertyMetadata(OnDateChanged));

        private static readonly Pen linePen;
        private static readonly Brush OtherMonthBrush;
        private static readonly Brush CurrentMonthBrush;
        private static readonly Brush TodayBrush;
        private static readonly Brush IncomeBrush;
        private static readonly Brush OutcomeBrush;

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }


        public TransactionCalendar()
        {
            InitializeComponent();
            SetupUI();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.OnMouseLeftButtonDown(e);
        }

        private void SetupUI()
        {
            for (int i = 0; i < 7; i++)
            {
                RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, i == 0 ? GridUnitType.Auto : GridUnitType.Star) });
                ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            // day of week label
            for (int c = 0; c < 7; c++)
            {
                TextBlock text = new TextBlock
                {
                    Text = "일월화수목금토"[c].ToString(),
                    Margin = new Thickness(0, 12, 0, 12),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                SetColumn(text, c);
                Children.Add(text);
            }

            for (int r = 1; r < 7; r++)
            {
                for (int c = 0; c < 7; c++)
                {
                    StackPanel panel = new StackPanel();

                    Border border = new Border
                    {
                        BorderBrush = TodayBrush,
                        Child = panel
                    };

                    Ripple ripple = new Ripple
                    {
                        Feedback = Brushes.Gray,
                        Content = border,
                        VerticalContentAlignment = VerticalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Stretch
                    };
                    SetColumn(ripple, c);
                    SetRow(ripple, r);
                    Children.Add(ripple);

                    // day digit
                    panel.Children.Add(new TextBlock
                    {
                        Margin = new Thickness(4),
                        Text = "x"
                    });

                    // income label
                    panel.Children.Add(new TextBlock
                    {
                        Margin = new Thickness(4, 4, 4, 2),
                        Visibility = Visibility.Collapsed,
                        FontSize = 9,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Foreground = IncomeBrush
                    });

                    // outcome label
                    panel.Children.Add(new TextBlock
                    {
                        Margin = new Thickness(4, 2, 4, 2),
                        Visibility = Visibility.Collapsed,
                        FontSize = 9,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Foreground = OutcomeBrush
                    });
                }
            }
        }

        private void UpdateCalendar(DateTime firstDate)
        {
            int offset = (int) firstDate.DayOfWeek;
            int days = CommonUtil.GetTotalDays(firstDate);
            int lastMonthDays = CommonUtil.GetTotalDays(firstDate.Year, 12 - ((13 - firstDate.Month) % 12));

            // remains before
            for (int d = 0; d < offset; d++)
            {
                TextBlock label = GetTextblockAt(7 + d, 0);
                label.Foreground = OtherMonthBrush;
                label.Text = (lastMonthDays - offset + d + 1).ToString();
            }

            // set dates
            for (int d = 0; d < days; d++)
            {
                int index = 7 + offset + d;
                DateTime date = new DateTime(firstDate.Year, firstDate.Month, d + 1);
                TransactionGroup transaction = APIDataManager.Current.GetDayTransaction(date.ToString("yyyy.MM.dd"));

                // set digit number
                TextBlock label = GetTextblockAt(index, 0);
                Border border = (Border)((Ripple)Children[index]).Content;
                if(date == DateTime.Today)
                {
                    border.BorderThickness = new Thickness(2);
                    label.Foreground = TodayBrush;
                }
                else
                {
                    border.BorderThickness = new Thickness(0);
                    label.Foreground = CurrentMonthBrush;
                }
                label.Text = (d + 1).ToString();

                if (transaction != null)
                {
                    // set income text
                    if (transaction.TotalIncoming > 0)
                    {
                        TextBlock text = GetTextblockAt(index, 1);
                        text.Visibility = Visibility.Visible;
                        text.Text = string.Format("{0:+#,##}", transaction.TotalIncoming);
                    }

                    // set spending text
                    if (transaction.TotalSpending > 0)
                    {
                        TextBlock text = GetTextblockAt(index, 2);
                        text.Visibility = Visibility.Visible;
                        text.Text = string.Format("{0:-#,##}", transaction.TotalSpending);
                    }
                }
            }

            // remains after
            for (int d = 7 + offset + days; d < 49; d++)
            {
                TextBlock label = GetTextblockAt(d, 0);
                label.Foreground = OtherMonthBrush;
                label.Text = (d - 6 - offset - days).ToString();
            }
        }

        private TextBlock GetTextblockAt(int index, int innerIndex)
        {
            return (TextBlock)((StackPanel)((Border)((Ripple)Children[index]).Content).Child).Children[innerIndex];
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // draw vertical lines
            foreach (ColumnDefinition column in ColumnDefinitions)
                dc.DrawLine(linePen, new Point(column.Offset, RowDefinitions[1].Offset), new Point(column.Offset, ActualHeight));

            // draw horizontal lines
            for (int row = 1; row < RowDefinitions.Count; row++)
                dc.DrawLine(linePen, new Point(0, (int)RowDefinitions[row].Offset), new Point(ActualWidth, (int)RowDefinitions[row].Offset));

            // draw right line
            dc.DrawLine(linePen, new Point(ActualWidth, RowDefinitions[1].Offset), new Point(ActualWidth, ActualHeight));

            // draw bottom line
            dc.DrawLine(linePen, new Point(0, ActualHeight), new Point(ActualWidth, ActualHeight));
        }

        private static void OnDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransactionCalendar obj = d as TransactionCalendar;
            obj.UpdateCalendar((DateTime)e.NewValue);
        }

        static TransactionCalendar()
        {
            linePen = new Pen((Brush)App.Current.FindResource("BorderColor"), 1);
            linePen.Freeze();

            OtherMonthBrush = linePen.Brush;
            CurrentMonthBrush = (Brush)App.Current.FindResource("HeaderTextColor");
            TodayBrush = (Brush)App.Current.FindResource("Primary");
            IncomeBrush = TodayBrush;
            OutcomeBrush = (Brush)App.Current.FindResource("ErrorColor");
        }
    }
}
