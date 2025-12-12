using LivreNoirLibrary.Media;
using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Text : SkinElement
    {
        public FontFamily? FontFamily { get; set => SetValue(ref field, value); }
        public ValueExpression? FontSize { get => field; set => SetValue(ref field, value); }
        public FontWeight FontWeight { get => field; set => SetValue(ref field, value); } = FontWeights.Normal;
        public FontStyle FontStyle { get => field; set => SetValue(ref field, value); } = FontStyles.Normal;
        public FontStretch FontStretch { get => field; set => SetValue(ref field, value); } = FontStretches.Normal;
        public ValueExpression? Content { get; set => SetValue(ref field, value); }
        public LnColor Fill { get; set => SetValue(ref field, value); } = LnColor.FromRgb(255, 255, 255);
        public LnColor Stroke { get; set => SetValue(ref field, value); } = LnColor.FromRgb(0, 0, 0);
        public ValueExpression? StrokeThickness { get; set => SetValue(ref field, value); }
        public Stretch Stretch { get; set; } = Stretch.Uniform;
    }
}
