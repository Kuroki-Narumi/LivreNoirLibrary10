using System;

namespace LivreNoirLibrary.Collections
{
    public interface IGraphEdge
    {
        string? Name { get; }
        IGraphNode From { get; }
        IGraphNode To { get; }
    }
}
