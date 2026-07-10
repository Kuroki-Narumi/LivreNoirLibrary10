using System;
using System.Collections.Generic;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public readonly struct CardSortOption(SortKey key, SortDirection direction)
    {
        public SortKey Key { get; } = key;
        public SortDirection Direction { get; } = direction;

        public int GetIntValue() => (int)Key * (Direction is SortDirection.Ascending ? 1 : -1);
        public static CardSortOption FromIntValue(int value) => value >= 0 ? new((SortKey)value, SortDirection.Ascending) : new((SortKey)(-value), SortDirection.Descending);

        public string PropertyName => GetPropertyName(Key, Direction);

        private static readonly Dictionary<SortKey, (string Name, bool D)> _propNames = new()
        {
            { SortKey.Id, (nameof(Card.Id), false) },
            { SortKey.Name, (nameof(Card.Name), false) },
            { SortKey.Ruby, (nameof(Card.RubyForSort), true) },
            { SortKey.EnName, (nameof(Card.EnNameForSort), true) },
            { SortKey.CardType, (nameof(Card.TypeIndex), false) },
            { SortKey.Attribute, (nameof(Card.Attribute), true) },
            { SortKey.MonsterType, (nameof(Card.MonsterType), true) },
            { SortKey.Level, (nameof(Card.LevelIndex), true) },
            { SortKey.Atk, (nameof(Card.Atk), true) },
            { SortKey.Def, (nameof(Card.Def), true) },
            { SortKey.Scale, (nameof(Card.ScaleIndex), true) },
            { SortKey.Tuner, (nameof(Card.TunerIndex), false) },
            { SortKey.Effect, (nameof(Card.EffectIndex), false) },
            { SortKey.NameLength, (nameof(Card.NameLength), false) },
            { SortKey.RubyLength, (nameof(Card.RubyLength), true) },
            { SortKey.EnNameLength, (nameof(Card.EnNameLength), true) },
            { SortKey.TextLength, (nameof(Card.TextLength), false) },
            { SortKey.PTextLength, (nameof(Card.PendulumTextLength), true) },
            { SortKey.FirstDateOcg, (nameof(Card.FirstDateOcg), true) },
            { SortKey.LastDateOcg, (nameof(Card.LastDateOcg), true) },
            { SortKey.FirstDateTcg, (nameof(Card.FirstDateTcg), true) },
            { SortKey.LastDateTcg, (nameof(Card.LastDateTcg), true) },
            { SortKey.PackCount, (nameof(Card.PackCount), false) },
            { SortKey.PackCountOcg, (nameof(Card.PackCountOcg), false) },
            { SortKey.PackCountTcg, (nameof(Card.PackCountTcg), false) },
        };

        public static string GetPropertyName(SortKey key, SortDirection dir)
        {
            if (_propNames.TryGetValue(key, out var item))
            {
                if (dir is SortDirection.Descending && item.D)
                {
                    return string.Intern($"{item.Name}D");
                }
                else
                {
                    return item.Name;
                }
            }
            return "";
        }
    }
}
