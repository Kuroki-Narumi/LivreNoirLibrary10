using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate GraphViewNode? GraphViewNodeGetter(GraphNode node);

    public partial class GraphView : TextViewerBase
    {
        public static readonly SolidColorBrush DefaultTextOutline = MediaUtils.GetBrush(255, 255, 255, 255);
        public const double DefaultTextOutlineThickness = 4;

        static GraphView()
        {
            TextOutlineProperty.OverrideMetadata(typeof(GraphView), PropertyUtils.GetMetaTwoWay(DefaultTextOutline, PropertyUtils.OnVisualPropertyChanged));
            TextOutlineThicknessProperty.OverrideMetadata(typeof(GraphView), PropertyUtils.GetMetaTwoWay(DefaultTextOutlineThickness, PropertyUtils.OnVisualPropertyChanged));
        }

        protected readonly Dictionary<GraphNode, GraphViewNode> _node_list = [];
        protected readonly List<GraphViewEdge> _edge_list = [];

        protected GraphViewNode? _selected_viewNode;
        protected GraphViewEdge? _selected_edge;
        protected readonly HashSet<GraphViewNode> _connected_nodes = [];
        protected readonly HashSet<GraphViewEdge> _connected_edges = [];

        protected bool _element_moving;
        protected double _init_x;
        protected double _init_y;
        protected double _init_element_x;
        protected double _init_element_y;
        protected bool _element_moved;
        protected bool _element_position_draw;
        protected GraphViewNode? _moving_node;
        protected Action<double, double>? _moving_action;

        protected override void Refresh()
        {
            ClearFields();
            if (_source is not null)
            {
                ProcessRefresh(_source);
            }
        }

        protected virtual void ClearFields()
        {
            SelectedNode = null;
            _node_list.Clear();
            _edge_list.Clear();
            _selected_edge = null;
            _connected_nodes.Clear();
            _connected_edges.Clear();
        }

        protected virtual void ProcessRefresh(Graph source)
        {
            _offsetX = ActualWidth / 2;
            _offsetY = ActualHeight / 2;
            var r = Math.Min(_offsetX, _offsetY) - _nodeRadius - FontSize * 2;
            var max = source.Count;
            int i = 0;
            foreach (var node in source)
            {
                var (x, y) = Math.SinCos(2.0 * Math.PI * i / max);
                GraphViewNode vn = new(node)
                {
                    X = r * x,
                    Y = r * y,
                };
                _node_list.Add(node, vn);
                i++;
            }
            foreach (var edge in source.GetEdgeList())
            {
                _edge_list.Add(new(edge));
            }
        }

        protected GraphViewNode? GetViewNode(GraphNode? node) => node is not null && _node_list.TryGetValue(node, out var nv) ? nv : null;

        protected GraphViewNode? NodeAt(double x, double y)
        {
            x -= _offsetX;
            y -= _offsetY;
            var th = _nodeRadius;
            th *= th;
            foreach (var (_, node) in _node_list.Reverse())
            {
                if (node.ContainsPoint(x, y, th))
                {
                    return node;
                }
            }
            return null;
        }

        protected GraphViewEdge? EdgeAt(double x, double y)
        {
            x -= _offsetX;
            y -= _offsetY;
            var th = _edgeKnobRadius;
            th *= th;
            for (int i = _edge_list.Count - 1; i >= 0; i--)
            {
                var edge = _edge_list[i];
                if (edge.ContainsPoint(x, y, th))
                {
                    return edge;
                }
            }
            return null;
        }
    }
}
