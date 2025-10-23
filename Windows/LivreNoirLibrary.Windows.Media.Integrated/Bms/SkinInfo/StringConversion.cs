using System;
using System.Globalization;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    internal static class StringConversion
    {
        public static StringComparer DefaultComparer { get; } = StringComparer.Create(CultureInfo.InvariantCulture, true);

        public static string GetRectText(this IRectNode obj) => $"{obj.X}, {obj.Y}, {obj.Width}, {obj.Height}";

        public static void SetRectText(this IRectNode obj, string value)
        {
            var span = value.AsSpan().Trim();
            if (span.Length is 0)
            {
                goto Throw;
            }
            var sep = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var ranges = (stackalloc Range[5]);
            var rangeCount = span.Split(ranges, sep);
            if (rangeCount is not 4)
            {
                goto Throw;
            }
            obj.X = new(span[ranges[0]].ToString());
            obj.Y = new(span[ranges[1]].ToString());
            obj.Width = new(span[ranges[2]].ToString());
            obj.Height = new(span[ranges[3]].ToString());
            return;
        Throw:
            ThrowFormatException(value, "Rect");
        }

        public static void GetTuple(string value, out ValueExpression? v1, out ValueExpression? v2)
        {
            var span = value.AsSpan().Trim();
            if (span.Length is 0)
            {
                goto Throw;
            }
            var sep = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var ranges = (stackalloc Range[3]);
            var rangeCount = span.Split(ranges, sep);
            if (rangeCount is not 2)
            {
                goto Throw;
            }
            v1 = new(span[ranges[0]].ToString());
            v2 = new(span[ranges[1]].ToString());
            return;
        Throw:
            v1 = v2 = null;
            ThrowFormatException(value, "Rect");
        }

        private static void ThrowFormatException(string text, string typeName)
        {
            throw new FormatException($"cannot convert \"{text}\" to {typeName}.");
        }
    }
}
