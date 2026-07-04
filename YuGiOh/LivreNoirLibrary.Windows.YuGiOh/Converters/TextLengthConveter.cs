using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Converters
{
    public class TextLengthConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value is string s ? s : value.ToString();
            return LivreNoirLibrary.YuGiOh.Vocab.GetTextLength(text);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
