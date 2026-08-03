using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Win32Api;
using LivreNoirLibrary.Windows.Input;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static (double ScaleX, double ScaleY) GetDisplayScale(this Visual visual)
        {
            var scale = VisualTreeHelper.GetDpi(visual);
            return (scale.DpiScaleX, scale.DpiScaleY);
        }

        public static Rect GetRect(this Window window) => new(window.Left, window.Top, window.ActualWidth, window.ActualHeight);
        public static Rect GetWindowRect(this Visual visual) => Window.GetWindow(visual).GetRect();

        public static Int32Rect GetScreenBounds(this Point point)
        {
            Int32Rect result = default;
            System.Drawing.Point p = new((int)point.X, (int)point.Y);
            NativeMethods.EnumerateMonitorInfo(info =>
            {
                if (info.Rect.Contains(p))
                {
                    result = info.Rect.ToInt32Rect();
                    return false;
                }
                return true;
            });
            return result;
        }

        public static Int32Rect GetScreenBounds(this Window window) => NativeMethods.MonitorFromWindow(window.GetHandle()).Rect.ToInt32Rect();
        public static Int32Rect GetScreenBounds(this Visual visual) => Window.GetWindow(visual).GetScreenBounds();

        public static Rect GetScaledScreenBounds(this Window window)
        {
            var (x, y, w, h) = GetScreenBounds(window);
            var (sx, sy) = GetDisplayScale(window);
            return new(x / sx, y / sy, w / sx, h / sy);
        }

        public static void CorrectPosition(this Window target, Window? boundsSource = null)
        {
            boundsSource ??= target;
            var bounds = GetScreenBounds(boundsSource); 
            NativeMethods.TryGetWindowRect(target.GetHandle(), out var windowRect);
            NativeMethods.TryGetActualWindowRect(target.GetHandle(), out var r2);
            var (x, y, w, h) = windowRect;
            var (sx, sy) = GetDisplayScale(target);
            var newX = Math.Clamp(x, bounds.X, Math.Max(bounds.Right - w, bounds.X)) / sx;
            var newY = Math.Clamp(y, bounds.Y, Math.Max(bounds.Bottom - h, bounds.Y)) / sy;
            target.Left = newX;
            target.Top = newY;
        }

        public static void PlaceToCenter(this Window window, double offsetX = 0, double offsetY = 0, Visual? scaleSource = null)
        {
            var bounds = GetScreenBounds(window);
            var (sx, sy) = GetDisplayScale(scaleSource ?? window);
            var x = bounds.X + (bounds.Width - window.ActualWidth * sx) * 0.5 + offsetX;
            var y = bounds.Y + (bounds.Height - window.ActualHeight * sy) * 0.5 + offsetY;
            window.Left = x / sx;
            window.Top = y / sy;
        }

        public static void PlaceToCursor(this Window window, double offsetX = 0, double offsetY = 0, Window? placementTarget = null)
        {
            placementTarget ??= window;
            var (x, y) = NativeMethods.GetCursorPos();
            PlaceToPoint(window, new(x + offsetX, y + offsetY), placementTarget);
        }

        public static void PlaceToPoint(this Window window, Point pixelPoint, Visual? scaleSource = null)
        {
            var bounds = GetScreenBounds(pixelPoint);
            var (sx, sy) = GetDisplayScale(scaleSource ?? window);
            var x = Math.Min(pixelPoint.X, bounds.X + Math.Max(bounds.Width - window.ActualWidth * sx, 0));
            var y = Math.Min(pixelPoint.Y, bounds.Y + Math.Max(bounds.Height - window.ActualHeight * sy, 0));
            window.Left = x / sx;
            window.Top = y / sy;
        }

        public static void DragMoveWithSnap(this Window window, DragMoveOptions options = default)
        {
            var handle = window.GetHandle();
            var initialRect = window.GetRect();
            var (limitLeft, limitTop, limitWidth, limitHeight) = GetScreenBounds(window);
            NativeMethods.TryGetWindowRect(handle, out var r);
            var (initLeft, initTop, initWidth, initHeight) = r;
            var (initCursorX, initCursorY) = NativeMethods.GetCursorPos();
            var minX = limitLeft - initLeft;
            var maxX = minX + limitWidth - initWidth;
            var minY = limitTop - initTop;
            var maxY = minY + limitHeight - initHeight;
            var (m_th, s_th, changing, finished) = options;

            bool moving = false;

            void MouseMove(object sender, MouseEventArgs e)
            {
                var pos = NativeMethods.GetCursorPos();
                double dX = pos.X - initCursorX;
                double dY = pos.Y - initCursorY;
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
                var (sx, sy) = window.GetDisplayScale();
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
