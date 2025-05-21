using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static LivreNoirLibrary.Media.MediaUtils;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class TableView
    {
        public static readonly SolidColorBrush DefaultBorderBrush = GetBrush(128, 128, 128, 128);
        public static readonly Thickness DefaultBorderThickness = new(0, 0, 1, 1);

        public const double DefaultMainFontSize = 16;
        public const double DefaultSubFontSize = 12;
        public const double DefaultCellWidth = 24;
        public const double DefaultCellHeight = 24;
        public static readonly Thickness DefaultCellMargin = new(4, 2, 4, 2);
        public const VerticalAlignment DefaultVerticalCellAlignment = VerticalAlignment.Center;
        public const HorizontalAlignment DefaultHorizontalCellAlignment = HorizontalAlignment.Right;
        public const bool DefaultRatioFixed = true;
        public const int DefaultRatioDigits = 2;

        public static readonly SolidColorBrush DefaultVerticalBackground = GetBrush(4, 0, 0, 0);
        public static readonly SolidColorBrush DefaultHorizontalBackground = GetBrush(8, 0, 0, 0);
        public static readonly SolidColorBrush DefaultCrossedBackground = GetBrush(12, 0, 0, 0);
        public static readonly SolidColorBrush DefaultSelectedBackground = GetBrush(192, 204, 238, 255);

        public const bool DefaultIsTotalVisible = true;
        public const string DefaultTotalText = "Total";
        public const string DefaultZeroText = "-";
        public const string DefaultDetailText = "クリックで詳細";

        public static readonly DependencyProperty BorderBrushProperty = Border.BorderBrushProperty.AddOwner(typeof(TableView), DefaultBorderBrush);
        public static readonly DependencyProperty BorderThicknessProperty = Border.BorderThicknessProperty.AddOwner(typeof(TableView), DefaultBorderThickness);

        [DependencyProperty]
        private double _mainFontSize = DefaultMainFontSize;
        [DependencyProperty]
        private double _subFontSize = DefaultSubFontSize;
        [DependencyProperty]
        private double _cellWidth = DefaultCellWidth;
        [DependencyProperty]
        private double _cellHeight = DefaultCellHeight;
        [DependencyProperty]
        private Thickness _cellMargin = DefaultCellMargin;
        [DependencyProperty]
        private TableViewCellStyle _cellStyle = TableViewCellStyle.Normal;
        [DependencyProperty]
        private VerticalAlignment _verticalCellAlignment = DefaultVerticalCellAlignment;
        [DependencyProperty]
        private HorizontalAlignment _horizontalCellAlignment = DefaultHorizontalCellAlignment;
        [DependencyProperty]
        private bool _ratioFixed = DefaultRatioFixed;
        [DependencyProperty]
        private int _ratioDigits = DefaultRatioDigits;
        [DependencyProperty]
        private Brush? _verticalBackground = DefaultVerticalBackground;
        [DependencyProperty]
        private Brush? _horizontalBackground = DefaultHorizontalBackground;
        [DependencyProperty]
        private Brush? _crossedBackground = DefaultCrossedBackground;
        [DependencyProperty]
        private Brush? _selectedBackground = DefaultSelectedBackground;
        [DependencyProperty]
        private bool _isTotalVisible = DefaultIsTotalVisible;
        [DependencyProperty]
        private string? _totalText = DefaultTotalText;
        [DependencyProperty]
        private string? _zeroText = DefaultZeroText;
        [DependencyProperty]
        private string? _detailText = DefaultDetailText;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Protected)]
        private int _count;
        [DependencyProperty]
        private IEnumerable? _source;
        [DependencyProperty]
        private TableDataSelector? _verticalSelector;
        [DependencyProperty]
        private TableDataSelector? _horizontalSelector;

        public Brush? BorderBrush { get => GetValue(BorderBrushProperty) as Brush; set => SetValue(BorderBrushProperty, value); }
        public Thickness BorderThickness { get => (Thickness)GetValue(BorderThicknessProperty); set => SetValue(BorderThicknessProperty, value); }
        public FontFamily FontFamily { get => TextBlock.GetFontFamily(this); set => TextBlock.SetFontFamily(this, value); }
        public FontStyle FontStyle { get => TextBlock.GetFontStyle(this); set => TextBlock.SetFontStyle(this, value); }
        public FontStretch FontStretch { get => TextBlock.GetFontStretch(this); set => TextBlock.SetFontStretch(this, value); }
        public FontWeight FontWeight { get => TextBlock.GetFontWeight(this); set => TextBlock.SetFontWeight(this, value); }
        public Brush Foreground { get => TextBlock.GetForeground(this); set => TextBlock.SetForeground(this, value); }

        private void OnCellStyleChanged(TableViewCellStyle value) => UpdateDataCells();
        private void OnRatioFixedChanged(bool value) => UpdateDataCells();
        private void OnRatioDigitsChanged(int value) => UpdateDataCells();
        private void OnSourceChanged(IEnumerable? value) => ReserveRefresh();
        private void OnVerticalSelectorChanged(TableDataSelector? value) => ReserveRefresh();
        private void OnHorizontalSelectorChanged(TableDataSelector? value) => ReserveRefresh();
    }
}
