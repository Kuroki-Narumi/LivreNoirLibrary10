using System;
using System.Globalization;
using System.Windows.Data;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public class LimitIconConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Card card)
            {
                return Icons.GetLimitIcon(Regulation.Instance.Get(card));
            }
            else if (value is ICardWrapper w)
            {
                return Icons.GetLimitIcon(Regulation.Instance.Get(w.Card));
            }
            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
