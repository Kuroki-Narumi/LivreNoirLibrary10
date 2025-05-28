using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public partial class TextLengthConverter : IValueConverter
    {
        [GeneratedRegex("\\s")]
        public static partial Regex Regex_Space { get; }
        public bool IncludesSpace { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return IncludesSpace ? str.Length : GetLength(str);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => "";

        public static int GetLength(string text) => Regex_Space.Replace(text, "").Length;
    }
}
