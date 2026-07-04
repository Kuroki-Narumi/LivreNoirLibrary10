using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows
{
    public static partial class StructExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ToColor(this in LnColor color) => Color.FromArgb(color.A, color.R, color.G, color.B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LnColor ToLnColor(this in Color color) => new(color.A, color.R, color.G, color.B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LnColor ToLnColorWithoutAlpha(this in Color color) => LnColor.FromRgb(color.R, color.G, color.B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FloatColor ToFloatColor(this in Color color) => FloatColor.FromByte(color.A, color.R, color.G, color.B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FloatColor ToFloatColorWithoutAlpha(this in Color color) => FloatColor.FromByte(color.R, color.G, color.B);

        public static string GetColorCode(this Color color, bool alpha = true)
        {
            return alpha ? $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}" : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public static Color ToColor(this ReadOnlySpan<char> colorCode)
        {
            if (colorCode.Length is 0)
            {
                return default;
            }
            if (ColorUtils.TryParseColorCodeToByte(colorCode, out var a, out var r, out var g, out var b))
            {
                return Color.FromArgb(a, r, g, b);
            }
            throw new FormatException($"wrong color code style: {colorCode}");
        }

        public static bool TryParseToColor(this ReadOnlySpan<char> colorCode, out Color color)
        {
            if (ColorUtils.TryParseColorCodeToByte(colorCode, out var a, out var r, out var g, out var b))
            {
                color = Color.FromArgb(a, r, g, b);
                return true;
            }
            color = default;
            return false;
        }
    }
}
