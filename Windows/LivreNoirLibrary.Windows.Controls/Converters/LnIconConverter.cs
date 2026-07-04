using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using V = LivreNoirLibrary.Media.VectorGraphics;

namespace LivreNoirLibrary.Windows.Converters
{
    public class LnIconConverter : IValueConverter
    {

        private static readonly Dictionary<V.ElementGroup, DrawingGroup> _cache = [];

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is V.ElementGroup elements)
            {
                return _cache.GetOrAdd(elements, static i => Media.Icons.Create(i));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
