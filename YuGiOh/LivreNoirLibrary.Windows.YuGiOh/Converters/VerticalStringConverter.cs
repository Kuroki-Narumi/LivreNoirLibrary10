using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Converters
{
    public class VerticalStringConverter : IValueConverter
    {
        private static readonly SelectCharsStringConverter _reverse = new(static c => c is not ('\r' or '\n'));

        public static string Convert(ReadOnlySpan<char> text) => text.Length is 0 ? "" : string.Create(text.Length * 2 - 1, text, ProcessCreate);
        public static string ConvertBack(ReadOnlySpan<char> text) => _reverse.Convert(text);

        private static void ProcessCreate(Span<char> target, ReadOnlySpan<char> source)
        {
            for (var i = 0; i < target.Length; i++)
            {
                target[i] = i % 2 is 0 ? source[i / 2] : '\n';
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                return Convert(s);
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                return ConvertBack(s);
            }
            throw new NotImplementedException();
        }
    }
}
