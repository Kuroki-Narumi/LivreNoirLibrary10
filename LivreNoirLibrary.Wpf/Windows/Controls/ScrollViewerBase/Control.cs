using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ScrollViewerBase
    {
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
            if (e.ChangedButton is MouseButton.Middle)
            {
                OnMouseMiddleButtonDown(e);
            }
        }

        protected virtual void OnMouseMiddleButtonDown(MouseButtonEventArgs e)
        {
            if (!e.Handled && e.ClickCount is 1)
            {
                StartMiddleScroll(e);
            }
        }

        private void StartMiddleScroll(MouseButtonEventArgs e)
        {
            var type = ScrollIconType.None;
            if (_main_canvas.Width > ViewportWidth)
            {
                type |= ScrollIconType.Horizontal;
            }
            if (_main_canvas.Height > ViewportHeight)
            {
                type |= ScrollIconType.Vertical;
            }
            if (type is not ScrollIconType.None)
            {
                e.Handled = true;
                var icon = _scroll_icon;
                var pos = e.GetPosition(_fixed_canvas);
                icon.IconType = type;
                Canvas.SetLeft(icon, pos.X);
                Canvas.SetTop(icon, pos.Y);

                var moving = false;

                void mouseMove(object? sender, EventArgs e)
                {
                    var newPos = Mouse.GetPosition(_fixed_canvas);
                    var dx = newPos.X - pos.X;
                    dx = dx is > 0 ? Math.Max(dx - 14, 0) : Math.Min(dx + 14, 0);
                    var dy = newPos.Y - pos.Y;
                    dy = dy is > 0 ? Math.Max(dy - 14, 0) : Math.Min(dy + 14, 0);
                    if (dx is not 0)
                    {
                        moving = true;
                        ScrollToHorizontalOffset(HorizontalOffset + dx);
                    }
                    if (dy is not 0)
                    {
                        moving = true;
                        ScrollToVerticalOffset(VerticalOffset + dy);
                    }
                }

                void mouseUp(object sender, MouseButtonEventArgs e)
                {
                    if (moving)
                    {
                        moveEnd(sender, e);
                    }
                }

                void moveEnd(object sender, MouseButtonEventArgs e)
                {
                    icon.IconType = ScrollIconType.None;
                    ReleaseMouseCapture();
                    CompositionTarget.Rendering -= mouseMove;
                    PreviewMouseDown -= moveEnd;
                    MouseUp -= mouseUp;
                    if (e.ClickCount is 1)
                    {
                        e.Handled = true;
                    }
                }

                CaptureMouse();
                CompositionTarget.Rendering += mouseMove;
                PreviewMouseDown += moveEnd;
                MouseUp += mouseUp;
            }
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                this.StartScroll(e);
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if (!e.Handled && ProcessWheel(e.Delta, KeyInput.IsCtrlDown(), KeyInput.IsShiftDown()))
            {
                e.Handled = true;
            }
            else
            {
                base.OnPreviewMouseWheel(e);
            }
        }

        protected virtual bool ProcessWheel(int delta, bool ctrl, bool shift) => false;

    }
}
