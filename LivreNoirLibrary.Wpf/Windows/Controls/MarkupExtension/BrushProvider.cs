using System;
using System.Windows.Markup;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public class BrushProvider : MarkupExtension
    {
        [ConstructorArgument("color")]
        public string? Color { get; set; }

        public BrushProvider() { }
        public BrushProvider(string color) => Color = color;

        public override object ProvideValue(IServiceProvider serviceProvider) => GetBrush(Color);

        public static SolidColorBrush GetBrush(string? name)
        {
            Color color = default;
            if (!string.IsNullOrEmpty(name) && !ColorExtension.TryParseToColor(name, out color))
            {
                color = (Color)ColorConverter.ConvertFromString(name);
            }
            return MediaUtils.GetBrush(color);
        }
    }
}
