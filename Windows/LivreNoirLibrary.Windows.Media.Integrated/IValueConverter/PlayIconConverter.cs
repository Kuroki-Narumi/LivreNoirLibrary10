using System;
using System.Globalization;
using System.Windows.Data;
using LivreNoirLibrary.Windows.Controls;

namespace LivreNoirLibrary.Windows.Converters
{

    public class PlayIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (BooleanConverter.IsFalsy(value))
            {
                return Icons.Play;
            }
            return Icons.Pause;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value == Icons.Play;
    }
}
