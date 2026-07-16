using System;
using System.Collections.Generic;
using System.ComponentModel;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public readonly struct CardSortOption(SortKey key, ListSortDirection direction)
    {
        public SortKey Key { get; } = key;
        public ListSortDirection Direction { get; } = direction;
        public bool IsAscending => Direction is ListSortDirection.Ascending;
        public bool IsDescending => Direction is ListSortDirection.Descending;

        public CardSortOption(SortKey key, bool isDescending) : this(key, isDescending ? ListSortDirection.Descending : ListSortDirection.Ascending) { }

        public int GetIntValue() => (int)Key * (IsDescending ? -1 : 1);
        public static CardSortOption FromIntValue(int value) => value >= 0 ? new((SortKey)value, ListSortDirection.Ascending) : new((SortKey)(-value), ListSortDirection.Descending);

        public string PropertyName => GetPropertyName(Key, IsDescending);

        private static HashSet<string> AppendD { get; } = new(StringComparer.OrdinalIgnoreCase)
        {
            nameof(Card.RubyForSort),
            nameof(Card.EnNameForSort),
            nameof(Card.AttributeIndex),
            nameof(Card.MonsterTypeIndex),
            nameof(Card.LevelIndex),
            nameof(Card.AtkIndex),
            nameof(Card.DefIndex),
            nameof(Card.ScaleIndex),
            nameof(Card.RubyLength),
            nameof(Card.EnNameLength),
            nameof(Card.PendulumTextLength),
            nameof(Card.FirstDateOcg),
            nameof(Card.LastDateOcg),
            nameof(Card.FirstDateTcg),
            nameof(Card.LastDateTcg),
        };

        private static (string Name, string? DecName)[] PropNames { get; } = CreatePropNames();

        private static (string, string?)[] CreatePropNames()
        {
            var result = new (string, string?)[(int)SortKey._Count];
            result[0] = ("", null);
            var set = AppendD;
            Add(SortKey.Id, nameof(Card.Id), result, set);
            Add(SortKey.Name, nameof(Card.Name), result, set);
            Add(SortKey.Ruby, nameof(Card.RubyForSort), result, set);
            Add(SortKey.EnName, nameof(Card.EnNameForSort), result, set);
            Add(SortKey.CardType, nameof(Card.TypeIndex), result, set);
            Add(SortKey.Attribute, nameof(Card.AttributeIndex), result, set);
            Add(SortKey.MonsterType, nameof(Card.MonsterTypeIndex), result, set);
            Add(SortKey.Level, nameof(Card.LevelIndex), result, set);
            Add(SortKey.Atk, nameof(Card.AtkIndex), result, set);
            Add(SortKey.Def, nameof(Card.DefIndex), result, set);
            Add(SortKey.Scale, nameof(Card.ScaleIndex), result, set);
            Add(SortKey.Tuner, nameof(Card.TunerIndex), result, set);
            Add(SortKey.Effect, nameof(Card.EffectIndex), result, set);
            Add(SortKey.NameLength, nameof(Card.NameLength), result, set);
            Add(SortKey.RubyLength, nameof(Card.RubyLength), result, set);
            Add(SortKey.EnNameLength, nameof(Card.EnNameLength), result, set);
            Add(SortKey.TextLength, nameof(Card.TextLength), result, set);
            Add(SortKey.PTextLength, nameof(Card.PendulumTextLength), result, set);
            Add(SortKey.FirstDateOcg, nameof(Card.FirstDateOcg), result, set);
            Add(SortKey.LastDateOcg, nameof(Card.LastDateOcg), result, set);
            Add(SortKey.FirstDateTcg, nameof(Card.FirstDateTcg), result, set);
            Add(SortKey.LastDateTcg, nameof(Card.LastDateTcg), result, set);
            Add(SortKey.PackCount, nameof(Card.PackCount), result, set);
            Add(SortKey.PackCountOcg, nameof(Card.PackCountOcg), result, set);
            Add(SortKey.PackCountTcg, nameof(Card.PackCountTcg), result, set);
            return result;

            static void Add(SortKey key, string propName, (string, string?)[] result, HashSet<string> set)
            {
                result[(int)key] = ($"ThisCard.{propName}", set.Contains(propName) ? $"ThisCard.{propName}D" : null);
            }
        }

        public static string GetActualProperyName(string key, bool isDescending)
        {
            if (Enum.TryParse<SortKey>(key, true, out var value))
            {
                return GetPropertyName(value, isDescending);
            }
            return AppendD.Contains(key) ? $"ThisCard.{key}" : $"ThisCard.{key}D";
        }

        public static string GetPropertyName(SortKey key, bool isDescending)
        {
            var index = (uint)key;
            var ary = PropNames;
            if (index < (uint)ary.Length)
            {
                var (name, dName) = ary[index];
                return dName is not null && isDescending ? dName : name;
            }
            return "";
        }
    }
}
