using System;
using System.Windows.Media;
using static LivreNoirLibrary.Media.ColorUtils;

namespace LivreNoirLibrary.Media
{
    public static partial class ColorExtension
    {
        public static string GetColorCode(this Color color, bool alpha = true)
        {
            return alpha ? $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}" : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public static Color ToColor(this string colorCode)
        {
            if (TryParseColorCodeToByte(colorCode, out var a, out var r, out var g, out var b))
            {
                return Color.FromArgb(a, r, g, b);
            }
            throw new FormatException($"wrong color code style: {colorCode}");
        }

        public static bool TryParseToColor(this string colorCode, out Color color)
        {
            if (TryParseColorCodeToByte(colorCode, out var a, out var r, out var g, out var b))
            {
                color = Color.FromArgb(a, r, g, b);
                return true;
            }
            color = default;
            return false;
        }
    }
}
