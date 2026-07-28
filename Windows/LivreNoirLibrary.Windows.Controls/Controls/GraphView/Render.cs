using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    partial class GraphView
    {
        private readonly TranslateTransform _transform = new();

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var needPop = false;
            if (OffsetX != 0 || OffsetY != 0)
            {
                needPop = true;
                _transform.X = OffsetX;
                _transform.Y = OffsetY;
                drawingContext.PushTransform(_transform);
            }

            DrawEdges(drawingContext);
            DrawConnectedEdges(drawingContext);
            if (DisplaysEdgeName)
            {
                DrawEdgeNames(drawingContext);
            }

            DrawNodes(drawingContext);
            DrawConnectedNodes(drawingContext);
            DrawSelectedNode(drawingContext);
            if (DisplaysNodeName)
            {
                DrawNodeNames(drawingContext);
            }
            if (_movingElement is { } element && DisplaysElementPosition)
            {
                DrawPosition(drawingContext, element.X, element.Y);
            }

            if (needPop)
            {
                drawingContext.Pop();
            }
        }

        protected virtual void DrawEdges(DrawingContext dc)
        {
            var pen = MediaUtils.GetPen(EdgeBrush, EdgeThickness);
            foreach (var edge in _edges.AsSpan())
            {
                edge.EnsurePosition(FindNode);
                if (pen is not null)
                {
                    DrawEdge(dc, edge, pen);
                }
            }
        }

        protected virtual void DrawConnectedEdges(DrawingContext dc)
        {
            var th = SelectedEdgeThickness;
            if (MediaUtils.GetPen(SelectedEdgeBrush, th) is { } pen)
            {
                foreach (var edge in _connectedEdges)
                {
                    DrawEdge(dc, edge, pen);
                }
            }
        }

        protected void DrawEdge(DrawingContext dc, Edge edge, Pen pen)
        {
            dc.DrawLine(pen, new(edge.X1, edge.Y1), new(edge.X, edge.Y));
            dc.DrawLine(pen, new(edge.X, edge.Y), new(edge.X2, edge.Y2));
            var brush = edge.IsSelected ? pen.Brush : Brushes.Transparent;
            var r = EdgeKnobRadius;
            dc.DrawEllipse(brush, null, new(edge.X, edge.Y), r, r);
        }

        protected virtual void DrawEdgeNames(DrawingContext dc)
        {
            foreach (var edge in _connectedEdges)
            {
                DrawEdgeName(dc, edge);
            }
            if (_selectedViewEdge is not null)
            {
                DrawEdgeName(dc, _selectedViewEdge);
            }
        }

        protected virtual void DrawEdgeName(DrawingContext dc, Edge edge)
        {
            RenderText(dc, edge.X, edge.Y, edge.Name, EdgeNameFontSize.Validate(FontSize));
        }

        protected virtual void DrawNodes(DrawingContext dc)
        {
            var pen = MediaUtils.GetPen(NodeStroke, NodeStrokeThickness);
            var brush = NodeFill;
            if (brush is not null || pen is not null)
            {
                foreach (var (_, node) in _nodes)
                {
                    DrawNode(dc, node, brush, pen);
                }
            }
        }

        protected virtual void DrawConnectedNodes(DrawingContext dc)
        {
            var pen = MediaUtils.GetPen(ConnectedNodeStroke, NodeStrokeThickness);
            var brush = ConnectedNodeFill;
            if (brush is not null || pen is not null)
            {
                foreach (var node in _connectedNodes)
                {
                    DrawNode(dc, node, brush, pen);
                }
            }
        }

        protected virtual void DrawSelectedNode(DrawingContext dc)
        {
            var pen = MediaUtils.GetPen(SelectedNodeStroke, NodeStrokeThickness);
            var brush = SelectedNodeFill;
            if ((brush is not null || pen is not null) && _selectedViewNode is { } node)
            {
                DrawNode(dc, node, brush, pen);
            }
        }

        protected virtual void DrawNode(DrawingContext dc, Node node, Brush? brush, Pen? pen)
        {
            var r = NodeRadius;
            dc.DrawEllipse(brush, pen, new(node.X, node.Y), r, r);
        }

        private readonly Dictionary<int, List<Rect>> _namePositions = [];

        protected virtual void DrawNodeNames(DrawingContext dc)
        {
            var pos = _namePositions;
            var fontSize = FontSize;
            foreach (var (_, node) in _nodes)
            {
                DrawNodeName(dc, node, pos, fontSize);
            }
            pos.Clear();
        }

        protected virtual void DrawNodeName(DrawingContext dc, Node node, Dictionary<int, List<Rect>> positions, double fontSize)
        {
            var x = node.X;
            var intX = (int)Math.Round(x / HorizontalGrid);
            var rects = positions.GetOrAdd(intX);
            var y = node.Y;
            var rect = new Rect(intX - 10, y, 20, fontSize - 1);
            var intY = (int)Math.Round(y / fontSize / 2);
            while (rects.Any(rect.IntersectsWith))
            {
                y += fontSize;
                rect.Y += fontSize;
            }
            rects.Add(rect);
            RenderText(dc, x, y + NodeRadius, node.Name, va: VerticalAlignment.Top);
        }

        protected virtual void DrawPosition(DrawingContext dc, double x, double y)
        {
            RenderText(dc, x, y, $"({x:0},{y:0})");
        }
    }
}
