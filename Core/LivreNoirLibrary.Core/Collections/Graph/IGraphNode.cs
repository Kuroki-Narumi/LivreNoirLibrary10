using System;

namespace LivreNoirLibrary.Collections
{
    public interface IGraphNode
    {
        string? Name { get; }
        object ObjectKey { get; }
    }

    public interface IGraphNode<T> : IGraphNode
        where T : notnull
    {
        T Key { get; }

        object IGraphNode.ObjectKey => Key;
    }
}
