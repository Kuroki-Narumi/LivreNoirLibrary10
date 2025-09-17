using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    public class GraphViewNode(GraphNode node)
    {
        public GraphNode Node { get; } = node;
        public string Name => Node.Name;
        public double X { get; set; }
        public double Y { get; set; }

        public bool ContainsPoint(double x, double y, double th)
        {
            var dx = x - X;
            var dy = y - Y;
            return (dx * dx + dy * dy) < th;
        }
    }
}
