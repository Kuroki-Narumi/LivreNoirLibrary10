using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Input;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    partial class GraphView
    {
        protected virtual void ClearSelection()
        {
            _connectedNodes.Clear();
            _connectedEdges.Clear();
        }

        protected virtual void UpdateSelection()
        {
            ClearSelection();
            // edges from selected nodes
            foreach (var edge in _edges.AsSpan())
            {
                if (FindNode(edge.From) is { } n1 && FindNode(edge.To) is { } n2)
                {
                    if (n1 == _selectedViewNode)
                    {
                        _connectedNodes.Add(n2);
                        _connectedEdges.Add(edge);
                    }
                    else if (n2 == _selectedViewNode)
                    {
                        _connectedNodes.Add(n1);
                        _connectedEdges.Add(edge);
                    }
                }
            }
            InvalidateVisual();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            var pos = e.GetPosition(this);
            SelectEdge(GetEdgeAt(pos.X, pos.Y));
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            SelectEdge(null);
            base.OnMouseLeave(e);
        }

        private void SelectEdge(Edge? edge)
        {
            if (edge != _selectedViewEdge)
            {
                _selectedViewEdge?.IsSelected = false;
                edge?.IsSelected = true;
                InvalidateVisual();
            }
            _selectedViewEdge = edge;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            CheckCtrlDown();
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            CheckCtrlDown();
            base.OnKeyUp(e);
        }

        private void CheckCtrlDown()
        {
            DisplaysElementPosition = KeyInput.IsCtrlDown();
        }

        private bool _elementMoving;
        private bool _elementMoved;
        private Element? _movingElement;
        private double _initialX;
        private double _initialY;
        private double _initialElementX;
        private double _initialElementY;
        private Action<double, double>? _moveAction;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var (x, y) = e.GetPosition(this);
            _initialX = x;
            _initialY = y;
            if (KeyInput.IsShiftDown())
            {
                _movingElement = null;
                _initialElementX = OffsetX;
                _initialElementY = OffsetY;
                _moveAction = (x, y) => (OffsetX, OffsetY) = (x, y);
            }
            else if (GetNodeAt(x, y) is { } node)
            {
                _movingElement = node;
                _initialElementX = node.X;
                _initialElementY = node.Y;
                _moveAction = (x, y) => (node.X, node.Y) = (x, y);
                SelectEdge(null);
            }
            else if (_selectedViewEdge is { } edge)
            {
                _movingElement = edge;
                _initialElementX = edge.X;
                _initialElementY = edge.Y;
                _moveAction = (x, y) => (edge.CenterX, edge.CenterY) = (x, y);
            }
            else
            {
                base.OnMouseLeftButtonDown(e);
                return;
            }
            CheckCtrlDown();
            CaptureMouse();
            _elementMoving = true;
            _elementMoved = false;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            _elementMoving = false;
            if (!_elementMoved && _movingElement is Node node)
            {
                if (Source is { } source && source.TryGetNode(node.Key, out var n) && SelectedNode != n)
                {
                    SelectedNode = n;
                }
                else
                {
                    SelectedNode = null;
                }
            }
            _movingElement = null;
            InvalidateVisual();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var (x, y) = e.GetPosition(this);
            if (_elementMoving)
            {
                var dx = x - _initialX;
                var dy = y - _initialY;
                CheckCtrlDown();
                var ctrl = DisplaysElementPosition;
                if (_elementMoved || (IsInteractive && (dx * dx + dy * dy) > MoveThreshold))
                {
                    _elementMoved = true;
                    var newX = _initialElementX + dx;
                    var newY = _initialElementY + dy;
                    if (ctrl)
                    {
                        var h = HorizontalGrid;
                        if (h > 1)
                        {
                            newX = Math.Round(newX / h) * h;
                        }
                        var v = VerticalGrid;
                        if (v > 1)
                        {
                            newY = Math.Round(newY / v) * v;
                        }
                    }
                    _moveAction?.Invoke(newX, newY);
                    InvalidateVisual();
                }
            }
            else
            {
                SelectEdge(GetEdgeAt(x, y));
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            if (_selectedViewEdge is { } edge)
            {
                edge.CenterX = double.NaN;
                edge.CenterY = double.NaN;
                InvalidateVisual();
            }
            base.OnMouseRightButtonUp(e);
        }
    }
}
