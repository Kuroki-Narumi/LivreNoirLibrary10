using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static LivreNoirLibrary.Windows.Media.MediaUtils;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class TableView
    {
        public static readonly SolidColorBrush DefaultBorderBrush = GetBrush(128, 128, 128, 128);
        public static readonly Thickness DefaultBorderThickness = new(0, 0, 1, 1);

        public const double DefaultSubFontSize = 12;
        public const double DefaultCellWidth = 24;
        public const double DefaultCellHeight = 24;
        public static readonly Thickness DefaultCellPadding = new(4, 2, 4, 2);

        public const VerticalAlignment DefaultVerticalCellAlignment = VerticalAlignment.Center;
        public const HorizontalAlignment DefaultHorizontalCellAlignment = HorizontalAlignment.Right;

        public const int DefaultRatioDigits = 2;
        public const TableViewDigitMode DefaultDigitMode = TableViewDigitMode.DecimalPart;

        public static readonly SolidColorBrush DefaultVerticalBackground = GetBrush(4, 0, 0, 0);
        public static readonly SolidColorBrush DefaultHorizontalBackground = GetBrush(8, 0, 0, 0);
        public static readonly SolidColorBrush DefaultCrossedBackground = GetBrush(12, 0, 0, 0);
        public static readonly SolidColorBrush DefaultSelectedBackground = GetBrush(192, 204, 238, 255);

        public const Visibility DefaultTotalVisibility = Visibility.Visible;
        public const string DefaultTotalText = "Total";
        public const string DefaultZeroText = "-";

        [DependencyProperty]
        private double _subFontSize = DefaultSubFontSize;
        [DependencyProperty]
        private double _cellWidth = DefaultCellWidth;
        [DependencyProperty]
        private double _cellHeight = DefaultCellHeight;
        [DependencyProperty]
        private Thickness _cellPadding = DefaultCellPadding;
        [DependencyProperty]
        private VerticalAlignment _verticalCellAlignment = DefaultVerticalCellAlignment;
        [DependencyProperty]
        private HorizontalAlignment _horizontalCellAlignment = DefaultHorizontalCellAlignment;
        [DependencyProperty]
        private TableViewValueStyle _cellValueStyle = TableViewValueStyle.Normal;
        [DependencyProperty]
        private int _ratioDigits = DefaultRatioDigits;
        [DependencyProperty]
        private TableViewDigitMode _ratioDigitMode = DefaultDigitMode;
        [DependencyProperty]
        private Brush? _verticalBackground = DefaultVerticalBackground;
        [DependencyProperty]
        private Brush? _horizontalBackground = DefaultHorizontalBackground;
        [DependencyProperty]
        private Brush? _crossedBackground = DefaultCrossedBackground;
        [DependencyProperty]
        private Brush? _selectedBackground = DefaultSelectedBackground;
        [DependencyProperty]
        private Visibility _totalVisibility = DefaultTotalVisibility;
        [DependencyProperty]
        private string? _totalText = DefaultTotalText;
        [DependencyProperty]
        private string? _zeroText = DefaultZeroText;
        [DependencyProperty]
        private string? _detailText;

        [DependencyProperty(SetterScope = Scope.Protected)]
        private int _itemsCount;
        [DependencyProperty]
        private IEnumerable? _itemsSource;
        [DependencyProperty]
        private ITableDataSelector? _verticalSelector;
        [DependencyProperty]
        private ITableDataSelector? _horizontalSelector;

        private void OnCellValueStyleChanged() => UpdateDataCells();
        private void OnRatioDigitModeChanged() => UpdateDataCells();
        private void OnRatioDigitsChanged() => UpdateDataCells();

        private void OnItemsSourceChanged() => ReserveRefresh();
        private void OnVerticalSelectorChanged() => ReserveRefresh();
        private void OnHorizontalSelectorChanged() => ReserveRefresh();
    }
}
