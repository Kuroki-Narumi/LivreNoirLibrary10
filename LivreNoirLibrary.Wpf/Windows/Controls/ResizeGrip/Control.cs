using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ResizeGrip
    {
        private static double Validate(double value, double @default = 0) => double.IsNormal(value) ? Math.Max(value, @default) : @default;

        private void OnMouseLeftButtonDown_Grip(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Shape s) return;
            _root ??= Window.GetWindow(this);
            if (_root is null) return;

            var dir = (int)s.Tag;
            var moveRight = dir is 3 or 6 or 9;
            var moveLeft = dir is 1 or 4 or 7;
            var moveBottom = dir is 1 or 2 or 3;
            var moveTop = dir is 7 or 8 or 9;

            var pos = this.GetCursorPos();
            var initCursorX = pos.X;
            var initCursorY = pos.Y;
            var screen = pos.GetScreenBounds();
            var initRect = _root.GetRect();

            var limitLeft = moveRight ? initRect.Left + Validate(_root.MinWidth) : screen.Left;
            var limitRight = moveLeft ? initRect.Right - Validate(_root.MinWidth) : screen.Right;
            var limitTop = moveBottom ? initRect.Top + Validate(_root.MinHeight) : screen.Top;
            var limitBottom = moveTop ? initRect.Bottom - Validate(_root.MinHeight) : screen.Bottom;

            double initLeft, initRight, initTop, initBottom, newLeft, newRight, newTop, newBottom;
            initLeft = newLeft = initRect.Left;
            initRight = newRight= initRect.Right;
            initTop = newTop = initRect.Top;
            initBottom = newBottom = initRect.Bottom;

            Rect NewRect()
            {
                return new(newLeft, newTop, newRight - newLeft, newBottom - newTop);
            }

            void Adjust()
            {
                if (KeyInput.IsCtrlDown())
                {
                    if (moveLeft)
                    {
                        newLeft = Math.Clamp(newLeft, limitLeft, limitRight);
                    }
                    else
                    {
                        newRight = Math.Clamp(newRight, limitLeft, limitRight);
                    }
                    if (moveTop)
                    {
                        newTop = Math.Clamp(newTop, limitTop, limitBottom);
                    }
                    else
                    {
                        newBottom = Math.Clamp(newBottom, limitTop, limitBottom);
                    }
                }
                else
                {
                    if (moveLeft)
                    {
                        newLeft = Math.Min(newLeft, limitRight);
                    }
                    else
                    {
                        newRight = Math.Max(newRight, limitLeft);
                    }
                    if (moveTop)
                    {
                        newTop = Math.Min(newTop, limitBottom);
                    }
                    else
                    {
                        newBottom = Math.Max(newBottom, limitTop);
                    }
                }
            }

            void MouseMove(object sender, MouseEventArgs e)
            {
                var pos = this.GetCursorPos();
                var dx = pos.X - initCursorX;
                var dy = pos.Y - initCursorY;
                newLeft = initLeft + (moveLeft ? dx : 0);
                newRight = initRight + (moveRight ? dx : 0);
                newTop = initTop + (moveTop ? dy : 0);
                newBottom = initBottom + (moveBottom ? dy : 0);
                Adjust();
                // 縦横比の維持
                var refWidth = Validate(_baseWidth);
                var refHeight = Validate(_baseHeight);
                if (KeyInput.IsShiftDown())
                {
                    refWidth = initRect.Width;
                    refHeight = initRect.Height;
                }
                if (refWidth > 0 && refHeight > 0)
                {
                    var nW = newRight - newLeft;
                    var nH = newBottom - newTop;
                    double scale = 1;
                    if (moveRight || moveLeft)
                    {
                        if (moveTop || moveBottom)
                        {
                            scale = Math.Max(nW / refWidth, nH / refHeight);
                        }
                        else
                        {
                            scale = nW / refWidth;
                        }
                    }
                    else if (moveTop || moveBottom)
                    {
                        scale = nH / refHeight;
                    }
                    if (moveLeft)
                    {
                        newLeft = Math.Round(newRight - refWidth * scale);
                    }
                    else
                    {
                        newRight = Math.Round(newLeft + refWidth * scale);
                    }
                    if (moveTop)
                    {
                        newTop = Math.Round(newBottom - refHeight * scale);
                    }
                    else
                    {
                        newBottom = Math.Round(newTop + refHeight * scale);
                    }
                    Adjust();
                }
                if (RectChanged is not null)
                {
                    RectChanged?.Invoke(this, new(initRect, NewRect()));
                }
                else
                {
                    _root.SetRect(NewRect());
                }
            }

            void MouseUp(object sender, MouseButtonEventArgs e)
            {
                ResizeFinished?.Invoke(this, new(initRect, NewRect()));
                s.ReleaseMouseCapture();
                s.MouseMove -= MouseMove;
                s.MouseLeftButtonUp -= MouseUp;
                e.Handled = true;
            }

            s.CaptureMouse();
            s.MouseMove += MouseMove;
            s.MouseLeftButtonUp += MouseUp;
            e.Handled = true;
        }
    }
}
