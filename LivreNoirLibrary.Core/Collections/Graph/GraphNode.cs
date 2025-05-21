using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public class GraphNode(string name) : IEquatable<GraphNode>
    {
        public event NotifyGraphEdgeChangedEventHandler? EdgeChanged;

        public string Name { get; } = name;

        private readonly Dictionary<GraphNode, string> _edges = [];

        public void ClearEdges()
        {
            _edges.Clear();
            EdgeChanged?.Invoke(this, new(NotifyEdgeChangedAction.Reset));
        }

        public bool AddEdge(GraphNode target, string name = "")
        {
            if (_edges.TryGetValue(target, out var value))
            {
                if (value != name)
                {
                    _edges[target] = name;
                    EdgeChanged?.Invoke(this, new(NotifyEdgeChangedAction.Change, new(name, this, target)));
                }
                return false;
            }
            else
            {
                _edges.Add(target, name);
                EdgeChanged?.Invoke(this, new(NotifyEdgeChangedAction.Add, new(name, this, target)));
                return true;
            }
        }

        public bool RemoveEdge(GraphNode target)
        {
            if (_edges.TryGetValue(target, out var name))
            {
                _edges.Remove(target);
                EdgeChanged?.Invoke(this, new(NotifyEdgeChangedAction.Remove, new(name, this, target)));
                return true;
            }
            return false;
        }

        public void GetEdgeList(List<GraphEdge> list)
        {
            foreach (var (target, name) in _edges)
            {
                list.Add(new(name, this, target));
            }
        }

        public List<GraphEdge> GetEdgeList()
        {
            List<GraphEdge> list = [];
            GetEdgeList(list);
            return list;
        }

        public bool Equals(GraphNode? other) => Name == other?.Name;
        public override bool Equals(object? obj) => (obj as GraphNode)?.Name == Name;
        public override int GetHashCode() => Name.GetHashCode();
        public static bool operator ==(GraphNode? left, GraphNode? right) => left?.Name == right?.Name;
        public static bool operator !=(GraphNode? left, GraphNode? right) => left?.Name != right?.Name;
    }
}
