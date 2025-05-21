using System;
using System.Globalization;
using System.Windows.Data;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows
{
    public class DoubleLinearConverter : IValueConverter
    {
        public double Minimum { get; set; } = double.NaN;
        public double Maximum { get; set; } = double.NaN;
        public double Slope { get; set; } = 1;
        public double Offset { get; set; }
        public DoubleRounding Rounding { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (NumberExtensions.TryGetDouble(value, out var v))
            {
                v = DoubleRoundConverter.GetRound(v * Slope.Validate(1) + Offset.Validate(), Rounding);
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
            if (NumberExtensions.TryGetDouble(value, out var v))
            {
                v -= Offset.Validate();
                if (double.IsFinite(Slope) && Slope is not 0)
                {
                    v /= Slope;
                }
                return v;
            }
            else
            {
                return value;
            }
        }
    }
}
