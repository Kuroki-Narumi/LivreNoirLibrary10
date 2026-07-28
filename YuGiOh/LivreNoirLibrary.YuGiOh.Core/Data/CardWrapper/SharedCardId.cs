using LivreNoirLibrary.ObjectModel;
using System;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public sealed class SharedCardId : IId
    {
        public int Id { get; }

        private SharedCardId(int id)
        {
            Id = id;
        }

        private static SharedCardId?[] Cache { get; } = new SharedCardId[CardDataCollection.Capacity];

        public static SharedCardId GetItem(int id) => Cache[id] ??= new(id);
        public static SharedCardId GetItem(IId obj) => GetItem(obj.Id);
        public static SharedCardId GetItem(ICard card) => GetItem(card.ThisCard.Id);
    }
}
