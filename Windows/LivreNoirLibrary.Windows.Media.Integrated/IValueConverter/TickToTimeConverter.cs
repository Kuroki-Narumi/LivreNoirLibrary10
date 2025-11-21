using System;
using System.Globalization;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Converters
{
    public class TickToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                long v => TimeSpan.FromTicks(v),
                byte v => TimeSpan.FromTicks(v),
                sbyte v => TimeSpan.FromTicks(v),
                short v => TimeSpan.FromTicks(v),
                ushort v => TimeSpan.FromTicks(v),
                int v => TimeSpan.FromTicks(v),
                uint v => TimeSpan.FromTicks(v),
                ulong v => TimeSpan.FromTicks((long)v),
                float v => TimeSpan.FromSeconds(v),
                double v => TimeSpan.FromSeconds(v),
                decimal v => TimeSpan.FromSeconds((double)v),
                _ => value,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan t)
            {
                return t.Ticks;
            }
            return value;
        }
    }
}
