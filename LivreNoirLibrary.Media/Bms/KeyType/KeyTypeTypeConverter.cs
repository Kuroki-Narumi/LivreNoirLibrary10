using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace LivreNoirLibrary.Media.Bms
{
    public class KeyTypeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string) || sourceType == typeof(KeyType) || base.CanConvertFrom(sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) 
            => destinationType == typeof(string) || destinationType == typeof(KeyType) || base.CanConvertTo(destinationType);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is KeyType k)
            {
                return k;
            }
            if (value is string str)
            {
                return KeyType.Parse(str);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is KeyType k)
            {
                if (destinationType == typeof(string))
                {
                    return k.ToString();
                }
                if (destinationType == typeof(KeyType))
                {
                    return k;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
