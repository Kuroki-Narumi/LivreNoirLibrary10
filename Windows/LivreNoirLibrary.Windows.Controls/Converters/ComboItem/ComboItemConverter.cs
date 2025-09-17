using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Converters
{
    public class ComboItemConverter : IValueConverter
    {
        private static readonly Dictionary<Type, Func<object, object>> _converters = [];

        public static void Register<T>()
            where T : IComboItem
        {
            _converters.TryAdd(T.KeyType, T.GetItem);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_converters.TryGetValue(value.GetType(), out var func))
            {
                return func(value);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IComboItem i)
            {
                return i.Value;
            }
            return value;
        }
    }
}
