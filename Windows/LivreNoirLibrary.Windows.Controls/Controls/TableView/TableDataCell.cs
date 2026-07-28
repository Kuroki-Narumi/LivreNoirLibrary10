using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public class TableDataCell : IClear
    {
        internal readonly Border _border;
        internal readonly TextBlock _textBlock;
        internal readonly List<object> _list = [];

        public int VTotal { get; internal set; }
        public int HTotal { get; internal set; }
        public UIElement RootElement => _border;
        public string? Text { get => _textBlock.Text; internal set => _textBlock.Text = value; }
        public IEnumerable Items => _list;
        public int Count => _list.Count;

        internal TableDataCell(Border border, TextBlock textBlock)
        {
            _border = border;
            _textBlock = textBlock;
        }
        
        public void Clear()
        {
            _list.Clear();
        }

        public void UpdateText(TableViewValueStyle style, int digits, TableViewDigitMode mode)
        {
            var count = _list.Count;
            _textBlock.Text = style switch
            {
                TableViewValueStyle.RatioVertical => GetRatioText(count, VTotal, digits, mode),
                TableViewValueStyle.RatioHorizontal => GetRatioText(count, HTotal, digits, mode),
                _ => $"{count}",
            };
        }

        private static readonly Dictionary<int, string> _zeroes = [];

        public static string GetRatioText(int count, int total, int digits, TableViewDigitMode mode)
        {
            if (total <= 0)
            {
                return $"{count}";
            }
            var ratio = count * 100.0 / total;
            if (ratio >= 100)
            {
                return "100";
            }
            var format = _zeroes.GetOrAdd(digits, static key => $"0.{new string('0', key)}");
            var text = ratio.ToString(format);
            if (mode is TableViewDigitMode.Entire)
            {
                var index = text.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator) - 1;
                return text[..Math.Max(index, digits)];
            }
            else
            {
                return text;
            }
        }
    }
}
