using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LivreNoirLibrary.Collections
{
    public class Graph<TKey, TNode> : IGraph<TKey>
        where TKey : notnull
        where TNode : IGraphNode<TKey>
    {
        private readonly Func<TKey, TNode> _factory;
        private readonly IEqualityComparer<TKey> _keyComparer;
        private readonly NodeEqualityComparer _nodeComparer;
        private readonly EdgeEqualityComparer _edgeComparer;

        private readonly HashSet<TNode> _nodes;
        private readonly HashSet<Edge> _edges;
        private readonly HashSet<TNode>.AlternateLookup<TKey> _nodeLookup;
        private readonly HashSet<Edge>.AlternateLookup<(TKey, TKey)> _edgeLookup;

        public bool IsDirected => !_edgeComparer._undirected;
        public int Count => _nodes.Count;
        public int EdgeCount => _edges.Count;

        public Graph(Func<TKey, TNode> factory, bool isDirected = false) : this(factory, isDirected, null) { }

        public Graph(Func<TKey, TNode> factory, bool isDirected, IEqualityComparer<TKey>? keyComparer)
        {
            _factory = factory;
            _keyComparer = keyComparer ??= EqualityComparer<TKey>.Default;
            _nodeComparer = new(keyComparer);
            _edgeComparer = new(!isDirected, keyComparer);

            _nodes = new(_nodeComparer);
            _edges = new(_edgeComparer);
            _nodeLookup = _nodes.GetAlternateLookup<TKey>();
            _edgeLookup = _edges.GetAlternateLookup<(TKey, TKey)>();
        }

        public void Clear()
        {
            _nodes.Clear();
            ClearEdges();
        }

        public void ClearEdges()
        {
            _edges.Clear();
        }

        public bool Contains(TKey key) => _nodeLookup.Contains(key);

        public bool TryGetNode(TKey key, [MaybeNullWhen(false)] out TNode node) => _nodeLookup.TryGetValue(key, out node);
        public bool NodeEquals(TKey key, TNode node) => _nodeComparer.Equals(key, node);

        public bool AddNode(TKey key, out TNode node)
        {
            if (_nodeLookup.TryGetValue(key, out node!))
            {
                return false;
            }
            node = _factory(key);
            _nodes.Add(node);
            return true;
        }

        public bool AddNode(TKey key) => AddNode(key, out _);

        public bool RemovNode(TKey key)
        {
            if (_nodeLookup.Remove(key))
            {
                _edges.RemoveWhere(edge => _edgeComparer.Contains(edge, key));
                return true;
            }
            return false;
        }

        public bool IsConnected(TKey from, TKey to) => _edgeLookup.Contains((from, to));
        public bool TryGetEdge(TKey from, TKey to, [MaybeNullWhen(false)] out Edge edge) => _edgeLookup.TryGetValue((from, to), out edge);

        public bool AddEdge(TKey from, TKey to, string? name, [MaybeNullWhen(false)] out Edge edge, out bool renamed)
        {
            renamed = false;
            if (SetEdgeName(from, to, name, out edge))
            {
                renamed = true;
                return true;
            }
            var n = _nodeLookup;
            if (n.TryGetValue(from, out var fromNode) && n.TryGetValue(to, out var toNode))
            {
                edge = new(name, fromNode, toNode);
                _edges.Add(edge);
                return true;
            }
            return false;
        }

        public bool AddEdge(TKey from, TKey to, string? name = null) => AddEdge(from, to, name, out _, out _);

        public bool SetEdgeName(TKey from, TKey to, string? name, [NotNullWhen(true)] out Edge? edge)
        {
            if (_edgeLookup.TryGetValue((from, to), out edge))
            {
                edge.Name = name;
                return true;
            }
            return false;
        }

        public bool RemoveEdge(TKey from, TKey to) => _edgeLookup.Remove((from, to));

        public int RemoveEdges(TKey containingKey) => _edges.RemoveWhere(edge => _edgeComparer.Contains(edge, containingKey));

        bool IGraph<TKey>.TryGetNode(TKey key, [MaybeNullWhen(false)] out IGraphNode<TKey> node)
        {
            if (TryGetNode(key, out var n))
            {
                node = n;
                return true;
            }
            node = default;
            return false;
        }

        bool IGraph.TryGetNode(object value, [MaybeNullWhen(false)] out IGraphNode node)
        {
            if (value is TKey v && TryGetNode(v, out var n))
            {
                node = n;
                return true;
            }
            node = default;
            return false;
        }

        bool IGraph.NodeEquals(object value, IGraphNode node) => value is TKey v1 && node.ObjectKey is TKey v2 && _keyComparer.Equals(v1, v2);

        public NodeEnumerable EnumerateNodes() => new(this);
        public EdgeEnumerable EnumerateEdges() => new(this);

        public IEnumerable<TNode> EnumerateConnectedNodes(TKey value)
        {
            var c = _nodeComparer;
            foreach (var edge in _edges)
            {
                if (c.Equals(value, edge.From))
                {
                    yield return edge.To;
                }
            }
        }

        IEnumerable<IGraphNode> IGraph.EnumerateNodes()
        {
            foreach (var node in EnumerateNodes())
            {
                yield return node;
            }
        }

        IEnumerable<IGraphEdge> IGraph.EnumerateEdges() => EnumerateEdges();

        IEnumerable<IGraphNode> IGraph.EnumerateConnectedNodes(IGraphNode node)
        {
            if (node.ObjectKey is TKey key)
            {
                foreach (var n in EnumerateConnectedNodes(key))
                {
                    yield return n;
                }
            }
        }

        public class Edge(string? name, TNode from, TNode to) : IGraphEdge
        {
            public string? Name { get; set; } = name;
            public TNode From { get; } = from;
            public TNode To { get; } = to;

            public (TKey, TKey) Keys => (From.Key, To.Key);

            IGraphNode IGraphEdge.From => From;
            IGraphNode IGraphEdge.To => To;
        }

        private class NodeEqualityComparer(IEqualityComparer<TKey> comparer) : IEqualityComparer<TNode>, IAlternateEqualityComparer<TKey, TNode>
        {
            internal readonly IEqualityComparer<TKey> _comparer = comparer;

            public TNode Create(TKey alternate) => throw new NotImplementedException();

            public bool Equals(TNode? x, TNode? y) => x is null ? y is null : y is not null && Equals(_comparer, x.Key, y.Key);
            public bool Equals(TKey alternate, TNode other) => Equals(_comparer, alternate, other.Key);
            public static bool Equals(IEqualityComparer<TKey> comparer, TKey x, TKey y) => comparer.Equals(x, y);

            public int GetHashCode([DisallowNull] TNode obj) => GetHashCode(obj.Key);
            public int GetHashCode(TKey alternate) => _comparer.GetHashCode(alternate);
        }

        private class EdgeEqualityComparer(bool undirected, IEqualityComparer<TKey> comparer) : IEqualityComparer<Edge>, IAlternateEqualityComparer<(TKey, TKey), Edge>
        {
            internal readonly bool _undirected = undirected;
            internal readonly IEqualityComparer<TKey> _comparer = comparer;

            public Edge Create((TKey, TKey) alternate) => throw new NotImplementedException();

            public bool Equals(Edge? x, Edge? y) => x is null ? y is null : y is not null && Equals(_comparer, _undirected, x.Keys, y.Keys);
            public bool Equals((TKey, TKey) alternate, Edge other) => Equals(_comparer, _undirected, alternate, other.Keys);

            public static bool Equals(IEqualityComparer<TKey> comparer, bool undirected, (TKey, TKey) x, (TKey, TKey) y)
            {
                return (comparer.Equals(x.Item1, y.Item1) && comparer.Equals(x.Item2, y.Item2)) || 
                    (undirected && comparer.Equals(x.Item1, y.Item2) && comparer.Equals(x.Item2, y.Item1));
            }

            public int GetHashCode([DisallowNull] Edge obj) => GetHashCode(obj.Keys);

            public int GetHashCode((TKey, TKey) alternate)
            {
                var c = _comparer;
                var h1 = c.GetHashCode(alternate.Item1);
                var h2 = c.GetHashCode(alternate.Item2);
                return _undirected ? h1 ^ h2 : HashCode.Combine(h1, h2);
            }

            public bool Contains(Edge edge, TKey value) => _comparer.Equals(edge.From.Key, value) || _comparer.Equals(edge.To.Key, value);
        }

        public readonly struct NodeEnumerable(Graph<TKey, TNode> graph) : ISafeEnumerable<TNode>
        {
            private readonly Graph<TKey, TNode> _graph = graph;
            public HashSet<TNode>.Enumerator GetEnumerator() => _graph._nodes.GetEnumerator();
            IEnumerator<TNode> IEnumerable<TNode>.GetEnumerator() => GetEnumerator();
        }

        public readonly struct EdgeEnumerable(Graph<TKey, TNode> graph) : ISafeEnumerable<Edge>
        {
            private readonly Graph<TKey, TNode> _graph = graph;
            public HashSet<Edge>.Enumerator GetEnumerator() => _graph._edges.GetEnumerator();
            IEnumerator<Edge> IEnumerable<Edge>.GetEnumerator() => GetEnumerator();
        }
    }
}
