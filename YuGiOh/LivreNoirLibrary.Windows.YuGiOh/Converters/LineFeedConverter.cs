using System;
using System.Globalization;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Converters
{
    public class LineFeedConverter : IValueConverter
    {
        public static string? Convert(string? text) => text?.ReplaceLineEndings();
        public static string? ConvertBack(string? text) => text?.ReplaceLineEndings("\n");

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value is string s ? s : value.ToString();
            return Convert(text);
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value is string s ? s : value.ToString();
            return ConvertBack(text);
        }
    }
}
