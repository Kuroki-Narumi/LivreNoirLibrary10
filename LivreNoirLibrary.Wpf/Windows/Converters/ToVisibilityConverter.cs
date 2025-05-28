using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Converters
{
    public class ToVisibilityConverter : IValueConverter
    {
        public bool IsInverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (IsInverted ^ WindowsExtensions.IsFalsy(value)) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return IsInverted ^ (value is Visibility.Visible);
        }
    }
}
