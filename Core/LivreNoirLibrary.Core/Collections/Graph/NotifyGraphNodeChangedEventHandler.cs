using System;

namespace LivreNoirLibrary.Collections
{
    public delegate void NotifyGraphNodeChangedEventHandler(object? sender, NotifyGraphNodeChangedEventArgs e);

    public class NotifyGraphNodeChangedEventArgs(NotifyNodeChangedAction action, GraphNode? node = null) : EventArgs
    {
        public NotifyNodeChangedAction Action { get; } = action;
        public GraphNode? Node { get; } = node;
    }

    public enum NotifyNodeChangedAction
    {
        Add,
        Remove,
        Reset,
    }
}
