using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        [return:NotNullIfNotNull(nameof(source))]
        public static DrawingGroup? Convert(V.ElementGroup? source) => source is null ? null : _cache.GetOrAdd(source, static i => Media.Icons.Create(i));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is V.ElementGroup elements ? Convert(elements) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
