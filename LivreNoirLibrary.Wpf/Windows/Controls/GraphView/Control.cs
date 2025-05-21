using System;
using System.Windows.Input;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class GraphView
    {
        private void OnNodeChanged(object? sender, NotifyGraphNodeChangedEventArgs e)
        {
            var node = e.Node;
            switch (e.Action)
            {
                case NotifyNodeChangedAction.Add:
                    if (node is not null)
                    {
                        OnNodeAdded(node);
                    }
                    break;
                case NotifyNodeChangedAction.Remove:
                    if (node is not null)
                    {
                        OnNodeRemoved(node);
                    }
                    break;
                case NotifyNodeChangedAction.Reset:
                    OnNodeCleared();
                    break;
            }
            UpdateSelection();
        }

        protected virtual void OnNodeAdded(GraphNode added)
        {
            _node_list.Add(added, new(added));
        }

        protected virtual void OnNodeRemoved(GraphNode removed)
        {
            if (_node_list.TryGetValue(removed, out var vn))
            {
                _node_list.Remove(removed);
                if (vn == _selected_viewNode)
                {
                    SelectedNode = null;
                }
            }
        }

        protected virtual void OnNodeCleared()
        {
            _node_list.Clear();
            SelectedNode = null;
        }

        private void OnEdgeChanged(object? sender, NotifyGraphEdgeChangedEventArgs e)
        {
            var edge = e.Edge;
            switch (e.Action)
            {
                case NotifyEdgeChangedAction.Add:
                    if (edge is not null)
                    {
                        OnEdgeAdded(edge);
                    }
                    break;
                case NotifyEdgeChangedAction.Remove:
                    if (edge is not null)
                    {
                        OnEdgeRemoved(edge);
                    }
                    break;
                case NotifyEdgeChangedAction.Change:
                    if (edge is not null)
                    {
                        OnEdgeChanged(edge);
                    }
                    break;
                case NotifyEdgeChangedAction.Reset:
                    OnEdgeCleared(sender as GraphNode);
                    break;
            }
            UpdateSelection();
        }

        protected virtual void OnEdgeAdded(GraphEdge added)
        {
            _edge_list.Add(new(added));
        }

        protected virtual void OnEdgeRemoved(GraphEdge removed)
        {
            for (int i = 0; i < _edge_list.Count; i++)
            {
                var ve = _edge_list[i];
                if (ve.Start == removed.Start && ve.End == removed.End)
                {
                    if (_selected_edge == ve)
                    {
                        _selected_edge = null;
                    }
                    _edge_list.RemoveAt(i);
                    break;
                }
            }
        }

        protected virtual void OnEdgeChanged(GraphEdge changed)
        {
            for (int i = 0; i < _edge_list.Count; i++)
            {
                var ve = _edge_list[i];
                if (ve.Start == changed.Start && ve.End == changed.End)
                {
                    GraphViewEdge edge = new(changed);
                    _edge_list[i] = edge;
                    _selected_edge = edge;
                    break;
                }
            }
        }

        protected virtual void OnEdgeCleared(GraphNode? node)
        {
            if (node is not null)
            {
                int i = 0;
                while (i < _edge_list.Count)
                {
                    var ve = _edge_list[i];
                    if (ve.Start == node)
                    {
                        if (_selected_edge == ve)
                        {
                            _selected_edge = null;
                        }
                        _edge_list.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            var pos = e.GetPosition(this);
            SelectEdge(EdgeAt(pos.X, pos.Y));
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            SelectEdge(null);
            base.OnMouseLeave(e);
        }

        private bool CheckIsCtrlDown()
        {
            var ctrl = KeyInput.IsCtrlDown();
            if (!_element_position_draw && ctrl)
            {
                _element_position_draw = true;
                InvalidateVisual();
            }
            return ctrl;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var pos = e.GetPosition(this);
            if (_element_moving)
            {
                var dx = pos.X - _init_x;
                var dy = pos.Y - _init_y;
                var ctrl = CheckIsCtrlDown();
                if (_element_moved || (_isInteractive && (dx * dx + dy * dy) > MoveThreshold))
                {
                    _element_moved = true;
                    var newX = dx + _init_element_x;
                    var newY = dy + _init_element_y;
                    if (ctrl)
                    {
                        if (_horizontalGrid > 1)
                        {
                            newX = Math.Round(newX / _horizontalGrid) * _horizontalGrid;
                        }
                        if (_verticalGrid > 1)
                        {
                            newY = Math.Round(newY / _verticalGrid) * _verticalGrid;
                        }
                    }
                    _moving_action?.Invoke(newX, newY);
                    InvalidateVisual();
                }
            }
            else
            {
                SelectEdge(EdgeAt(pos.X, pos.Y));
            }
            base.OnMouseMove(e);
        }

        private void SelectEdge(GraphViewEdge? edge)
        {
            if (edge != _selected_edge)
            {
                if (_selected_edge is not null)
                {
                    _selected_edge.IsSelected = false;
                }
                if (edge is not null)
                {
                    edge.IsSelected = true;
                }
                InvalidateVisual();
            }
            _selected_edge = edge;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(this);
            _init_x = pos.X;
            _init_y = pos.Y;
            if (KeyInput.IsShiftDown())
            {
                _moving_node = null;
                _init_element_x = _offsetX;
                _init_element_y = _offsetY;
                _moving_action = (x, y) => (OffsetX, OffsetY) = (x, y);
            }
            else if (NodeAt(_init_x, _init_y) is GraphViewNode node)
            {
                _moving_node = node;
                _init_element_x = node.X;
                _init_element_y = node.Y;
                _moving_action = (x, y) => (node.X, node.Y) = (x, y);
            }
            else if (_selected_edge is not null)
            {
                _moving_node = null;
                _init_element_x = _selected_edge.CX;
                _init_element_y = _selected_edge.CY;
                _moving_action = (x, y) => (_selected_edge.CX, _selected_edge.CY) = (x, y);
            }
            else
            {
                return;
            }
            CheckIsCtrlDown();
            CaptureMouse();
            _element_moving = true;
            _element_moved = false;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            _element_moving = false;
            _element_position_draw = false;
            if (!_element_moved && _moving_node is not null)
            {
                SelectedNode = _moving_node == _selected_viewNode ? null : _moving_node.Node;
            }
            InvalidateVisual();
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            if (_selected_edge is not null)
            {
                _selected_edge.CX = double.NaN;
                _selected_edge.CY = double.NaN;
                InvalidateVisual();
            }
            base.OnMouseRightButtonUp(e);
        }

        protected virtual void ClearSelection()
        {
            _connected_nodes.Clear();
            _connected_edges.Clear();
        }

        protected virtual void UpdateSelection()
        {
            ClearSelection();
            // edges from selected nodes
            for (int i = 0; i < _edge_list.Count; i++)
            {
                var edge = _edge_list[i];
                if (GetViewNode(edge.Start) is GraphViewNode n1 && GetViewNode(edge.End) is GraphViewNode n2)
                {
                    if (n1 == _selected_viewNode)
                    {
                        _connected_nodes.Add(n2);
                        _connected_edges.Add(edge);
                    }
                    else if (n2 == _selected_viewNode)
                    {
                        _connected_nodes.Add(n1);
                        _connected_edges.Add(edge);
                    }
                }
            }
            InvalidateVisual();
        }
    }
}
