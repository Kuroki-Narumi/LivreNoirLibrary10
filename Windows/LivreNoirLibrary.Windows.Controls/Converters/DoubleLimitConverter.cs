using System;
using System.Globalization;
using System.Windows.Data;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Converters
{
    public class DoubleLimitConverter : IValueConverter
    {
        public double Minimum { get; set; } = double.NaN;
        public double Maximum { get; set; } = double.NaN;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (NumberExtensions.TryGetDouble(value, out var v))
            {
                if (double.IsFinite(Maximum) && v > Maximum)
                {
                    v = Maximum;
                }
                if (double.IsFinite(Minimum) && v < Minimum)
                {
                    v = Minimum;
                }
                return v;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return NumberExtensions.TryGetDouble(value, out var v) ? v : value;
        }
    }
}
