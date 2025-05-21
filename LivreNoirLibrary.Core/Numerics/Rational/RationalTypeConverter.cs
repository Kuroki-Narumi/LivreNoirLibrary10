using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace LivreNoirLibrary.Numerics
{
    public class RationalTypeConverter : TypeConverter
    {
        private static readonly HashSet<Type> _convert_types = [
            typeof(string), 
            typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(nint), typeof(nuint), typeof(long), typeof(ulong), typeof(Int128), typeof(UInt128),
            typeof(Half), typeof(float), typeof(double), typeof(decimal), typeof(Rational)
        ];

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => _convert_types.Contains(sourceType) || base.CanConvertFrom(context, sourceType);
        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) => (destinationType is not null && _convert_types.Contains(destinationType)) || base.CanConvertTo(context, destinationType);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            return value switch
            {
                string v => Rational.Parse(v, culture),
                int v => (Rational)v,
                long v => (Rational)v,
                double v => (Rational)v,
                decimal v => (Rational)v,
                Rational v => v,
                byte v => (Rational)v,
                sbyte v => (Rational)v,
                short v => (Rational)v,
                ushort v => (Rational)v,
                uint v => (Rational)v,
                ulong v => (Rational)v,
                Int128 v => (Rational)v,
                UInt128 v => (Rational)v,
                Half v => (Rational)v,
                float v => (Rational)v,
                _ => base.ConvertFrom(context, culture, value)
            };
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is Rational v)
            {
                if (destinationType == typeof(string))
                {
                    return v.ToString();
                }
                else if (destinationType == typeof(int))
                {
                    return (int)v;
                }
                else if (destinationType == typeof(long))
                {
                    return (long)v;
                }
                else if (destinationType == typeof(double))
                {
                    return (double)v;
                }
                else if (destinationType == typeof(decimal))
                {
                    return (decimal)v;
                }
                else if (destinationType == typeof(Rational))
                {
                    return v;
                }
                else if (destinationType == typeof(byte))
                {
                    return (byte)v;
                }
                else if (destinationType == typeof(sbyte))
                {
                    return (sbyte)v;
                }
                else if (destinationType == typeof(short))
                {
                    return (short)v;
                }
                else if (destinationType == typeof(ushort))
                {
                    return (ushort)v;
                }
                else if (destinationType == typeof(uint))
                {
                    return (uint)v;
                }
                else if (destinationType == typeof(ulong))
                {
                    return (ulong)v;
                }
                else if (destinationType == typeof(Int128))
                {
                    return (Int128)v;
                }
                else if (destinationType == typeof(UInt128))
                {
                    return (UInt128)v;
                }
                else if (destinationType == typeof(Half))
                {
                    return (Half)v;
                }
                else if (destinationType == typeof(float))
                {
                    return (float)v;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
