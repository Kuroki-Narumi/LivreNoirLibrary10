using LivreNoirLibrary.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardProvider : IEnumerable<Card>, ICardEnumerable
    {
        Card? GetOrDefault(int id);
        bool TryGetByName(string name, [MaybeNullWhen(false)] out Card card);

        IEnumerable<Card> ICardEnumerable.CardEnumerable => this;
    }

    public class EmptyCardProvider : ICardProvider, ISafeEnumerator<Card>
    {
        public static EmptyCardProvider Instance { get; } = new();

        private EmptyCardProvider() { }

        public Card? GetOrDefault(int id) => null;
        public bool TryGetByName(string name, [MaybeNullWhen(false)] out Card card)
        {
            card = null;
            return false;
        }

        Card IEnumerator<Card>.Current => null!;
        bool IEnumerator.MoveNext() => false;
    }

    public static partial class Extensions
    {
        extension (ICardProvider? p)
        {
            public bool TryGet(int id, [MaybeNullWhen(false)] out Card card)
            {
                card = p?.GetOrDefault(id);
                return card is not null;
            }
        }
    }
}
