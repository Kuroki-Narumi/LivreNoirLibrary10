using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class DeckForHistoryData(Deck source)
    {
        public (int, int)[] MainDeck { get; } = CreateArray(source.MainDeck);
        public (int, int)[] ExtraDeck { get; } = CreateArray(source.ExtraDeck);
        public (int, int)[] SideDeck { get; } = CreateArray(source.SideDeck);

        private static (int, int)[] CreateArray(DeckCardList source)
        {
            var ary = new (int, int)[source.UniqueCount];
            var i = 0;
            foreach (var item in source)
            {
                ary[i++] = (item.ThisCard.Id, item.Count);
            }
            return ary;
        }
    }
}
