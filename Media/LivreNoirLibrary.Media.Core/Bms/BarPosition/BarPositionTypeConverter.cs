using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class BarPositionTypeConverter : TypeConverter
    {
        private static readonly HashSet<Type> _source = [typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(float), typeof(double), typeof(decimal), typeof(BarPosition), typeof(string)];
        private static readonly HashSet<Type?> _dest = [typeof(BarPosition), typeof(string)];

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return _source.Contains(sourceType) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        {
            return _dest.Contains(destinationType) || base.CanConvertTo(context, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            return value switch
            {
                string v => BarPosition.Parse(v),
                BarPosition v => v,
                double v => new BarPosition(v),
                int v => new BarPosition(v),
                byte v => new BarPosition(v),
                sbyte v => new BarPosition(v),
                short v => new BarPosition(v),
                ushort v => new BarPosition(v),
                float v => new BarPosition((double)v),
                decimal v => new BarPosition((double)v),
                _ => base.ConvertFrom(context, culture, value)
            };
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is BarPosition v)
            {
                if (destinationType == typeof(BarPosition))
                {
                    return v;
                }
                else if (destinationType == typeof(string))
                {
                    return v.ToString();
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
