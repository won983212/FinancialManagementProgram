using FinancialManagementProgram.kftcAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FinancialManagementProgram.Controls
{
    public class ColoredChartCategory
    {
        public Brush Fill { get; set; }

        public string CategoryName { get; set; }

        public int Amount { get; set; }
    }

    public partial class TransactionChart : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CategorizedTransactionsProperty =
            DependencyProperty.Register("CategorizedTransaction",
                typeof(IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>>), typeof(TransactionChart),
                new PropertyMetadata(OnCategorizedTransactionsChanged));

        private ICollectionView _chartDatas;
        private ObservableCollection<ColoredChartCategory> _categorizedTotal = new ObservableCollection<ColoredChartCategory>();
        public event PropertyChangedEventHandler PropertyChanged;

        public TransactionChart()
        {
            InitializeComponent();
        }

        private static Point CreateStraightPoint(double len, double angle, double offset)
        {
            return new Point
            {
                X = len * Math.Cos(angle) + offset,
                Y = len * Math.Sin(angle) + offset
            };
        }

        private void UpdatePieChart()
        {
            const double PieProportion = 0.5;
            double radius = canvasChart.Width / 2;

            int total = 0;
            foreach (var ent in _categorizedTotal)
                total += ent.Amount;

            double angle = 0;
            foreach (var ent in _categorizedTotal)
            {
                Point from1 = CreateStraightPoint(radius * PieProportion, angle, radius);
                Point from2 = CreateStraightPoint(radius, angle, radius);

                double proportion = (double)ent.Amount / total;
                angle += proportion * 2 * Math.PI;

                if (proportion == 1)
                    angle -= 0.000001;

                Point to1 = CreateStraightPoint(radius * PieProportion, angle, radius);
                Point to2 = CreateStraightPoint(radius, angle, radius);

                PathFigure pathFigure = new PathFigure(from1, new List<PathSegment>() {
                    new LineSegment(from2, false),
                    new ArcSegment()
                    {
                        Size = new Size(radius, radius),
                        Point = to2,
                        SweepDirection = SweepDirection.Clockwise,
                        IsLargeArc = proportion > 0.5
                    },
                    new LineSegment(to1, false),
                    new ArcSegment()
                    {
                        Size = new Size(radius * PieProportion, radius * PieProportion),
                        Point = from1,
                        SweepDirection = SweepDirection.Counterclockwise,
                        IsLargeArc = proportion > 0.5
                    }
                }, true);

                Path path = new Path()
                {
                    Fill = ent.Fill,
                    Data = new PathGeometry(new List<PathFigure>() { pathFigure })
                };

                ToolTipService.SetInitialShowDelay(path, 0);
                ToolTipService.SetPlacement(path, System.Windows.Controls.Primitives.PlacementMode.Mouse);
                path.ToolTip = ent.CategoryName;

                canvasChart.Children.Add(path);
            }
        }

        private void BuildCategoryEntites(IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>> data)
        {
            _categorizedTotal.Clear();
            int i = 0;
            foreach (var ent in data)
            {
                _categorizedTotal.Add(new ColoredChartCategory() 
                { 
                    CategoryName = ent.Key.Label, 
                    Amount = ent.Value.TotalSpending, 
                    Fill = i++ == 0 ? Brushes.Tomato : Brushes.Green
                });
            }

            ChartDatas = CollectionViewSource.GetDefaultView(_categorizedTotal);
            ChartDatas.SortDescriptions.Add(new SortDescription("Amount", ListSortDirection.Descending));
        }

        private static void OnCategorizedTransactionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransactionChart obj = d as TransactionChart;
            var value = e.NewValue as IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>>;
            obj.BuildCategoryEntites(value);
            obj.UpdatePieChart();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ICollectionView ChartDatas
        { 
            get => _chartDatas;
            private set { _chartDatas = value; OnPropertyChanged(); }
        }

        public IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>> CategorizedTransaction
        {
            get => (IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>>)GetValue(CategorizedTransactionsProperty);
            set => SetValue(CategorizedTransactionsProperty, value);
        }
    }
}
