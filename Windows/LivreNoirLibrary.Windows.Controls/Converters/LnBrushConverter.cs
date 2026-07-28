using LivreNoirLibrary.Windows.Media;
using System;
using System.Globalization;
using System.Windows.Data;
using V = LivreNoirLibrary.Media.VectorGraphics;

namespace LivreNoirLibrary.Windows.Converters
{
    public class LnBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return MediaUtils.GetBrush(value as V.IBrush) ?? value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
