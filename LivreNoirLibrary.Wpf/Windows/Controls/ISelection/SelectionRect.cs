using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using static LivreNoirLibrary.Media.MediaUtils;

namespace LivreNoirLibrary.Windows.Controls
{
    public class SelectionRect : ISelectionRect
    {
        private SelectionMode _mode;

        public Rectangle Rectangle { get; init; } = new()
        {
            Fill = GetBrush(SelectionColors.Fill),
            Stroke = GetBrush(SelectionColors.Stroke),
            StrokeThickness = 1,
            Width = 0,
            Height = 0,
        };

        public SelectionMode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                Fill = value switch
                {
                    SelectionMode.New => GetBrush(SelectionColors.Fill),
                    SelectionMode.Union => GetBrush(SelectionColors.Union),
                    SelectionMode.Except => GetBrush(SelectionColors.Except),
                    SelectionMode.Intersect => GetBrush(SelectionColors.Intersect),
                    _ => null
                };
            }
        }

        public Brush? Fill { get => Rectangle.Fill; set => Rectangle.Fill = value; }

        public void SetHorizontalBinding(Binding binding)
        {
            Rectangle.SetBinding(FrameworkElement.WidthProperty, binding);
        }

        public void SetVerticalBinding(Binding binding)
        {
            Rectangle.SetBinding(FrameworkElement.HeightProperty, binding);
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

        public void Hide()
        {
            Rectangle.Visibility = Visibility.Collapsed;
        }
    }
}
