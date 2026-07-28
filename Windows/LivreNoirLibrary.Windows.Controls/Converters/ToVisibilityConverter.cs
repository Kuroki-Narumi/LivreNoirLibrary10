using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Converters
{
    public class ToVisibilityConverter : IValueConverter
    {
        public bool IsInverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertCore(value, targetType);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertCore(value, targetType);

        private object ConvertCore(object value, Type targetType)
        {
            if (targetType == typeof(Visibility))
            {
                return (IsInverted ^ BooleanConverter.IsFalsy(value)) ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return IsInverted ^ (value is Visibility.Visible);
            }
        }
    }
}
