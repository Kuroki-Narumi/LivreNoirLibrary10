using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using static LivreNoirLibrary.Media.MediaUtils;

namespace LivreNoirLibrary.Windows.Controls
{
    public class SelectionRect(SelectionType type)
    {
        public Rectangle Rectangle { get; init; } = new()
        {
            Fill = GetBrush(SelectionColors.Fill),
            Stroke = GetBrush(SelectionColors.Stroke),
            StrokeThickness = 1,
            Width = 0,
            Height = 0,
        };

        public SelectionType Type { get; init; } = type;
        public SelectionMode Mode { get; set; }

        public Brush? Fill { get => Rectangle.Fill; set => Rectangle.Fill = value; }

        public void SetNew()
        {
            Mode = SelectionMode.New;
            Fill = GetBrush(SelectionColors.Fill);
        }

        public void SetUnion()
        {
            Mode = SelectionMode.Union;
            Fill = GetBrush(SelectionColors.Union);
        }

        public void SetExcept()
        {
            Mode = SelectionMode.Except;
            Fill = GetBrush(SelectionColors.Except);
        }

        public void SetIntersect()
        {
            Mode = SelectionMode.Intersect;
            Fill = GetBrush(SelectionColors.Intersect);
        }

        public void SetHorizontalBinding(Binding binding)
        {
            Rectangle.SetBinding(FrameworkElement.WidthProperty, binding);
        }

        public void SetVerticalBinding(Binding binding)
        {
            Rectangle.SetBinding(FrameworkElement.HeightProperty, binding);
        }

        public void Clear()
        {
            if (Type.IsHorizontal())
            {
                Rectangle.Width = 0;
            }
            if (Type.IsVertical())
            {
                Rectangle.Height = 0;
            }
            Rectangle.Visibility = Visibility.Collapsed;
        }

        public void Show()
        {
            Rectangle.Visibility = Visibility.Visible;
        }

        public void SetHorizontal(double x, double width)
        {
            Canvas.SetLeft(Rectangle, x);
            Rectangle.Width = Math.Max(width, 0);
            Show();
        }

        public void SetVertical(double y, double height)
        {
            Canvas.SetTop(Rectangle, y);
            Rectangle.Height = Math.Max(height, 0);
            Show();
        }
    }
}
