using System;
using System.Windows;
using System.Windows.Input;
using Forms = System.Windows.Forms;
using DrPoint = System.Drawing.Point;
using DrRect = System.Drawing.Rectangle;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static DrRect GetScreenBounds(this Point point)
        {
            DrPoint p = new((int)point.X, (int)point.Y);
            var screens = Forms.Screen.AllScreens;
            for (int i = 0; i < screens.Length; i++)
            {
                var bounds = screens[i].Bounds;
                if (bounds.Contains(p))
                {
                    return bounds;
                }
            }
            return default;
        }

        public static DrRect GetScreenBounds(this Window window)
        {
            var screens = Forms.Screen.AllScreens;
            DrRect windowRect = new((int)window.Left, (int)window.Top, (int)window.Width, (int)window.Height);
            int maxArea = 0;
            int maxIndex = -1;
            for (int i = 0; i < screens.Length; i++)
            {
                var bounds = DrRect.Intersect(windowRect, screens[i].Bounds);
                var area = bounds.Width * bounds.Height;
                if (area > maxArea)
                {
                    maxArea = area;
                    maxIndex = i;
                }
            }
            return (maxIndex >= 0 ? screens[maxIndex] : Forms.Screen.PrimaryScreen)?.Bounds ?? default;
        }

        public static void CorrectPosition(this Window target, Window subject)
        {
            var bounds = GetScreenBounds(subject);
            if (target.Left < bounds.Left)
            {
                target.Left = bounds.Left;
            }
            else if (target.Left + target.ActualWidth > bounds.Right)
            {
                target.Left = bounds.Right - target.ActualWidth;
            }
            if (target.Top < bounds.Top)
            {
                target.Top = bounds.Top;
            }
            else if (target.Top + target.ActualHeight > bounds.Bottom)
            {
                target.Top = bounds.Bottom - target.ActualHeight;
            }
        }

        public static void PlaceToCenter(this Window window, double offsetX = 0, double offsetY = 0)
        {
            var bounds = GetScreenBounds(window);
            window.Left = bounds.X + (bounds.Width - window.Width) / 2 + offsetX;
            window.Top = bounds.Y + (bounds.Height - window.Height) / 2 + offsetY;
        }

        public static void PlaceToCursor(this Window window, double offsetX = 0, double offsetY = 0)
        {
            var pos = GetCursorPos(window);
            var bounds = GetScreenBounds(pos);
            var x = pos.X + offsetX;
            var y = pos.Y + offsetY;
            if (x + window.Width > bounds.X + bounds.Width)
            {
                x = bounds.X + bounds.Width - window.Width;
            }
            if (y + window.Height > bounds.Y + bounds.Height)
            {
                y = bounds.Y + bounds.Height - window.Height;
            }
            window.Left = x;
            window.Top = y;
        }

        public static void PlaceToPoint(this Window window, Point point)
        {
            var bounds = GetScreenBounds(point);
            var (x, y) = point;
            if (x + window.Width > bounds.X + bounds.Width)
            {
                x = bounds.X + bounds.Width - window.Width;
            }
            if (y + window.Height > bounds.Y + bounds.Height)
            {
                y = bounds.Y + bounds.Height - window.Height;
            }
            window.Left = x;
            window.Top = y;
        }

        public static void DragMoveWithSnap(this Window window, DragMoveOptions options = default)
        {
            var rect = window.GetRect();
            var (limitLeft, limitTop, limitWidth, limitHeight) = GetScreenBounds(window);
            var (initLeft, initTop, initWidth, initHeight) = rect;
            var (initCursorX, initCursorY) = GetCursorPos();
            var minX = limitLeft - initLeft;
            var maxX = minX + limitWidth - initWidth;
            var minY = limitTop - initTop;
            var maxY = minY + limitHeight - initHeight;
            var (m_th, s_th, changing, finished) = options;

            bool moving = false;

            void MouseMove(object sender, MouseEventArgs e)
            {
                var pos = GetCursorPos();
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
                window.Left = initLeft + dX;
                window.Top = initTop + dY;
                changing?.Invoke(window, new(rect, window.GetRect()));
                e.Handled = true;
            }

            void MouseUp(object sender, MouseButtonEventArgs e)
            {
                window.ReleaseMouseCapture();
                window.MouseMove -= MouseMove;
                window.MouseLeftButtonUp -= MouseUp;
                finished?.Invoke(window, new(rect, window.GetRect()));
                e.Handled = true;
            }

            window.CaptureMouse();
            window.MouseMove += MouseMove;
            window.MouseLeftButtonUp += MouseUp;
        }
    }
}
