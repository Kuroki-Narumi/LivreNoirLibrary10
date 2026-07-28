using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardCollection : ICollection<Card>, ISafeEnumerable<Card>, ICardEnumerable
    {
        void AddRange(ReadOnlySpan<Card> cards);
        void AddRange(IEnumerable<Card> cards);
        int RemoveAll(Predicate<Card> predicate);
        bool ContainsId(int id);
    }
}
