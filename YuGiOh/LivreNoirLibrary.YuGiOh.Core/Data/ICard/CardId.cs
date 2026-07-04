using System;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CardId(int id) : ICardWrapper
    {
        public int Id { get; } = id;
        public Card Card => CardPool.Instance.Get(Id);
    }
}
