using System;
using System.Windows.Markup;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public class PenProvider : MarkupExtension
    {
        [ConstructorArgument("color")]
        public string? Color { get; set; }
        public Brush? Brush { get; set; }
        public double Thickness { get; set; } = 1;

        public PenProvider() { }
        public PenProvider(string color) => Color = color;

        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            Brush brush = Brush ?? BrushProvider.GetBrush(Color);
            return MediaUtils.GetPen(brush, Thickness);
        }
    }
}
