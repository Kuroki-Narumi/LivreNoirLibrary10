using System;
using System.Globalization;
using System.Windows.Data;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public class LinkIconConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICard card)
            {
                return card.CardType.IsLink() ? Icons.GetLinkIcon((LinkDirection)card.Def) : Icons.GetCardIcon(Icons.GetIconType(card));
            }
            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
