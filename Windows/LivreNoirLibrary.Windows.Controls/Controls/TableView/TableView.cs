using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class TableView : Control
    {
        static TableView()
        {
            PropertyUtils.OverrideDefaultStyleKey<TableView>();
        }

        [RoutedEvent]
        public partial event RoutedEventHandler<TableDataCell>? CellClick;

        private Grid? _mainGrid;
        private bool _needRefresh;
        private bool _isValid;

        private readonly Border _emptyBorder;
        private readonly TableDataCell _total_header;
        private readonly TableDataCell _total_value;
        private readonly CellCache _headerCells;
        private readonly CellCache _totalCells;
        private readonly CellCache _dataCells;
        private readonly CellCache _zeroCells;

        protected readonly Dictionary<int, TableDataCell> _vTotals = [];
        protected readonly Dictionary<int, TableDataCell> _hTotals = [];
        protected readonly Dictionary<int, Dictionary<int, TableDataCell>> _dataCache = [];

        public TableView()
        {
            var border = CreateCellBorder(false);
            Grid.SetColumnSpan(border, 2);
            _emptyBorder = border;

            var cell = CreateHeaderCell();
            BindTotalVisibility(cell);
            cell._textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(TotalText)) { Source = this });
            _total_header = cell;

            cell = CreateFixedDataCell();
            BindTotalVisibility(cell);
            cell._textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(ItemsCount)) { Source = this });
            _total_value = cell;

            _headerCells = new(CreateHeaderCell);
            _totalCells = new(CreateTotalCell);
            _dataCells = new(CreateDataCell);
            _zeroCells = new(CreateZeroCell);
        }

        protected void RaiseCellClick(TableDataCell context) => RaiseEvent(new RoutedEventArgs<TableDataCell>(context, CellClickEvent, this));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainGrid = GetTemplateChild("MainGrid") as Grid;
            ReserveRefresh();
        }

        protected void ReserveRefresh()
        {
            _needRefresh = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (_needRefresh)
            {
                Refresh();
                _needRefresh = false;
            }
            base.OnRender(dc);
        }

        private void UpdateDataCells()
        {
            var style = CellValueStyle;
            var digits = RatioDigits;
            var mode = RatioDigitMode;
            _totalCells.UpdateText(style, digits, mode);
            _dataCells.UpdateText(style, digits, mode);
        }

        private void Refresh()
        {
            _needRefresh = false;
            _isValid = false;
            _headerCells.Clear();
            _totalCells.Clear();
            _dataCells.Clear();
            _zeroCells.Clear();
            _dataCache.Clear();
            _vTotals.Clear();
            _hTotals.Clear();
            if (_mainGrid is not { } grid)
            {
                return;
            }
            grid.Children.Clear();
            if (ItemsSource is not { } source)
            {
                return;
            }
            var vSelector = VerticalSelector;
            var hSelector = HorizontalSelector;
            if (vSelector is not null || hSelector is not null)
            {
                vSelector ??= TableDataSelector.Default;
                hSelector ??= TableDataSelector.Default;
                RefreshData(source, vSelector, hSelector, _totalCells, _dataCells);
                CreateTable(grid, vSelector, hSelector, _headerCells, _zeroCells);
                _isValid = true;
            }
        }

        protected virtual void RefreshData(IEnumerable source, ITableDataSelector vSelector, ITableDataSelector hSelector, CellCache totals, CellCache datas)
        {
            var cache = _dataCache;
            var vTotals = _vTotals;
            var hTotals = _hTotals;
            // 分類
            var count = 0;
            foreach (var item in source)
            {
                var vKey = vSelector.GetKey(item);
                var hKey = hSelector.GetKey(item);
                AddItem(vTotals, vKey, totals, item);
                AddItem(hTotals, hKey, totals, item);
                AddItem(cache.GetOrAdd(vKey), hKey, datas, item);
                count++;
            }
            ItemsCount = count;
        }

        protected virtual void CreateTable(Grid grid, ITableDataSelector vSelector, ITableDataSelector hSelector, CellCache headers, CellCache zeroes)
        {
            var cache = _dataCache;
            var vTotals = _vTotals;
            var hTotals = _hTotals;
            var style = CellValueStyle;
            var digits = RatioDigits;
            var mode = RatioDigitMode;
            var total = ItemsCount;

            TableDataCell? cell;
            // グリッドの作成
            AddFixedElements(grid);
            // 列ヘッダー
            var col = 2;
            var hSkip = hSelector.SkipEmpty;
            foreach (var info in hSelector.EnumerateInfo())
            {
                if (hTotals.TryGetValue(info.Key, out cell))
                {
                    SetTotal(cell, 0, total);
                    cell.UpdateText(style, digits, mode);
                }
                else if (hSkip)
                {
                    continue;
                }
                else
                {
                    cell = zeroes.GetNext();
                }
                AddCell(cell, grid, 1, col);

                cell = headers.GetNext();
                SetText(cell, info.VerticalHeader ?? info.Header);
                AddCell(cell, grid, 0, col);
                col++;
            }
            // 各データ行
            var row = 2;
            foreach (var info in vSelector.EnumerateInfo())
            {
                var vKey = info.Key;
                if (vTotals.TryGetValue(vKey, out cell))
                {
                    SetTotal(cell, total, 0);
                    cell.UpdateText(style, digits, mode);
                    AddCell(cell, grid, row, 1);
                }
                else
                {
                    continue;
                }
                cell = headers.GetNext();
                SetText(cell, info.Header);
                AddCell(cell, grid, row, 0);

                col = 2;
                var colCache = cache[vKey];
                foreach (var hInfo in hSelector.EnumerateInfo())
                {
                    var hKey = hInfo.Key;
                    if (hSkip && !hTotals.ContainsKey(hKey))
                    {
                        continue;
                    }
                    if (colCache.TryGetValue(hKey, out cell))
                    {
                        SetTotal(cell, hTotals[hKey].Count, vTotals[vKey].Count);
                        cell.UpdateText(style, digits, mode);
                    }
                    else
                    {
                        cell = zeroes.GetNext();
                    }
                    AddCell(cell, grid, row, col);
                    col++;
                }

                row++;
            }
        }

        protected static void AddItem(Dictionary<int, TableDataCell> target, int key, CellCache cache, object item)
        {
            if (!target.TryGetValue(key, out var cell))
            {
                cell = cache.GetNext();
                target[key] = cell;
            }
            cell._list.Add(item);
        }

        protected static void SetText(TableDataCell cell, string text) => cell.Text = text;
        protected static void SetTotal(TableDataCell cell, int vTotal, int hTotal)
        {
            cell.VTotal = vTotal;
            cell.HTotal = hTotal;
        }

        protected void AddFixedElements(Grid grid)
        {
            grid.Children.Add(_emptyBorder);
            AddCell(_total_header, grid, 1, 0);
            AddCell(_total_value, grid, 1, 1);
        }

        protected static void AddCell(TableDataCell cell, Grid grid, int row, int col)
        {
            var rows = grid.RowDefinitions;
            while (row >= rows.Count)
            {
                rows.Add(new() { Height = GridLength.Auto });
            }
            var cols = grid.ColumnDefinitions;
            while (col >= cols.Count)
            {
                cols.Add(new() { Width = GridLength.Auto });
            }
            var element = cell._border;
            Grid.SetRow(element, row);
            Grid.SetColumn(element, col);
            grid.Children.Add(element);
        }

        private List<string?> _lineCache = [];

        public string CreateText()
        {
            if (_needRefresh)
            {
                Refresh();
            }
            if (!_isValid)
            {
                return "";
            }
            var cache = _dataCache;
            var vTotals = _vTotals;
            var hTotals = _hTotals;
            var vSelector = VerticalSelector ?? TableDataSelector.Default;
            var hSelector = HorizontalSelector ?? TableDataSelector.Default;
            var zeroText = ZeroText;
            var hSkip = hSelector.SkipEmpty;

            using var o = ObjectPool.RentStringBuilder(out var sb);
            TableDataCell? cell;

            var lines = _lineCache;
            // 左上の空白
            sb.Append("\t\t");
            // 列ヘッダー
            foreach (var info in hSelector.EnumerateInfo())
            {
                if (hTotals.TryGetValue(info.Key, out cell))
                {
                    lines.Add(cell.Text);
                }
                else if (hSkip)
                {
                    continue;
                }
                else
                {
                    lines.Add(zeroText);
                }
                sb.Append(info.Header);
                sb.Append('\t');
            }
            sb.AppendLine();
            sb.Append(TotalText);
            sb.Append('\t');
            sb.Append(ItemsCount);
            sb.Append('\t');
            sb.AppendJoin('\t', lines);
            sb.AppendLine();
            lines.Clear();
            // 行
            foreach (var info in vSelector.EnumerateInfo())
            {
                var vKey = info.Key;
                if (!vTotals.TryGetValue(vKey, out cell))
                {
                    continue;
                }
                sb.Append(info.Header);
                sb.Append('\t');
                sb.Append(cell.Text);
                sb.Append('\t');

                var colCache = cache[vKey];
                foreach (var hInfo in hSelector.EnumerateInfo())
                {
                    var hKey = hInfo.Key;
                    if (hSkip && !hTotals.ContainsKey(hKey))
                    {
                        continue;
                    }
                    if (colCache.TryGetValue(hKey, out cell))
                    {
                        sb.Append(cell.Text);
                    }
                    else
                    {
                        sb.Append(zeroText);
                    }
                    sb.Append('\t');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
