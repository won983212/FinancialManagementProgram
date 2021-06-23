using FinancialManagementProgram.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FinancialManagementProgram.Controls
{
    public class ColoredChartCategory
    {
        public Brush Fill { get; set; }

        public string CategoryName { get; set; }

        public long Amount { get; set; }
    }

    public partial class TransactionChartDataList : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CategorizedTransactionsProperty =
            DependencyProperty.Register(nameof(CategorizedTransaction),
                typeof(IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>>), typeof(TransactionChartDataList),
                new PropertyMetadata(OnCategorizedTransactionsChanged));

        public static readonly DependencyProperty ChartDatasProperty =
            DependencyProperty.Register(nameof(ChartDatas), typeof(IEnumerable<ColoredChartCategory>), typeof(TransactionChartDataList));

        // TODO Color Addition
        private static Brush[] _categoryFills = new Brush[]
        {
            Brushes.Tomato, Brushes.LightSeaGreen, Brushes.DodgerBlue, Brushes.Coral, Brushes.Cornsilk, Brushes.Khaki, Brushes.LightPink
        };

        private ICollectionView _categorizedCollectionView;
        private List<ColoredChartCategory> _categorizedTotal = new List<ColoredChartCategory>();
        public event PropertyChangedEventHandler PropertyChanged;


        public TransactionChartDataList()
        {
            InitializeComponent();
        }

        private static void OnCategorizedTransactionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransactionChartDataList obj = d as TransactionChartDataList;
            var value = e.NewValue as IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>>;
            if (value != null)
                obj.BuildCategoryEntites(value);
        }

        private void BuildCategoryEntites(IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>> data)
        {
            _categorizedTotal = new List<ColoredChartCategory>();
            int i = 0;
            foreach (var ent in data)
            {
                _categorizedTotal.Add(new ColoredChartCategory()
                {
                    CategoryName = ent.Key.Label,
                    Amount = ent.Value.TotalSpending,
                    Fill = _categoryFills[i++ % _categoryFills.Length]
                });
            }

            ChartDatas = _categorizedTotal;
            ChartCategorizedCollectionView = CollectionViewSource.GetDefaultView(_categorizedTotal);
            ChartCategorizedCollectionView.SortDescriptions.Add(new SortDescription(nameof(ColoredChartCategory.Amount), ListSortDirection.Descending));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>> CategorizedTransaction
        {
            get => (IEnumerable<KeyValuePair<TransactionCategory, TransactionGroup>>)GetValue(CategorizedTransactionsProperty);
            set => SetValue(CategorizedTransactionsProperty, value);
        }

        public ICollectionView ChartCategorizedCollectionView
        {
            get => _categorizedCollectionView;
            set
            {
                _categorizedCollectionView = value;
                OnPropertyChanged(nameof(ChartCategorizedCollectionView));
                OnPropertyChanged(nameof(ChartDatas));
            }
        }

        public IEnumerable<ColoredChartCategory> ChartDatas
        {
            get => (IEnumerable<ColoredChartCategory>)GetValue(ChartDatasProperty);
            set => SetValue(ChartDatasProperty, value);
        }
    }
}
