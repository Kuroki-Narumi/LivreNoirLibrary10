using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace LivreNoirLibrary.Media
{
    public sealed class LnColorTypeConverter : TypeConverter
    {
        private static readonly HashSet<Type?> _types = [typeof(string), typeof(LnColor), typeof(byte[]), typeof(int[]), typeof(float[]), typeof(double[])];

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return _types.Contains(sourceType) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        {
            return _types.Contains(destinationType) || base.CanConvertTo(context, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) => value switch
        {
            string v => ConvertFromStatic(v),
            LnColor v => v,
            byte[] v => ConvertFromStatic(v),
            int[] v => ConvertFromStatic(v),
            float[] v => ConvertFromStatic(v),
            double[] v => ConvertFromStatic(v),
            _ => base.ConvertFrom(context, culture, value),
        };

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is LnColor color)
            {
                if (destinationType == typeof(string))
                {
                    return color.ToString();
                }
                else if (destinationType == typeof(LnColor))
                {
                    return color;
                }
                else if (destinationType == typeof(byte[]))
                {
                    return ConvertToStatic<byte>(color);
                }
                else if (destinationType == typeof(int[]))
                {
                    return ConvertToStatic<int>(color);
                }
                else if (destinationType == typeof(float[]))
                {
                    return ConvertToStatic<float>(color);
                }
                else if (destinationType == typeof(double[]))
                {
                    return ConvertToStatic<double>(color);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

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
                return new(tuple3.Item1, tuple3.Item2, tuple3.Item3);
            }
            throw new NotImplementedException();
        }

        public static LnColor ConvertFromStatic<T>(ReadOnlySpan<T> value)
            where T : INumber<T>
        {
            if (value.Length is 3)
            {
                return new(byte.CreateSaturating(value[0]), byte.CreateSaturating(value[1]), byte.CreateSaturating(value[2]));
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
