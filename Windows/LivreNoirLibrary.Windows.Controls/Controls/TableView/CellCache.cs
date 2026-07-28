using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    partial class TableView
    {
        protected static TableDataCell BuildCell(Border border, TextBlock t)
        {
            border.Child = t;
            TableDataCell cell = new(border, t);
            border.DataContext = cell;
            return cell;
        }

        protected Border CreateCellBorder(bool isDataCell)
        {
            Border border = new()
            {
                Focusable = false,
            };
            border.SetBinding(Border.BackgroundProperty, CreateCellBackgroundBinding(border, isDataCell));
            border.SetBinding(Border.PaddingProperty, new Binding(nameof(CellPadding)) { Source = this });
            border.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { Source = this });
            border.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { Source = this });
            border.SetBinding(MinWidthProperty, new Binding(nameof(CellWidth)) { Source = this });
            border.SetBinding(MinHeightProperty, new Binding(nameof(CellHeight)) { Source = this });
            if (isDataCell)
            {
                border.SetBinding(ToolTipProperty, new Binding(nameof(DetailText)) { Source = this });
                border.MouseLeftButtonDown += DataCell_OnMouseLeftButtonDown;
                border.MouseLeftButtonUp += DataCell_OnMouseLeftButtonUp;
            }

            return border;
        }

        private object? _currentCell;
        private void DataCell_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentCell = sender;
        }

        private void DataCell_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentCell == sender && sender is FrameworkElement { DataContext: TableDataCell cell })
            {
                RaiseCellClick(cell);
            }
        }

        protected TextBlock CreateCellText()
        {
            TextBlock t = new();
            t.SetBinding(VerticalAlignmentProperty, new Binding(nameof(VerticalCellAlignment)) { Source = this });
            t.SetBinding(HorizontalAlignmentProperty, new Binding(nameof(HorizontalCellAlignment)) { Source = this });
            return t;
        }

        protected TableDataCell CreateHeaderCell()
        {
            var border = CreateCellBorder(false);
            TextBlock t = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            t.SetBinding(TextBlock.FontSizeProperty, new Binding(nameof(SubFontSize)) { Source = this });
            return BuildCell(border, t);
        }

        protected TableDataCell CreateFixedDataCell()
        {
            var border = CreateCellBorder(false);
            var t = CreateCellText();
            t.SetBinding(TextBlock.FontSizeProperty, new Binding(nameof(SubFontSize)) { Source = this });
            return BuildCell(border, t);
        }

        protected TableDataCell CreateDataCell()
        {
            var border = CreateCellBorder(true);
            var t = CreateCellText();
            return BuildCell(border, t);
        }

        protected void BindTotalVisibility(TableDataCell cell)
        {
            cell._border.SetBinding(VisibilityProperty, new Binding(nameof(TotalVisibility)) { Source = this });
        }

        protected TableDataCell CreateTotalCell()
        {
            var cell = CreateDataCell();
            BindTotalVisibility(cell);
            return cell;
        }

        protected TableDataCell CreateZeroCell()
        {
            var cell = CreateFixedDataCell();
            cell._textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(ZeroText)) { Source = this });
            return cell;
        }

        protected class CellCache(Func<TableDataCell> factory) : ObjectCache<TableDataCell>(factory)
        {
            public void UpdateText(TableViewValueStyle style, int digits, TableViewDigitMode mode)
            {
                foreach (var data in ActiveElements)
                {
                    data.UpdateText(style, digits, mode);
                }
            }
        }
    }
}
