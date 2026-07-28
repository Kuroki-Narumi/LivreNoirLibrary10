using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardIdList : IIdEnumerable
    {
        bool Contains(int id);
        void Add(int id);
        bool Remove(int id);
        void Load(IEnumerable<int> ids);
    }

    public static partial class Extensions
    {
        extension (ICardIdList list)
        {
            public bool Contains(ICard card) => list.Contains(card.ThisCard.Id);
            public void Add(ICard card) => list.Add(card.ThisCard.Id);
            public bool Remove(ICard card) => list.Remove(card.ThisCard.Id);
        }
    }
}
