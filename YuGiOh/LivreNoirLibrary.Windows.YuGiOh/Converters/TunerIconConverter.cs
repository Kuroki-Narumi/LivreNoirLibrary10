using System;
using System.Globalization;
using System.Windows.Data;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public class TunerIconConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is ICard card && card.Ability.IsTuner() ? Icons.TunerIcon : (object?)null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
