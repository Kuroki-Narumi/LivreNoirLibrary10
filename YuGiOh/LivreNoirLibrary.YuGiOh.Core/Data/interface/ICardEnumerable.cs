using System;
using System.Collections.Generic;
using System.Linq;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardEnumerable : IIdEnumerable
    {
        public IEnumerable<Card> CardEnumerable { get; }

        IEnumerable<int> IIdEnumerable.IdEnumerable => CardEnumerable.Select(IId.GetId);
    }
}
