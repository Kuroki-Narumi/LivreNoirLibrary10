using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.AccessControl;
using System.Text;

namespace LivreNoirLibrary.Media
{
    public sealed class ClipRectTypeConverter : TypeConverter
    {
        private static readonly HashSet<Type?> _types = [typeof(string), typeof(ClipRect), typeof(float[]), typeof(double[])];

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
            ClipRect v => v,
            float[] v => new ClipRect(v[0], v[1], v[2], v[3]),
            double[] v => new ClipRect(v[0], v[1], v[3], v[4]),
            _ => base.ConvertFrom(context, culture, value),
        };

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is ClipRect r)
            {
                if (destinationType == typeof(string))
                {
                    return r.ToString();
                }
                if (destinationType == typeof(ClipRect))
                {
                    return r;
                }
                if (destinationType == typeof(float[]))
                {
                    return new float[] { (float)r.X, (float)r.Y, (float)r.Width, (float)r.Height };
                }
                if (destinationType == typeof(double[]))
                {
                    return new double[] { r.X, r.Y, r.Width, r.Height };
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public static ClipRect ConvertFromStatic(string text)
        {
            if (TupleStringConverter.TryConvertFromString<double, double, double, double>(text, out var tuple))
            {
                return new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
            }
            throw new NotImplementedException();
        }
    }
}
