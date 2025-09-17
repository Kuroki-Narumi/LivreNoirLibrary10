using System;
using System.Globalization;
using System.Windows.Data;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public class FrameBrushConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICard card)
            {
                return Icons.GetFrameBrush(Icons.GetIconType(card));
            }
            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
