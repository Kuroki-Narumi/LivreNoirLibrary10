using System;

namespace LivreNoirLibrary.Collections
{
    public delegate void NotifyGraphEdgeChangedEventHandler(object? sender, NotifyGraphEdgeChangedEventArgs e);

    public class NotifyGraphEdgeChangedEventArgs(NotifyEdgeChangedAction action, GraphEdge? edge = null) : EventArgs
    {
        public NotifyEdgeChangedAction Action { get; } = action;
        public GraphEdge? Edge { get; } = edge;
    }

    public enum NotifyEdgeChangedAction
    {
        Add,
        Remove,
        Change,
        Reset,
    }
}
