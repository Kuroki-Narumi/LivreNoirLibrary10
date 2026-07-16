using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardEnumerable
    {
        public IEnumerable<ICard> EnumerateCards();
    }
}
