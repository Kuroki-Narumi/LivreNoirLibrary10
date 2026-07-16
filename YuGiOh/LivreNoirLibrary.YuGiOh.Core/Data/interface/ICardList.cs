using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardList : ICardEnumerable
    {
        bool Contains(ICard card);
        void Add(ICard card);
        bool Remove(ICard card);
        void Load(IEnumerable<ICard> cards);
    }
}
