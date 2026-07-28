using LivreNoirLibrary.YuGiOh.Data;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class CardIdEqualityComparer : IEqualityComparer<Card>, IAlternateEqualityComparer<int, Card>
    {
        public static CardIdEqualityComparer Default { get; } = new();

        public ICardProvider? Provider { get; set; }

        public Card Create(int alternate) => Provider?.GetOrDefault(alternate) ?? Card.Dummy(alternate);

        public bool Equals(Card? x, Card? y) => x is null ? y is null : y is not null && Equals(x.Id, y);
        public bool Equals(int alternate, Card other) => alternate == other.Id;

        public int GetHashCode([DisallowNull] Card obj) => GetHashCode(obj.Id);
        public int GetHashCode(int alternate) => alternate.GetHashCode();
    }
}
