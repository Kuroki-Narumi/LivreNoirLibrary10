using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public readonly struct TableDataCell
    {
        public readonly TextBlock TextBlock;
        public readonly List<object> List;
        public readonly int V_Total;
        public readonly int H_Total;

        internal TableDataCell(TextBlock textBlock, List<object> list, int v_total, int h_total)
        {
            TextBlock = textBlock;
            List = list;
            V_Total = v_total;
            H_Total = h_total;
        }

        public void Update(TableViewCellStyle style, bool ratioFixed, int ratioDigits)
        {
            TextBlock.Text = style switch
            {
                TableViewCellStyle.RatioVertical => GetRatioText(List.Count, V_Total, ratioFixed, ratioDigits),
                TableViewCellStyle.RatioHorizontal => GetRatioText(List.Count, H_Total, ratioFixed, ratioDigits),
                _ => GetRatioText(List.Count, 0, false, 0)
            };
        }

        public static string GetRatioText(int count, int total, bool @fixed, int digits)
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
            var text = string.Format($"{{0:0.{new string('0', digits)}}}", ratio);
            if (@fixed)
            {
                return text;
            }
            else
            {
                var p = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                var tt = text.Split(p);
                digits = digits - tt[0].Length - 1;
                return string.Format($"{{0:0.{new string('0', Math.Max(digits, 0))}}}", ratio);
            }
        }
    }
}
