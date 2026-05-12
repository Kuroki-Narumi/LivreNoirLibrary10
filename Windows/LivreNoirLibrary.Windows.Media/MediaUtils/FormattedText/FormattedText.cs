using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class MediaUtils
    {
        public static FormattedText CreateFormattedText(this string text, FormattedTextOptions option)
        {
            return new(
                    text,
                    option.CultureInfo,
                    option.FlowDirection,
                    new(option.FontFamily, option.FontStyle, option.FontWeight, option.FontStretch),
                    option.FontSize,
                    option.Foreground,
                    option.PixelsPerDip
                    );
        }

        public static FormattedText CreateFormattedText(this Control control, string text, FormattedTextOptions? option = null)
        {
            return CreateFormattedText(text, option ?? new(control));
        }

        public static void RenderText(this Control control, DrawingContext dc, string text, FormattedTextOptions? ftOption = null, RenderTextOption? renderOption = null)
        {
            var ft = CreateFormattedText(control, text, ftOption);
            renderOption ??= new() { Foreground = control.Foreground };
            RenderText(dc, ft, renderOption);
        }

        public static void RenderText(this DrawingContext dc, FormattedText ft, RenderTextOption option)
        {
            var x = option.X;
            switch (option.HorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch:
                    x -= ft.Width * 0.5;
                    break;
                case HorizontalAlignment.Right:
                    x -= ft.Width;
                    break;
            }
            var y = option.Y;
            switch (option.VerticalAlignment)
            {
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    y -= ft.Height * 0.5;
                    break;
                case VerticalAlignment.Bottom:
                    y -= ft.Height;
                    break;
            }
            Point origin = new(x, y);
            var g = ft.BuildGeometry(origin);
            if (option.Stroke is Pen pen)
            {
                dc.DrawGeometry(null, pen, g);
            }
            if (option.UseDrawText)
            {
                dc.DrawText(ft, origin);
            }
            else if (option.Foreground is not null)
            {
                dc.DrawGeometry(option.Foreground, null, g);
            }
        }
    }
}
