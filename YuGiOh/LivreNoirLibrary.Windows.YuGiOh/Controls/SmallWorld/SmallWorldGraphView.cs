using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class SmallWorldGraphView : GraphView
    {
        public const bool DefaultDisplaysConnected = true;
        public static readonly SolidColorBrush DefaultConnectedEdgeBrush = MediaUtils.GetBrush(95, 0, 255, 0);
        public const double DefaultConnectedEdgeThickness = 4;

        [DependencyProperty(AffectsRender = true)]
        private bool _displaysConnected = DefaultDisplaysConnected;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _connectedEdgeBrush = DefaultConnectedEdgeBrush;
        [DependencyProperty(AffectsRender = true)]
        private double _connectedEdgeThickness = DefaultConnectedEdgeThickness;

        private readonly SmallWorldGraph _graph = new();
        private readonly List<StatusKey> _keyBuffer = [];
        private readonly HashSet<Node> _directedNodes = [];
        private readonly HashSet<Edge> _directedEdges = [];

        public SmallWorldGraphView()
        {
            Source = _graph;
        }

        public void LoadCards(ICardEnumerable? source)
        {
            _graph.Build(source, _keyBuffer);
            ReserveRefresh();
        }

        protected override void ClearSelection()
        {
            base.ClearSelection();
            _directedNodes.Clear();
            _directedEdges.Clear();
        }

        protected override void DrawConnectedEdges(DrawingContext dc)
        {
            if (DisplaysConnected && MediaUtils.GetPen(ConnectedEdgeBrush, ConnectedEdgeThickness) is { } pen)
            {
                foreach (var edge in _directedEdges)
                {
                    DrawEdge(dc, edge, pen);
                }
            }
            base.DrawConnectedEdges(dc);
        }

        protected override void DrawEdgeNames(DrawingContext dc)
        {
            if (DisplaysConnected)
            {
                foreach (var edge in _directedEdges)
                {
                    DrawEdgeName(dc, edge);
                }
            }
            base.DrawEdgeNames(dc);
        }

        protected override void DrawConnectedNodes(DrawingContext dc)
        {
            base.DrawConnectedNodes(dc);
            var brush = ConnectedNodeFill;
            var pen = MediaUtils.GetPen(ConnectedNodeStroke, NodeStrokeThickness);
            if (DisplaysConnected && (brush is not null || pen is not null))
            {
                var r = NodeRadius / 2;
                foreach (var node in _directedNodes)
                {
                    dc.DrawEllipse(brush, pen, new(node.X, node.Y), r, r);
                }
            }
        }

        protected override void UpdateSelection()
        {
            base.UpdateSelection();
            var dn = _directedNodes;
            var de = _directedEdges;
            foreach (var edge in _edges.AsSpan())
            {
                if (FindNode(edge.From) is { } n1 && FindNode(edge.To) is { } n2)
                {
                    if (IsConnected(n1) && n2 != SelectedViewNode)
                    {
                        dn.Add(n2);
                        de.Add(edge);
                    }
                    if (IsConnected(n2) && n1 != SelectedViewNode)
                    {
                        dn.Add(n1);
                        de.Add(edge);
                    }
                }
            }
        }
    }
}
