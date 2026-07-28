using LivreNoirLibrary.Collections;
using LivreNoirLibrary.YuGiOh.Data;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public static class SmallWorld
    {
        public static bool IsMatch(Card a, Card b, int requiredCount = 1, bool allowsGreater = false)
        {
            var match = 0;
            if (a.MonsterType == b.MonsterType) match++;
            if (a.Attribute == b.Attribute) match++;
            if (a.Level == b.Level) match++;
            if (a.Atk == b.Atk) match++;
            if (a.Def == b.Def) match++;
            return allowsGreater ? match >= requiredCount : match == requiredCount;
        }

        public static NeighborEnumerator EnumerateNeighbor(ICard a, ICardEnumerable source) => new(a.ThisCard, source);
        public static NeighborEnumerator EnumerateNeighbor(ICard a, IEnumerable<Card> source) => new(a.ThisCard, source);

        public static HashSet<Card> CreateCardSet() => new(CardIdEqualityComparer.Default);
        public static HashSet<Card> CreateCardSet(ICardProvider provider) => new(new CardIdEqualityComparer() { Provider = provider });

        public static ArrayPoolDisposable<int> RentFlagsArray()
        {
            var o = ArrayPool.Rent<int>(BitFlags.GetArrayLength(CardDataCollection.Capacity));
            o.Array.Clear();
            return o;
        }

        public static void FindRelay(ICardCollection target, IEnumerable<Card> cards, IEnumerable<Card> compareSource)
        {
            var enumer = cards.GetEnumerator();
            target.Clear();
            if (!enumer.MoveNext())
            {
                return;
            }
            target.AddRange(EnumerateNeighbor(enumer.Current, compareSource));

            using var o = RentFlagsArray();
            var array = o.Array;
            var span = o.Span;

            while (enumer.MoveNext())
            {
                var card = enumer.Current;
                array.Clear();
                foreach (var c in EnumerateNeighbor(card, compareSource))
                {
                    BitFlags.Set(span, c.Id);
                }
                target.RemoveAll(Predicate);
            }

            bool Predicate(Card card) => !BitFlags.IsSet(array, card.Id);
        }

        public static void FindRelay(ICardCollection target, ICardEnumerable cards, ICardEnumerable compareSource) => FindRelay(target, cards.CardEnumerable, compareSource.CardEnumerable);

        public struct NeighborEnumerator : ISafeEnumerator<Card>
        {
            private readonly Card _card;
            private readonly IEnumerator<Card> _enumerator;
            private Card? _current;

            public readonly Card Current => _current!;

            public NeighborEnumerator(Card card, ICardEnumerable source)
            {
                _card = card;
                if (card.IsMainDeckMonster())
                {
                    _enumerator = source.CardEnumerable.GetEnumerator();
                }
                else
                {
                    _enumerator = Enumerable.Empty<Card>().GetEnumerator();
                }
            }

            public NeighborEnumerator(Card card, IEnumerable<Card> source)
            {
                _card = card;
                if (card.IsMainDeckMonster())
                {
                    _enumerator = source.GetEnumerator();
                }
                else
                {
                    _enumerator = Enumerable.Empty<Card>().GetEnumerator();
                }
            }

            public bool MoveNext()
            {
                var e = _enumerator;
                var card = _card;
                while (e.MoveNext())
                {
                    var target = e.Current;
                    if (target.IsMainDeckMonster() && IsMatch(target, card, 1))
                    {
                        _current = target;
                        return true;
                    }
                }
                _current = null;
                return false;
            }
        }
    }
}
