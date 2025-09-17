using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public class Graph : HashSet<GraphNode>
    {
        public event NotifyGraphNodeChangedEventHandler? NodeChanged;
        public event NotifyGraphEdgeChangedEventHandler? EdgeChanged;

        public new void Clear()
        {
            if (Count > 0)
            {
                foreach (var node in this)
                {
                    node.EdgeChanged -= OnEdgeChanged;
                }
                base.Clear();
                NodeChanged?.Invoke(this, new(NotifyNodeChangedAction.Reset));
            }
        }

        public new bool Add(GraphNode node)
        {
            if (base.Add(node))
            {
                node.EdgeChanged += OnEdgeChanged;
                NodeChanged?.Invoke(this, new(NotifyNodeChangedAction.Add, node));
                return true;
            }
            return false;
        }

        public bool Add(string name)
        {
            return Add(new GraphNode(name));
        }

        public new bool Remove(GraphNode node)
        {
            if (TryGetValue(node, out var current))
            {
                base.Remove(current);
                current.EdgeChanged -= OnEdgeChanged;
                foreach (var n in this)
                {
                    n.RemoveEdge(current);
                }
                NodeChanged?.Invoke(this, new(NotifyNodeChangedAction.Remove, current));
                return true;
            }
            return false;
        }

        public bool Remove(string name)
        {
            return Remove(new GraphNode(name));
        }

        public bool AddEdge(string start, string end, string name = "")
        {
            if (TryGetValue(new(start), out var s) && TryGetValue(new(end), out var e))
            {
                return AddEdge(s, e, name);
            }
            return false;
        }

        public bool RemoveEdge(string start, string end)
        {
            if (TryGetValue(new(start), out var s) && TryGetValue(new(end), out var e))
            {
                return RemoveEdge(s, e);
            }
            return false;
        }

        private void OnEdgeChanged(object? sender, NotifyGraphEdgeChangedEventArgs e)
        {
            EdgeChanged?.Invoke(sender, e);
        }

        public static bool AddEdge(GraphNode start, GraphNode end, string name = "")
        {
            return start.AddEdge(end, name);
        }

        public static bool RemoveEdge(GraphNode start, GraphNode end)
        {
            return start.RemoveEdge(end);
        }

        public List<GraphEdge> GetEdgeList()
        {
            List<GraphEdge> list = [];
            foreach (var node in this)
            {
                node.GetEdgeList(list);
            }
            return list;
        }
    }
}
