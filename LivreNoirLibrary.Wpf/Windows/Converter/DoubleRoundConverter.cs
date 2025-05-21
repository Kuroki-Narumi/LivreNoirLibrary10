using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows
{
    public enum DoubleRounding
    {
        None,
        Round,
        Floor,
        Ceiling,
        Truncate,
    }

    public class DoubleRoundConverter : IValueConverter
    {
        public DoubleRounding Rounding { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return NumberExtensions.TryGetDouble(value, out var v) ? GetRound(v, Rounding) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;

        public static double GetRound(double value, DoubleRounding type) => type switch
        {
            DoubleRounding.Round => Math.Round(value),
            DoubleRounding.Floor => Math.Floor(value),
            DoubleRounding.Ceiling => Math.Ceiling(value),
            DoubleRounding.Truncate => Math.Truncate(value),
            _ => value
        };
    }
}
