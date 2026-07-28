using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class GraphView : TextViewerBase
    {
        public const bool DefaultDisplaysNodeName = true;
        public const double DefaultNodeRadius = 20;
        public const double DefaultNodeStrokeThickness = 1;
        public static readonly SolidColorBrush DefaultNodeFill = MediaUtils.GetBrush(255, 255, 255, 255);
        public static readonly SolidColorBrush DefaultNodeStroke = MediaUtils.GetBrush(255, 0, 0, 0);
        public static readonly SolidColorBrush DefaultSelectedNodeFill = MediaUtils.GetBrush(255, 192, 192, 255);
        public static readonly SolidColorBrush DefaultSelectedNodeStroke = MediaUtils.GetBrush(255, 0, 0, 127);
        public static readonly SolidColorBrush DefaultConnectedNodeFill = MediaUtils.GetBrush(255, 192, 255, 192);
        public static readonly SolidColorBrush DefaultConnectedNodeStroke = MediaUtils.GetBrush(255, 0, 127, 0);

        public const bool DefaultDisplaysEdgeName = false;
        public const double DefaultEdgeNameFontSize = 12;
        public const double DefaultEdgeKnobRadius = 10;
        public static readonly SolidColorBrush DefaultEdgeBrush = MediaUtils.GetBrush(95, 0, 0, 0);
        public const double DefaultEdgeThickness = 2;
        public static readonly SolidColorBrush DefaultSelectedEdgeBrush = MediaUtils.GetBrush(191, 0, 127, 255);
        public const double DefaultSelectedEdgeThickness = 4;

        public const bool DefaultIsInteractive = true;
        public const double DefaultGrid = DefaultNodeRadius * 2;
        public const double MoveThreshold = 9.0;

        static GraphView()
        {
            PropertyUtils.OverrideDefaultStyleKey<GraphView>();
        }

        [DependencyProperty(AffectsRender = true)]
        private double _offsetX;
        [DependencyProperty(AffectsRender = true)]
        private double _offsetY;
        [DependencyProperty(AffectsRender = true)]
        private IGraph? _source;
        [DependencyProperty(AffectsRender = true)]
        private IGraphNode? _selectedNode;
        [DependencyProperty(AffectsRender = true)]
        private bool _displaysNodeName = DefaultDisplaysNodeName;
        [DependencyProperty(AffectsRender = true)]
        private double _nodeRadius = DefaultNodeRadius;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _nodeFill = DefaultNodeFill;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _nodeStroke = DefaultNodeStroke;
        [DependencyProperty(AffectsRender = true)]
        private double _nodeStrokeThickness = DefaultNodeStrokeThickness;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _selectedNodeFill = DefaultSelectedNodeFill;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _selectedNodeStroke = DefaultSelectedNodeStroke;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _connectedNodeFill = DefaultConnectedNodeFill;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _connectedNodeStroke = DefaultConnectedNodeStroke;
        [DependencyProperty(AffectsRender = true)]
        private bool _displaysEdgeName = DefaultDisplaysEdgeName;
        [DependencyProperty(AffectsRender = true)]
        private double _edgeNameFontSize = DefaultEdgeNameFontSize;
        [DependencyProperty(AffectsRender = true)]
        private double _edgeKnobRadius = DefaultEdgeKnobRadius;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _edgeBrush = DefaultEdgeBrush;
        [DependencyProperty(AffectsRender = true)]
        private double _edgeThickness = DefaultEdgeThickness;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _selectedEdgeBrush = DefaultSelectedEdgeBrush;
        [DependencyProperty(AffectsRender = true)]
        private double _selectedEdgeThickness = DefaultSelectedEdgeThickness;
        [DependencyProperty]
        private bool _isInteractive = DefaultIsInteractive;
        [DependencyProperty]
        private double _verticalGrid = DefaultGrid;
        [DependencyProperty]
        private double _horizontalGrid = DefaultGrid;
        [DependencyProperty(AffectsRender = true, SetterScope = Scope.Private)]
        private bool _displaysElementPosition;

        public double NodeDiameter { get => NodeRadius * 2; set => NodeRadius = value * 0.5; }

        private readonly ObjectCache<Node> _nodeCache = new(() => new());
        private readonly ObjectCache<Edge> _edgeCache = new(() => new());

        protected readonly Dictionary<IGraphNode, Node> _nodes = [];
        protected readonly List<Edge> _edges = [];

        private Node? _selectedViewNode;
        private Edge? _selectedViewEdge;
        private readonly HashSet<Node> _connectedNodes = [];
        private readonly HashSet<Edge> _connectedEdges = [];

        protected Node? SelectedViewNode => _selectedViewNode;
        protected Edge? SelectedViewEdge => _selectedViewEdge;

        protected virtual void OnSourceChanged(IGraph? oldValue, IGraph? newValue)
        {
            ReserveRefresh();
        }

        protected virtual void OnSelectedNodeChanged(IGraphNode? value)
        {
            _selectedViewNode = value is not null ? _nodes.GetValueOrDefault(value) : null;
            UpdateSelection();
        }

        protected override void Refresh()
        {
            base.Refresh();
            ClearFields();
            if (Source is { } source)
            {
                Refresh(source);
            }
        }

        protected virtual void ClearFields()
        {
            _nodeCache.Clear();
            _edgeCache.Clear();
            _nodes.Clear();
            _edges.Clear();
            SelectedNode = null;
            _selectedViewEdge = null;
        }

        protected virtual void Refresh(IGraph source)
        {
            var r = Math.Min(ActualWidth, ActualHeight) / 2 - NodeRadius - FontSize * 2;
            var count = source.Count;
            var i = 0;
            var nodes = _nodes;
            var nodeCache = _nodeCache;
            foreach (var node in source.EnumerateNodes())
            {
                var (x, y) = Math.SinCos(2.0 * Math.PI * i / count);
                var vn = nodeCache.GetNext();
                vn.Name = node.Name;
                vn.Key = node.ObjectKey;
                vn.X = r * x;
                vn.Y = r * -y;
                nodes[node] = vn;
                i++;
            }
            var edges = _edges;
            var edgeCache = _edgeCache;
            foreach (var edge in source.EnumerateEdges())
            {
                var ve = edgeCache.GetNext();
                ve.Name = edge.Name;
                ve.From = edge.From.ObjectKey;
                ve.To = edge.To.ObjectKey;
                edges.Add(ve);
            }
        }

        public Node? FindNode(object key)
        {
            if (Source is not { } graph || key is null)
            {
                return null;
            }
            if (graph.TryGetNode(key, out var node))
            {
                return _nodes.GetValueOrDefault(node);
            }
            return null;
        }

        public Node? GetNodeAt(double x, double y)
        {
            x -= OffsetX;
            y -= OffsetY;
            var r = NodeRadius;
            var r2 = r * r;
            Node? result = null;
            foreach (var (_, node) in _nodes)
            {
                if (node.Contains(x, y, r2))
                {
                    result = node;
                }
            }
            return result;
        }

        public Edge? GetEdgeAt(double x, double y)
        {
            x -= OffsetX;
            y -= OffsetY;
            var r = EdgeKnobRadius;
            var r2 = r * r;
            var span = _edges.AsSpan();
            for (var i = span.Length - 1; i >= 0; i--)
            {
                var edge = span[i];
                if (edge.Contains(x, y, r2))
                {
                    return edge;
                }
            }
            return null;
        }

        public bool IsConnected(Node node) => _connectedNodes.Contains(node);
    }
}
