using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class GraphView
    {
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            // edge
            RenderEdges(dc);
            RenderConnectedEdges(dc);
            if (IsEdgeNameVisible)
            {
                RenderEdgeNames(dc);
            }
            // node
            RenderNodes(dc);
            RenderConnectedNodes(dc);
            RenderSelectedNode(dc);
            if (IsNodeNameVisible)
            {
                RenderNodeNames(dc);
            }
            if (_element_moving && _element_position_draw)
            {
                if (_moving_node is GraphViewNode n)
                {
                    RenderPosition(dc, n.X, n.Y);
                }
                else if (_selected_edge is GraphViewEdge e)
                {
                    RenderPosition(dc, e.CX, e.CY);
                }
            }
        }

        protected virtual void RenderEdges(DrawingContext dc)
        {
            var pen = MediaUtils.GetPen(EdgeBrush, EdgeThickness);
            for (int i = 0; i < _edge_list.Count; i++)
            {
                var edge = _edge_list[i];
                edge.PrepareRender(GetViewNode, _offsetX, _offsetY);
                if (pen is not null)
                {
                    RenderEdge(dc, edge, pen);
                }
            }
        }

        protected virtual void RenderConnectedEdges(DrawingContext dc)
        {
            if (_connected_edges.Count > 0)
            {
                var th = SelectedEdgeThickness;
                if (MediaUtils.GetPen(SelectedEdgeBrush, th) is Pen pen)
                {
                    foreach (var edge in _connected_edges)
                    {
                        RenderEdge(dc, edge, pen);
                    }
                }
            }
        }

        protected virtual void RenderEdge(DrawingContext dc, GraphViewEdge edge, Pen pen)
        {
            if (edge.Geometry is Geometry sg)
            {
                dc.DrawGeometry(null, pen, sg);
                var brush = edge.IsSelected ? pen.Brush : Brushes.Transparent;
                dc.DrawEllipse(brush, null, new(edge.CX + _offsetX, edge.CY + _offsetY), _edgeKnobRadius, _edgeKnobRadius);
            }
        }

        protected virtual void RenderEdgeNames(DrawingContext dc)
        {
            foreach (var edge in _connected_edges)
            {
                RenderEdgeName(dc, edge);
            }
            if (_selected_edge is not null)
            {
                RenderEdgeName(dc, _selected_edge);
            }
        }

        protected virtual void RenderEdgeName(DrawingContext dc, GraphViewEdge edge)
        {
            RenderText(dc, edge.CX + _offsetX, edge.CY + _offsetY, edge.Name, _edgeNameFontSize);
        }
        
        protected virtual void RenderNodes(DrawingContext dc)
        {
            var pen = MediaUtils.GetPen(NodeStroke, _nodeStrokeThickness);
            var brush = NodeFill;
            if (brush is not null || pen is not null)
            {
                foreach (var (_, node) in _node_list)
                {
                    RenderNode(dc, node, brush, pen);
                }
            }
        }

        protected virtual void RenderConnectedNodes(DrawingContext dc)
        {
            if (_connected_nodes.Count > 0)
            {
                var pen = MediaUtils.GetPen(ConnectedNodeStroke, _nodeStrokeThickness);
                var brush = ConnectedNodeFill;
                if (brush is not null || pen is not null)
                {
                    foreach (var node in _connected_nodes)
                    {
                        RenderNode(dc, node, brush, pen);
                    }
                }
            }
        }

        protected virtual void RenderSelectedNode(DrawingContext dc)
        {
            if (_selected_viewNode is not null)
            {
                var pen = MediaUtils.GetPen(SelectedNodeStroke, _nodeStrokeThickness);
                var brush = SelectedNodeFill;
                if (brush is not null || pen is not null)
                {
                    RenderNode(dc, _selected_viewNode, brush, pen);
                }
            }
        }

        protected virtual void RenderNode(DrawingContext dc, GraphViewNode node, Brush? fill, Pen? stroke)
        {
            dc.DrawEllipse(fill, stroke, new(node.X + _offsetX, node.Y + _offsetY), _nodeRadius, _nodeRadius);
        }

        private readonly Dictionary<int, List<Rect>> _name_pos = [];
        private double _fsize;
        protected virtual void RenderNodeNames(DrawingContext dc)
        {
            _name_pos.Clear();
            _fsize = FontSize;
            foreach (var (_, node) in _node_list)
            {
                RenderNodeName(dc, node);
            }
        }

        protected virtual void RenderNodeName(DrawingContext dc, GraphViewNode node)
        {
            var x = node.X;
            var intX = (int)Math.Round(x / _horizontalGrid);
            if (!_name_pos.TryGetValue(intX, out var list))
            {
                list = [];
                _name_pos.Add(intX, list);
            }
            var y = node.Y;
            Rect rect = new(intX - 10, y, 20, _fsize - 1);
            var intY = (int)Math.Round(y / _fsize / 2);
            while (list.Any(r => r.IntersectsWith(rect)))
            {
                y += _fsize;
                rect.Y += _fsize;
            }
            list.Add(rect);
            RenderText(dc, x + _offsetX, y + _offsetY + _nodeRadius, node.Name, va: VerticalAlignment.Top);
        }

        protected void RenderPosition(DrawingContext dc, double x, double y)
        {
            RenderText(dc, x + _offsetX, y + _offsetY, $"({x:0},{y:0})");
        }
    }
}
