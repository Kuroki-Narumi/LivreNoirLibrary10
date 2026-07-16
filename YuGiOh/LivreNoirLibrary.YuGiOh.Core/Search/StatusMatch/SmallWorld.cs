using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public static class SmallWorld
    {
        public static bool IsMatch(Card a, Card b, int requiredCount = 1)
        {
            var match = 0;
            if (a.MonsterType == b.MonsterType) match++;
            if (a.Attribute == b.Attribute) match++;
            if (a.Level == b.Level) match++;
            if (a.Atk == b.Atk) match++;
            if (a.Def == b.Def) match++;
            return match == requiredCount;
        }

        public static bool IsMatch(Card a, Card b, out string matchText, int requiredCount = 1)
        {
            using var o = ObjectPool.RentStringBuilder(out var sb);
            var match = 0;

            void Append(string text)
            {
                if (match is > 0)
                {
                    sb.Append('/');
                }
                sb.Append(text);
                match++;
            }

            if (a.MonsterType == b.MonsterType)
            {
                Append(Vocab.GetName(a.MonsterType));
            }
            if (a.Attribute == b.Attribute)
            {
                Append(Vocab.GetName(a.Attribute));
            }
            if (a.Level == b.Level)
            {
                Append($"★{a.Level}");
            }
            if (a.Atk == b.Atk)
            {
                Append($"ATK{Vocab.GetStatusText(a.Atk)}");
            }
            if (a.Def == b.Def)
            {
                Append($"DEF{Vocab.GetStatusText(a.Def)}");
            }
            matchText = sb.ToString();
            return match == requiredCount;
        }

        public static IEnumerable<Card> EnumerateNeighbor(ICard a, ICardEnumerable source)
        {
            if (a.IsMonster())
            {
                foreach (var b in source.EnumerateCards())
                {
                    var bCard = b.ThisCard;
                    if (b.IsMainMonster() && IsMatch(a.ThisCard, bCard))
                    {
                        yield return bCard;
                    }
                }
            }
        }

        public static HashSet<Card> FindRelay(ICardEnumerable cards, ICardEnumerable source, HashSet<Card>? result = null)
        {
            result ??= [];
            var first = true;
            foreach (var a in cards.EnumerateCards())
            {
                if (a.IsMainMonster())
                {
                    if (first)
                    {
                        result.UnionWith(EnumerateNeighbor(a, source));
                    }
                    else
                    {
                        result.IntersectWith(EnumerateNeighbor(a, source));
                    }
                }
            }
            return result;
        }

        public static Graph CreateGraph(ICardEnumerable cards, Graph? result = null, List<Card>? buffer = null)
        {
            result ??= [];
            result.Clear();
            buffer ??= [];
            buffer.Clear();
            foreach (var card in cards.EnumerateCards())
            {
                if (card.IsMainMonster())
                {
                    buffer.Add(card.ThisCard);
                    result.Add(card.ThisCard.Name);
                }
            }
            var count = buffer.Count;
            for (var i = 0; i < count; i++)
            {
                var a = buffer[i];
                var aName = a.Name;
                for (var j = i + 1; j < count; j++)
                {
                    var b = buffer[j];
                    var bName = b.Name;
                    if (IsMatch(a, b, out var matchText))
                    {
                        result.AddEdge(aName, bName, matchText);
                    }
                }
            }
            buffer.Clear();
            return result;
        }
    }
}
