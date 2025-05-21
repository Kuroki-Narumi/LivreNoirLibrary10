using System;
using System.Globalization;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Media
{
    public class FormattedTextOption
    {
        public static FontFamily DefaultFontFamily { get; } = new("Yu Gothic UI");

        public CultureInfo CultureInfo { get; set; }
        public FlowDirection FlowDirection { get; set; }
        public FontFamily FontFamily { get; set; }
        public FontStyle FontStyle { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontStretch FontStretch { get; set; }
        public double FontSize { get; set; }
        public Brush? Foreground { get; set; }
        public double PixelsPerDip { get; set; }

        public FormattedTextOption()
        {
            CultureInfo = CultureInfo.CurrentCulture;
            FlowDirection = FlowDirection.LeftToRight;
            FontFamily = DefaultFontFamily;
            FontStyle = FontStyles.Normal;
            FontWeight = FontWeights.Normal;
            FontStretch = FontStretches.Normal;
            FontSize = 12;
            Foreground = Brushes.Black;
            PixelsPerDip = 1;
        }

        public FormattedTextOption(Control control)
        {
            CultureInfo = CultureInfo.CurrentCulture;
            FlowDirection = control.FlowDirection;
            FontFamily = control.FontFamily;
            FontStyle = control.FontStyle;
            FontWeight = control.FontWeight;
            FontStretch = control.FontStretch;
            FontSize = control.FontSize;
            Foreground = control.Foreground;
            PixelsPerDip = VisualTreeHelper.GetDpi(control).PixelsPerDip;
        }
    }
}
