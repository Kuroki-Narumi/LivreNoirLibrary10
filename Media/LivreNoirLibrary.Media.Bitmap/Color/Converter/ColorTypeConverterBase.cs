using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace LivreNoirLibrary.Media
{
    public abstract class ColorTypeConverterBase : TypeConverter
    {
        private static readonly HashSet<Type?> _types = [typeof(string), typeof(LnColor), typeof(FloatColor), typeof(byte[]), typeof(int[]), typeof(float[]), typeof(double[])];

        public sealed override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return _types.Contains(sourceType) || base.CanConvertFrom(context, sourceType);
        }

        public sealed override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        {
            return _types.Contains(destinationType) || base.CanConvertTo(context, destinationType);
        }

        public sealed override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            switch (value)
            {
                case LnColor color:
                    if (destinationType == typeof(string))
                    {
                        return color.ToString();
                    }
                    else if (destinationType == typeof(LnColor))
                    {
                        return color;
                    }
                    else if (destinationType == typeof(FloatColor))
                    {
                        return color.ToFloat();
                    }
                    else if (destinationType == typeof(byte[]))
                    {
                        return LnColorTypeConverter.ConvertToStatic<byte>(color);
                    }
                    else if (destinationType == typeof(int[]))
                    {
                        return LnColorTypeConverter.ConvertToStatic<int>(color);
                    }
                    else if (destinationType == typeof(float[]))
                    {
                        return LnColorTypeConverter.ConvertToStatic<float>(color);
                    }
                    else if (destinationType == typeof(double[]))
                    {
                        return LnColorTypeConverter.ConvertToStatic<double>(color);
                    }
                    break;
                case FloatColor color:
                    if (destinationType == typeof(string))
                    {
                        return color.ToString();
                    }
                    else if (destinationType == typeof(LnColor))
                    {
                        return color.ToByteColor();
                    }
                    else if (destinationType == typeof(FloatColor))
                    {
                        return color;
                    }
                    else if (destinationType == typeof(byte[]))
                    {
                        return FloatColorTypeConverter.ConvertToByte<byte>(color);
                    }
                    else if (destinationType == typeof(int[]))
                    {
                        return FloatColorTypeConverter.ConvertToByte<int>(color);
                    }
                    else if (destinationType == typeof(float[]))
                    {
                        return FloatColorTypeConverter.ConvertToFloat<float>(color);
                    }
                    else if (destinationType == typeof(double[]))
                    {
                        return FloatColorTypeConverter.ConvertToFloat<double>(color);
                    }
                    break;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}