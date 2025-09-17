using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    using List = List<object>;
    using GroupDictionary = Dictionary<int, List<object>>;

    public delegate void CellClickedEventHandler(object sender, TableDataCell context);

    public partial class TableView : Grid
    {
        public event CellClickedEventHandler? CellClicked;

        protected readonly List<TableDataCell> _cells = [];
        protected bool _need_refresh;

        protected void ReserveRefresh()
        {
            _need_refresh = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (_need_refresh)
            {
                CreateTable();
                _need_refresh = false;
            }
            base.OnRender(dc);
        }

        protected TextBlock CreateTextBlock(bool main)
        {
            TextBlock t = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            t.SetBinding(MarginProperty, new Binding(nameof(CellMargin)) { Source = this });
            t.SetBinding(TextBlock.FontSizeProperty, new Binding(main ? nameof(MainFontSize) : nameof(SubFontSize)) { Source = this });
            return t;
        }

        protected TextBlock CreateTextBlock_Cell(bool main)
        {
            var t = CreateTextBlock(main);
            t.SetBinding(VerticalAlignmentProperty, new Binding(nameof(VerticalCellAlignment)) { Source = this });
            t.SetBinding(HorizontalAlignmentProperty, new Binding(nameof(HorizontalCellAlignment)) { Source = this });
            return t;
        }

        protected Border CreateBorder(UIElement? child = null)
        {
            Border b = new()
            {
                Child = child,
            };
            b.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { Source = this });
            b.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { Source = this });
            b.SetBinding(MinWidthProperty, new Binding(nameof(CellWidth)) { Source = this });
            b.SetBinding(MinHeightProperty, new Binding(nameof(CellHeight)) { Source = this });
            return b;
        }

        protected void SetBackground(Border border, int row, int col)
        {
            string propName;
            if (row % 2 == 1)
            {
                if (col % 2 == 1)
                {
                    propName = nameof(CrossedBackground);
                }
                else
                {
                    propName = nameof(HorizontalBackground);
                }
            }
            else if (col % 2 == 1)
            {
                propName = nameof(VerticalBackground);
            }
            else
            {
                border.Background = Brushes.Transparent;
                return;
            }
            border.SetBinding(Border.BackgroundProperty, new Binding(propName) { Source = this });
        }

        protected void AddCell_General(int row, int col, UIElement element)
        {
            var rows = RowDefinitions;
            while (rows.Count <= row)
            {
                rows.Add(new() { Height = GridLength.Auto });
            }
            var cols = ColumnDefinitions;
            while (cols.Count <= col)
            {
                cols.Add(new() { Width = GridLength.Auto });
            }
            SetRow(element, row);
            SetColumn(element, col);
            Children.Add(element);
        }

        protected void AddEmptyCell(int row, int col, bool isTotal = false)
        {
            var b = CreateBorder();
            if (isTotal)
            {
                b.SetBinding(VisibilityProperty, new Binding(nameof(IsTotalVisible)) { Source = this, Converter = new BooleanToVisibilityConverter() });
            }
            AddCell_General(row, col, b);
        }

        protected void AddCell_WithBackground(int row, int col, Border element, bool isTotal)
        {
            SetBackground(element, row, col);
            if (isTotal)
            {
                element.SetBinding(VisibilityProperty, new Binding(nameof(IsTotalVisible)) { Source = this, Converter = new BooleanToVisibilityConverter() });
            }
            AddCell_General(row, col, element);
        }

        protected void AddTotalCells(int count)
        {
            var t = CreateTextBlock(false);
            t.SetBinding(TextBlock.TextProperty, new Binding(nameof(TotalText)) { Source = this });
            AddCell_WithBackground(1, 0, CreateBorder(t), true);

            t = CreateTextBlock_Cell(false);
            t.Text = count.ToString();
            AddCell_WithBackground(1, 1, CreateBorder(t), true);
        }

        protected void AddZeroCell(int row, int col, bool isTotal = false)
        {
            var t = CreateTextBlock_Cell(false);
            t.SetBinding(TextBlock.TextProperty, new Binding(nameof(ZeroText)) { Source = this });
            AddCell_WithBackground(row, col, CreateBorder(t), isTotal);
        }

        protected void AddHeaderCell(int row, int col, string content)
        {
            var t = CreateTextBlock(false);
            t.Text = content;
            AddCell_WithBackground(row, col, CreateBorder(t), false);
        }

        protected void AddContextCell(int row, int col, UIElement child, object? context, bool isTotal = false)
        {
            var inner = CreateBorder(child);
            SetBackground(inner, row, col);
            if (isTotal)
            {
                inner.SetBinding(VisibilityProperty, new Binding(nameof(IsTotalVisible)) { Source = this, Converter = new BooleanToVisibilityConverter() });
            }
            Border outer = new()
            {
                Background = Brushes.Transparent,
                DataContext = context,
                Child = inner,
            };
            outer.MouseLeave += (s, e) => outer.Background = Brushes.Transparent;
            outer.MouseEnter += (s, e) => outer.Background = SelectedBackground;
            if (context is not null)
            {
                outer.SetBinding(ToolTipProperty, new Binding(nameof(DetailText)) { Source = this });
                outer.MouseLeftButtonDown += OnMouseLeftButtonDown_Cell;
            }
            AddCell_General(row, col, outer);
        }

        protected void AddDataCell(int row, int col, List list, int v_total, int h_total)
        {
            var t = CreateTextBlock_Cell(true);
            TableDataCell cell = new(t, list, v_total, h_total);
            cell.Update(_cellStyle, _ratioFixed, _ratioDigits);
            _cells.Add(cell);
            AddContextCell(row, col, t, cell, v_total is 0 || h_total is 0);
        }

        private void OnMouseLeftButtonDown_Cell(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement f && f.DataContext is TableDataCell ctx)
            {
                void MouseUp(object s, System.Windows.Input.MouseButtonEventArgs e)
                {
                    f.MouseLeftButtonUp -= MouseUp;
                    f.ReleaseMouseCapture();
                    var pos = e.GetPosition(f);
                    if (pos.X >= 0 && pos.X < f.ActualWidth && pos.Y >= 0 && pos.Y < f.ActualHeight)
                    {
                        OnCellClicked(ctx);
                    }
                }
                f.MouseLeftButtonUp += MouseUp;
                f.CaptureMouse();
            }
        }

        protected virtual void OnCellClicked(TableDataCell context)
        {
            CellClicked?.Invoke(this, context);
        }

        protected static void AddItemToDictionary(GroupDictionary target, int key, object item)
        {
            target.GetOrAdd(key).Add(item);
        }

        protected virtual void UpdateDataCells()
        {
            foreach (var cell in CollectionsMarshal.AsSpan(_cells))
            {
                cell.Update(_cellStyle, _ratioFixed, _ratioDigits);
            }
        }

        protected virtual void CreateTable()
        {
            _cells.Clear();
            Children.Clear();
            if (_source is not null && _verticalSelector is not null && _horizontalSelector is not null)
            {
                GroupDictionary total_v = [];
                GroupDictionary total_h = [];
                Dictionary<int, GroupDictionary> table = [];
                var total = 0;
                // 分類
                foreach (var item in _source)
                {
                    var key_v = _verticalSelector.GetKey(item);
                    var key_h = _horizontalSelector.GetKey(item);
                    AddItemToDictionary(total_v, key_v, item);
                    AddItemToDictionary(total_h, key_h, item);
                    if (!table.TryGetValue(key_v, out var dic))
                    {
                        dic = [];
                        table.Add(key_v, dic);
                    }
                    AddItemToDictionary(dic, key_h, item);
                    total++;
                }
                Count = total;
                // テーブルの生成
                AddTotalCells(total);
                AddEmptyCell(0, 1, true);
                // ヘッダ行
                var col = 2;
                foreach (var h in _horizontalSelector.EnumHeader())
                {
                    if (total_h.TryGetValue(h.Key, out var list))
                    {
                        AddDataCell(1, col, list, 0, total);
                    }
                    else if (_horizontalSelector.SkipEmpty)
                    {
                        continue;
                    }
                    else
                    {
                        AddZeroCell(1, col, true);
                    }
                    AddHeaderCell(0, col, h.VerticalHeader);
                    col++;
                }
                // データ行
                int row = 2;
                foreach (var v in _verticalSelector.EnumHeader())
                {
                    if (total_v.TryGetValue(v.Key, out var list))
                    {
                        AddDataCell(row, 1, list, total, 0);
                    }
                    else
                    {
                        continue;
                    }
                    AddHeaderCell(row, 0, v.Header);
                    col = 2;
                    var cols = table[v.Key];
                    foreach (var h in _horizontalSelector.EnumHeader())
                    {
                        if (_horizontalSelector.SkipEmpty && !total_h.ContainsKey(h.Key))
                        {
                            continue;
                        }
                        if (cols.TryGetValue(h.Key, out list))
                        {
                            AddDataCell(row, col, list, total_h[h.Key].Count, total_v[v.Key].Count);
                        }
                        else
                        {
                            AddZeroCell(row, col);
                        }
                        col++;
                    }
                    row++;
                }
            }
            else
            {
                Count = _source is ICollection c ? c.Count : 0;
            }
        }

        public string GetTableString()
        {
            var zero = ZeroText ?? "";
            List<List<string>> result = [];
            if (_source is not null && _verticalSelector is not null && _horizontalSelector is not null)
            {
                GroupDictionary total_v = [];
                GroupDictionary total_h = [];
                Dictionary<int, GroupDictionary> table = [];
                void AddRatio(List<string> target, List list, int total)
                {
                    target.Add(TableDataCell.GetRatioText(list.Count, total, _ratioFixed, _ratioDigits));
                }
                // 分類
                var total = 0;
                foreach (var item in _source)
                {
                    var key_v = _verticalSelector.GetKey(item);
                    var key_h = _horizontalSelector.GetKey(item);
                    AddItemToDictionary(total_v, key_v, item);
                    AddItemToDictionary(total_h, key_h, item);
                    if (!table.TryGetValue(key_v, out var dic))
                    {
                        dic = [];
                        table.Add(key_v, dic);
                    }
                    AddItemToDictionary(dic, key_h, item);
                    total++;
                }
                // ヘッダ
                List<string> line = [];
                List<string> line2 = [];
                result.Add(line);
                result.Add(line2);
                // ヘッダ行スペース
                line.Add("");
                line.Add("");
                // Total行
                line2.Add($"{TotalText}");
                line2.Add($"{total}");
                // 列ヘッダ
                foreach (var h in _horizontalSelector.EnumHeader())
                {
                    if (total_h.TryGetValue(h.Key, out var list))
                    {
                        if (_cellStyle is TableViewCellStyle.RatioHorizontal)
                        {
                            AddRatio(line2, list, total);
                        }
                        else
                        {
                            line2.Add($"{list.Count}");
                        }
                    }
                    else if (_horizontalSelector.SkipEmpty)
                    {
                        continue;
                    }
                    else
                    {
                        line2.Add(zero);
                    }
                    line.Add(h.Header);
                }
                // データ行
                foreach (var v in _verticalSelector.EnumHeader())
                {
                    line = [];
                    if (total_v.TryGetValue(v.Key, out var list))
                    {
                        line.Add(v.Header);
                        if (_cellStyle is TableViewCellStyle.RatioVertical)
                        {
                            AddRatio(line, list, total);
                        }
                        else
                        {
                            line.Add($"{list.Count}");
                        }
                    }
                    else
                    {
                        continue;
                    }
                    result.Add(line);
                    var cols = table[v.Key];
                    foreach (var h in _horizontalSelector.EnumHeader())
                    {
                        if (_horizontalSelector.SkipEmpty && !total_h.ContainsKey(h.Key))
                        {
                            continue;
                        }
                        if (cols.TryGetValue(h.Key, out list))
                        {
                            switch (_cellStyle)
                            {
                                case TableViewCellStyle.RatioVertical:
                                    AddRatio(line, list, total_h[h.Key].Count);
                                    break;
                                case TableViewCellStyle.RatioHorizontal:
                                    AddRatio(line, list, total_v[v.Key].Count);
                                    break;
                                default:
                                    line.Add($"{list.Count}");
                                    break;
                            }
                        }
                        else
                        {
                            line.Add(zero);
                        }
                    }
                }
            }
            StringBuilder sb = new();
            foreach (var row in CollectionsMarshal.AsSpan(result))
            {
                sb.AppendJoin('\t', row);
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}
