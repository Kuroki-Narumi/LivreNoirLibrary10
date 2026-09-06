using LivreNoirLibrary.Text;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace LivreNoirLibrary.Media
{
    public sealed class LnColorTypeConverter : ColorTypeConverterBase
    {
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) => value switch
        {
            string v => ConvertFromStatic(v),
            LnColor v => v,
            FloatColor v => v.ToByteColor(),
            byte[] v => ConvertFromStatic(v),
            int[] v => ConvertFromStatic(v),
            float[] v => ConvertFromStatic(v),
            double[] v => ConvertFromStatic(v),
            _ => base.ConvertFrom(context, culture, value),
        };

        public static LnColor ConvertFromStatic(string text)
        {
            if (ColorUtils.TryParseColorCodeToByte(text, out var a, out var r, out var g, out var b))
            {
                return new(a, r, g, b);
            }
            else if (TupleStringConverter.TryConvertFromString<byte, byte, byte, byte>(text, out var tuple4))
            {
                return new(tuple4.Item1, tuple4.Item2, tuple4.Item3, tuple4.Item4);
            }
            else if (TupleStringConverter.TryConvertFromString<byte, byte, byte>(text, out var tuple3))
            {
                return new(255, tuple3.Item1, tuple3.Item2, tuple3.Item3);
            }
            throw new NotImplementedException();
        }

        public static LnColor ConvertFromStatic<T>(ReadOnlySpan<T> value)
            where T : INumber<T>
        {
            if (value.Length is 3)
            {
                return new(255, byte.CreateSaturating(value[0]), byte.CreateSaturating(value[1]), byte.CreateSaturating(value[2]));
            }
            else if (value.Length is >= 4)
            {
                return new(byte.CreateSaturating(value[0]), byte.CreateSaturating(value[1]), byte.CreateSaturating(value[2]), byte.CreateSaturating(value[3]));
            }
            throw new NotImplementedException();
        }

        public static T[] ConvertToStatic<T>(LnColor value)
            where T : INumber<T>
        {
            var (a, r, g, b) = value;
            return [T.CreateSaturating(a), T.CreateSaturating(r), T.CreateSaturating(g), T.CreateSaturating(b)];
        }
    }
}
