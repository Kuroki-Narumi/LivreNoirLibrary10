using LivreNoirLibrary.Text;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace LivreNoirLibrary.Media
{
    public sealed class FloatColorTypeConverter : ColorTypeConverterBase
    {
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) => value switch
        {
            string v => ConvertFromStatic(v),
            LnColor v => v.ToFloatColor(),
            FloatColor v => v,
            byte[] v => ConvertFromByte(v),
            int[] v => ConvertFromByte(v),
            float[] v => ConvertFromFloat(v),
            double[] v => ConvertFromFloat(v),
            _ => base.ConvertFrom(context, culture, value),
        };

        public static FloatColor ConvertFromStatic(string text)
        {
            if (ColorUtils.TryParseColorCode(text, out var a, out var r, out var g, out var b))
            {
                return new(a, r, g, b);
            }
            else if (TupleStringConverter.TryConvertFromString<float, float, float, float>(text, out var tuple4))
            {
                return new(tuple4.Item1, tuple4.Item2, tuple4.Item3, tuple4.Item4);
            }
            else if (TupleStringConverter.TryConvertFromString<float, float, float>(text, out var tuple3))
            {
                return new(1, tuple3.Item1, tuple3.Item2, tuple3.Item3);
            }
            throw new NotImplementedException();
        }

        public static FloatColor ConvertFromByte<T>(ReadOnlySpan<T> value)
            where T : INumber<T>
        {
            if (value.Length is 3)
            {
                return FloatColor.FromByte(255, byte.CreateSaturating(value[0]), byte.CreateSaturating(value[1]), byte.CreateSaturating(value[2]));
            }
            else if (value.Length is >= 4)
            {
                return FloatColor.FromByte(byte.CreateSaturating(value[0]), byte.CreateSaturating(value[1]), byte.CreateSaturating(value[2]), byte.CreateSaturating(value[3]));
            }
            throw new NotImplementedException();
        }

        public static FloatColor ConvertFromFloat<T>(ReadOnlySpan<T> value)
            where T : INumber<T>
        {
            if (value.Length is 3)
            {
                return new(1, float.CreateSaturating(value[0]), float.CreateSaturating(value[1]), float.CreateSaturating(value[2]));
            }
            else if (value.Length is >= 4)
            {
                return new(float.CreateSaturating(value[0]), float.CreateSaturating(value[1]), float.CreateSaturating(value[2]), float.CreateSaturating(value[3]));
            }
            throw new NotImplementedException();
        }

        public static T[] ConvertToByte<T>(FloatColor value)
            where T : INumber<T>
        {
            var (a, r, g, b) = value.ToByte();
            return [T.CreateSaturating(a), T.CreateSaturating(r), T.CreateSaturating(g), T.CreateSaturating(b)];
        }

        public static T[] ConvertToFloat<T>(FloatColor value)
            where T : INumber<T>
        {
            var (a, r, g, b) = value;
            return [T.CreateSaturating(a), T.CreateSaturating(r), T.CreateSaturating(g), T.CreateSaturating(b)];
        }
    }
}
