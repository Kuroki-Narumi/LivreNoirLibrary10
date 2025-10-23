using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public sealed partial class ValueExpressionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(ValueExpression) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        {
            return destinationType == typeof(ValueExpression) || base.CanConvertTo(context, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string str)
            {
                return new ValueExpression(str);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is ValueExpression v)
            {
                if (destinationType == typeof(string))
                {
                    return v.ToString();
                }
                if (destinationType == typeof(ValueExpression))
                {
                    return v;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
