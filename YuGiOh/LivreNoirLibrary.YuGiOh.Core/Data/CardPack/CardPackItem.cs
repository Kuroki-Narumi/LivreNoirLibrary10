using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class CardPackItem(Card card, string number) : ObservableObjectBase
    {
        private bool _keyGenerated;
        private SortKey _sortKey;

        public Card Card { get; set => SetValue(ref field, value, UpdateSortKey); } = card;
        public string Number { get; set => SetValue(ref field, value, UpdateSortKey); } = number;

        private void UpdateSortKey()
        {
            _keyGenerated = false;
        }

        public SortKey GetSortKey()
        {
            if (!_keyGenerated)
            {
                var index = GetIndex(Number, int.MaxValue);
                _sortKey = new(index, Card.Id);
                _keyGenerated = true;
            }
            return _sortKey;
        }

        [GeneratedRegex(@"([0-9a-zA-Z]{1,5})$")]
        private static partial Regex Regex_Number { get; }

        public static int GetIndex(string number, int fallback)
        {
            var span = number.AsSpan();
            foreach (var match in Regex_Number.EnumerateMatches(span))
            {
                return BasedNumber.ParseToInt(span.Slice(match.Index, match.Length), 36);
            }
            return fallback;
        }

        public readonly struct SortKey(int index, int cardId) : IComparable<SortKey>
        {
            public int Index { get; } = index;
            public int CardId { get; } = cardId;

            public int CompareTo(SortKey other)
            {
                var c = Index.CompareTo(other.Index);
                if (c is not 0)
                {
                    return c;
                }
                return CardId.CompareTo(other.CardId);
            }
        }
    }
}
