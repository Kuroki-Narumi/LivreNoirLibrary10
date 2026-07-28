using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LivreNoirLibrary.Collections
{
    public interface IGraph<TKey> : IGraph
        where TKey : notnull
    {
        bool TryGetNode(TKey key, [MaybeNullWhen(false)] out IGraphNode<TKey> node);
    }

    public interface IGraph : IClear
    {
        public int Count { get; }
        public int EdgeCount { get; }

        bool NodeEquals(object key, IGraphNode node);
        bool TryGetNode(object key, [MaybeNullWhen(false)] out IGraphNode node);

        IEnumerable<IGraphNode> EnumerateNodes();
        IEnumerable<IGraphEdge> EnumerateEdges();
        IEnumerable<IGraphNode> EnumerateConnectedNodes(IGraphNode node);
    }
}
