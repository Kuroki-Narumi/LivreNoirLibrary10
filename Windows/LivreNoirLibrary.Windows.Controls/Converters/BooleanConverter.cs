using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Converters
{
    public class BooleanConverter : IValueConverter
    {
        public static bool IsFalsy(object value) => value is null or false or 0 or "" or Visibility.Collapsed or Visibility.Hidden || value == DependencyProperty.UnsetValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !IsFalsy(value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !IsFalsy(value);
    }

    public class BooleanInvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => BooleanConverter.IsFalsy(value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => BooleanConverter.IsFalsy(value);
    }
}
