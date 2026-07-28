using LivreNoirLibrary.Collections;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class SmallWorldGraph() : Graph<StatusKey, SmallWorldNode>(k => new(k), false, EqualityComparer<StatusKey>.Default)
    {
        public void Build(ICardEnumerable? source, List<StatusKey>? buffer = null)
        {
            Clear();
            if (source is null)
            {
                return;
            }
            buffer ??= [];
            buffer.Clear();
            // 同じステータスごとにグループ化
            foreach (var card in source.CardEnumerable)
            {
                if (card.IsMainDeckMonster())
                {
                    var key = new StatusKey(card);
                    if (AddNode(key, out var node))
                    {
                        buffer.Add(key);
                    }
                    node.Cards.Add(card);
                }
            }
            // 辺の生成
            var count = buffer.Count;
            for (var i = 0; i < count - 1; i++)
            {
                var a = buffer[i];
                for (var j = i + 1; j < count; j++)
                {
                    var b = buffer[j];
                    if (a.IsMatch(b, out var matchText))
                    {
                        AddEdge(a, b, matchText);
                    }
                }
            }
        }
    }

    public class SmallWorldNode(StatusKey key) : IGraphNode<StatusKey>
    {
        public StatusKey Key { get; } = key;
        public string? Name => string.Join('\n', Cards.Select(card => card.Name));
        public HashSet<Card> Cards { get; } = SmallWorld.CreateCardSet();
    }
}
