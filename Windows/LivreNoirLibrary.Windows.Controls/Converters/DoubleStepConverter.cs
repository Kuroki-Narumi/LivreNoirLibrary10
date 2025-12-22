using System;
using System.Globalization;
using System.Windows.Data;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Converters
{
    public class DoubleStepConverter : IValueConverter
    {
        public double Step { get; set; }

        public static object ConvertCore(object value, double step)
        {
            if (NumberExtensions.TryGetDouble(value, out var v))
            {
                return step is > 0 ? Math.Truncate(v / step) * step : v;
            }
            return value;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertCore(value, Step);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertCore(value, Step);
    }
}
