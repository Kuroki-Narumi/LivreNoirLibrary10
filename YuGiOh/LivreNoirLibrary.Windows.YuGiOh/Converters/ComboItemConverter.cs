using LivreNoirLibrary.YuGiOh.MasterDuel;
using LivreNoirLibrary.YuGiOh.Search;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using System;
using System.Globalization;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Converters
{
    using LivreNoirLibrary.YuGiOh;

    public class ComboItemConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) => value switch
        {
            CardType v => CardTypeItem.GetItem(v),
            Attribute v => AttributeItem.GetItem(v),
            MonsterType v => MonsterTypeItem.GetItem(v),
            Rank v => RankItem.GetItem(v),
            Order v => OrderItem.GetItem(v),
            Result v => ResultItem.GetItem(v),
            MatchType v => MatchTypeItem.GetItem(v),
            _ => value
        };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value switch
        {
            CardTypeItem item => item.Value,
            AttributeItem item => item.Value,
            MonsterTypeItem item => item.Value,
            RankItem item => item.Value,
            OrderItem item => item.Value,
            ResultItem item => item.Value,
            MatchTypeItem item => item.Value,
            _ => value
        };
    }
}
