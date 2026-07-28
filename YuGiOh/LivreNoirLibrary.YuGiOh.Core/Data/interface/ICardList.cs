using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardList : ICardCollection, IList<Card>
    {
    }
}
