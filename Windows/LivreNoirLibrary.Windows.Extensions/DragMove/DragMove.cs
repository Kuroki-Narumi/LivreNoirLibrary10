using LivreNoirLibrary.Media;
using LivreNoirLibrary.Windows.Input;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Forms = System.Windows.Forms;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static (double ScaleX, double ScaleY) GetDisplayScale(this Visual visual)
        {
            var matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return (matrix.M11, matrix.M22);
        }

        public static Rect GetRect(this Window window) => new(window.Left, window.Top, window.ActualWidth, window.ActualHeight);
        public static Rect GetDisplayRect(this Window window)
        {
            var x = window.Left;
            var y = window.Top;
            var (sx, sy) = GetDisplayScale(window);
            return new(x * sx, y * sy, window.ActualWidth * sx, window.ActualHeight * sy);
        }

        public static Rect GetScreenBounds(this Point point)
        {
            if (Forms.Screen.FromPoint(point.ToDrawingPoint()) is { } screen)
            {
                return screen.Bounds.ToRect();
            }
            return default;
        }

        public static Rect GetScreenBounds(this Window window)
        {
            var screen = Forms.Screen.FromRectangle(GetDisplayRect(window).ToDrawingRect()) ?? Forms.Screen.PrimaryScreen;
            if (screen is not null)
            {
                return screen.Bounds.ToRect();
            }
            return default;
        }

        public static void CorrectPosition(this Window target, Window? subject = null)
        {
            subject ??= target;
            var bounds = GetScreenBounds(subject);
            var (x, y, w, h) = GetDisplayRect(target);
            var (sx, sy) = GetDisplayScale(subject);
            x = Math.Clamp(x, bounds.X, Math.Max(bounds.Right - w, bounds.X));
            y = Math.Clamp(y, bounds.Y, Math.Max(bounds.Bottom - h, bounds.Y));
            target.Left = x / sx;
            target.Top = y / sy;
        }

        public static void PlaceToCenter(this Window window, double offsetX = 0, double offsetY = 0)
        {
            var bounds = GetScreenBounds(window);
            var (sx, sy) = window.GetDisplayScale();
            var x = bounds.X + (bounds.Width - window.ActualWidth * sx) * 0.5 + offsetX;
            var y = bounds.Y + (bounds.Height - window.ActualHeight * sy) * 0.5 + offsetY;
            window.Left = x / sx;
            window.Top = y / sy;
        }

        public static void PlaceToCursor(this Window window, double offsetX = 0, double offsetY = 0)
        {
            var (x, y) = window.PointToScreen(Mouse.GetPosition(window));
            PlaceToPoint(window, new(x + offsetX, y + offsetY));
        }

        public static void PlaceToPoint(this Window window, Point point)
        {
            var bounds = GetScreenBounds(point);
            var (sx, sy) = window.GetDisplayScale();
            var x = Math.Max(point.X, bounds.X + Math.Max(bounds.Width - window.ActualWidth * sx, 0));
            var y = Math.Max(point.Y, bounds.Y + Math.Max(bounds.Height - window.ActualHeight * sy, 0));
            window.Left = x / sx;
            window.Top = y / sy;
        }

        public static void DragMoveWithSnap(this Window window, DragMoveOptions options = default)
        {
            var initialRect = window.GetRect();
            var (limitLeft, limitTop, limitWidth, limitHeight) = GetScreenBounds(window);
            var (initLeft, initTop, initWidth, initHeight) = window.GetDisplayRect();
            var (initCursorX, initCursorY) = window.PointToScreen(Mouse.GetPosition(window));
            var minX = limitLeft - initLeft;
            var maxX = minX + limitWidth - initWidth;
            var minY = limitTop - initTop;
            var maxY = minY + limitHeight - initHeight;
            var (m_th, s_th, changing, finished) = options;

            bool moving = false;

            void MouseMove(object sender, MouseEventArgs e)
            {
                var (sx, sy) = GetDisplayScale(window);
                var pos = window.PointToScreen(e.GetPosition(window));
                var dX = pos.X - initCursorX;
                var dY = pos.Y - initCursorY;
                // スナップ
                if (KeyInput.IsCtrlDown())
                {
                    dX = Math.Clamp(dX, minX, maxX);
                    dY = Math.Clamp(dY, minY, maxY);
                    RectSelection.ProcessSnap(ref dX, minX, maxX, 2, s_th);
                    RectSelection.ProcessSnap(ref dY, minY, maxY, 2, s_th);
                }
                // 移動しきい値
                if (!moving)
                {
                    if (dX >= m_th || dY >= m_th || dX <= -m_th || dY <= -m_th)
                    {
                        moving = true;
                    }
                    else
                    {
                        return;
                    }
                }
                // 8方向移動
                if (KeyInput.IsShiftDown())
                {
                    var tan = Math.Abs(dY / dX);
                    if (tan < 0.5)
                    {
                        dY = 0;
                    }
                    else if (tan < 1)
                    {
                        dY = Math.Abs(dX) * Math.Sign(dY);
                    }
                    else if (tan < 2)
                    {
                        dX = Math.Abs(dY) * Math.Sign(dX);
                    }
                    else
                    {
                        dX = 0;
                    }
                }
                // 移動先の計算
                window.Left = (initLeft + dX) / sx;
                window.Top = (initTop + dY) / sy;
                changing?.Invoke(window, new(initialRect, window.GetRect()));
                e.Handled = true;
            }

            void MouseUp(object sender, MouseButtonEventArgs e)
            {
                window.ReleaseMouseCapture();
                window.MouseMove -= MouseMove;
                window.MouseLeftButtonUp -= MouseUp;
                finished?.Invoke(window, new(initialRect, window.GetRect()));
                e.Handled = true;
            }

            window.CaptureMouse();
            window.MouseMove += MouseMove;
            window.MouseLeftButtonUp += MouseUp;
        }
    }
}
