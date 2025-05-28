using System;
using System.Windows.Input;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public static partial class ControlExtension
    {
        public static void HandleHorizontalWheel(this ScrollViewer viewer, MouseWheelEventArgs e)
        {
            var offset = viewer.HorizontalOffset;
            var newOffset = Math.Clamp(offset + e.Delta, 0, viewer.ScrollableWidth);
            viewer.ScrollToHorizontalOffset(newOffset);
            e.Handled = newOffset != offset;
        }

        public static void AutoScroll(this ScrollViewer viewer)
        {
            var pos = Mouse.GetPosition(viewer);
            AutoScroll_Horizontal(viewer, pos.X);
            AutoScroll_Vertical(viewer, pos.Y);
        }

        public static void AutoScroll(this ScrollViewer viewer, MouseEventArgs e)
        {
            var pos = e.GetPosition(viewer);
            AutoScroll_Horizontal(viewer, pos.X);
            AutoScroll_Vertical(viewer, pos.Y);
        }

        public static void AutoScroll_Horizontal(this ScrollViewer viewer) => AutoScroll_Horizontal(viewer, Mouse.GetPosition(viewer).X);

        public static void AutoScroll_Horizontal(this ScrollViewer viewer, double x)
        {
            double dif;
            if (x is < 0)
            {
                dif = x - 15;
            }
            else
            {
                x -= viewer.ViewportWidth;
                if (x is > 0)
                {
                    dif = x + 15;
                }
                else
                {
                    return;
                }
            }
            viewer.ScrollToHorizontalOffset(viewer.HorizontalOffset + Math.Truncate(dif / 16));
        }

        public static void AutoScroll_Vertical(this ScrollViewer viewer) => AutoScroll_Vertical(viewer, Mouse.GetPosition(viewer).Y);

        public static void AutoScroll_Vertical(this ScrollViewer viewer, double y)
        {
            double dif;
            if (y < 0)
            {
                dif = y - 15;
            }
            else
            {
                y -= viewer.ViewportHeight;
                if (y is > 0)
                {
                    dif = y + 15;
                }
                else
                {
                    return;
                }
            }
            viewer.ScrollToVerticalOffset(viewer.VerticalOffset + Math.Truncate(dif / 16));
        }

        public const double ScrollThreshold = 8;

        public static void StartScroll(this ScrollViewer viewer, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(viewer);
            var initX = pos.X;
            var initY = pos.Y;
            var initOX = viewer.HorizontalOffset;
            var initOY = viewer.VerticalOffset;
            var moved = false;
            viewer.CaptureMouse();

            void mouseMove(object sender, MouseEventArgs e)
            {
                var pos = e.GetPosition(viewer);
                var x = pos.X;
                var y = pos.Y;
                viewer.ScrollToHorizontalOffset(initOX - x + initX);
                viewer.ScrollToVerticalOffset(initOY - y + initY);
                if (!moved)
                {
                    var dx = x - initX;
                    var dy = y - initY;
                    if (dx * dx + dy * dy >= ScrollThreshold * ScrollThreshold)
                    {
                        moved = true;
                    }
                }
                e.Handled = true;
            }

            void mouseUp(object sender, MouseButtonEventArgs e)
            {
                viewer.ReleaseMouseCapture();
                viewer.MouseMove -= mouseMove;
                viewer.MouseUp -= mouseUp;
                if (moved)
                {
                    e.Handled = true;
                }
            }

            viewer.MouseMove += mouseMove;
            viewer.MouseUp += mouseUp;
            e.Handled = true;
        }
    }
}
