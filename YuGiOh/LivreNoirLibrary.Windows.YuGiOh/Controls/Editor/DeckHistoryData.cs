using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class DeckHistoryData(Deck? deck) : HistoryDataWithSelectionBase(4), IHistoryData<DeckHistoryData>
    {
        private readonly DeckForHistoryData? _data = deck is not null ? new(deck) : null;

        public bool EqualsAll(DeckHistoryData other)
        {
            var left = _data;
            var right = other._data;
            if (left is null)
            {
                return right is null;
            }
            if (right is null)
            {
                return false;
            }
            return left.MainDeck.SequenceEqual(right.MainDeck) &&
                   left.ExtraDeck.SequenceEqual(right.ExtraDeck) &&
                   left.SideDeck.SequenceEqual(right.SideDeck);
        }

        public void ConvertBack(Deck? target, ReadOnlySpan<ListBox> listViews, ICardProvider? provider)
        {
            if (target is not null && _data is { } data)
            {
                target.Load(data, provider);
                RestoreSelection(listViews);
            }
        }
    }
}
