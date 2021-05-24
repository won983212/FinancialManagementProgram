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
    public partial class TransactionChart : UserControl
    {
        public static readonly DependencyProperty ChartDatasProperty =
            DependencyProperty.Register(nameof(ChartDatas), typeof(IEnumerable<ColoredChartCategory>), typeof(TransactionChart),
                new PropertyMetadata(OnChartDatasChanged));

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

        private void UpdatePieChart(IEnumerable<ColoredChartCategory> datas)
        {
            const double PieProportion = 0.5;
            double radius = canvasChart.Width / 2;

            int total = 0;
            foreach (var ent in datas)
                total += ent.Amount;

            double angle = 0;
            foreach (var ent in datas)
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

        private static void OnChartDatasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("?");
            ((TransactionChart)d).UpdatePieChart((IEnumerable<ColoredChartCategory>)e.NewValue);
        }

        public IEnumerable<ColoredChartCategory> ChartDatas
        {
            get => (IEnumerable<ColoredChartCategory>)GetValue(ChartDatasProperty);
            set => SetValue(ChartDatasProperty, value);
        }
    }
}
