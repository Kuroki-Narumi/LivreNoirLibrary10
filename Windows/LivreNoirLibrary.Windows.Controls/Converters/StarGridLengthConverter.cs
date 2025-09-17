using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Converters
{
    public class StarGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (NumberExtensions.TryGetDouble(value, out var v))
            {
                if (!double.IsNaN(v))
                {
                    return GridLength.Auto;
                }
                else if (v is >= 0)
                {
                    return new GridLength(v, GridUnitType.Pixel);
                }
                else
                {
                    return new GridLength(-v, GridUnitType.Star);
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GridLength g)
            {
                return g.GridUnitType switch
                {
                    GridUnitType.Pixel => g.Value,
                    GridUnitType.Star => -g.Value,
                    _ => double.NaN,
                };
            }
            return value;
        }
    }
}
