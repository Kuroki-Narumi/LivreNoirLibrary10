using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static bool IsFalsy(object value) => value is null or false or 0 or "" or Visibility.Collapsed or Visibility.Hidden;
    }
}

namespace LivreNoirLibrary.Windows.Converters
{
    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !WindowsExtensions.IsFalsy(value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !WindowsExtensions.IsFalsy(value);
    }

    public class BooleanInvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => WindowsExtensions.IsFalsy(value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => WindowsExtensions.IsFalsy(value);
    }
}
