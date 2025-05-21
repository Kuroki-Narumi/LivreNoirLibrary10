using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface ISelection
    {
        public SelectionRect SelectionRect { get; }
        public bool IsSelectionEmpty { get; }

        public void OnSelectionStart(double x, double y) { }
        public void OnHorizontalSelection(ref double x1, ref double x2) { }
        public void OnVerticalSelection(ref double y1, ref double y2) { }
        public void OnSelectionFinished(bool selected) { }

        public double GetDeadZone() => 0.0;
    }

    public static class ISelectionExtension
    {
        public static bool IsHorizontalSelection(this ISelection s) => s.SelectionRect.Type.IsHorizontal();
        public static bool IsVerticalSelection(this ISelection s) => s.SelectionRect.Type.IsVertical();
        public static bool NeedsHideSelection(this ISelection s) => s.SelectionRect.Type.NeedsHide();

        public static void StartSelection<T>(this T s, UIElement element)
            where T : ISelection
        {
            element.CaptureMouse();
            var initPos = Mouse.GetPosition(element);
            var horizontal = IsHorizontalSelection(s);
            var vertical = IsVerticalSelection(s);
            var hide = NeedsHideSelection(s);
            var selection = s.SelectionRect;

            if (s.IsSelectionEmpty)
            {
                selection.SetNew();
            }
            else if (KeyInput.IsAltDown())
            {
                if (KeyInput.IsShiftDown())
                {
                    selection.SetIntersect();
                }
                else
                {
                    selection.SetExcept();
                }
            }
            else if (KeyInput.IsShiftDown())
            {
                selection.SetUnion();
            }
            else
            {
                selection.SetNew();
            }

            var selecting = false;
            var deadZone = s.GetDeadZone();
            s.OnSelectionStart(initPos.X, initPos.Y);
            element.MouseMove += MouseMove;
            element.MouseLeftButtonUp += MouseUp;

            void MouseMove(object sender, MouseEventArgs e)
            {
                var pos = Mouse.GetPosition(element);
                var x1 = initPos.X;
                var x2 = pos.X;
                if (x2 < x1)
                {
                    (x2, x1) = (x1, x2);
                }
                var y1 = initPos.Y;
                var y2 = pos.Y;
                if (y2 < y1)
                {
                    (y2, y1) = (y1, y2);
                }
                if (!selecting)
                {
                    if ((horizontal && x2 - x1 >= deadZone) || (vertical && y2 - y1 >= deadZone))
                    {
                        selecting = true;
                    }
                    else
                    {
                        return;
                    }
                }
                (s as ScrollViewer)?.AutoScroll(e);
                if (horizontal)
                {
                    s.OnHorizontalSelection(ref x1, ref x2);
                    selection.SetHorizontal(x1, x2 - x1);
                }
                if (vertical)
                {
                    s.OnVerticalSelection(ref y1, ref y2);
                    selection.SetVertical(y1, y2 - y1);
                }
                e.Handled = true;
            }

            void MouseUp(object sender, MouseButtonEventArgs e)
            {
                element.ReleaseMouseCapture();
                element.MouseMove -= MouseMove;
                element.MouseLeftButtonUp -= MouseUp;
                s.OnSelectionFinished(selecting);
                selection.Mode = SelectionMode.None;
                if (hide)
                {
                    selection.Clear();
                }
            }
        }
    }
}
