using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ScrollIcon : FrameworkElement
    {
        [DependencyProperty]
        private ScrollIconType _iconType;

        public ScrollIcon()
        {
            IsHitTestVisible = false;
        }

        private void OnIconTypeChanged(ScrollIconType value)
        {
            if (value is ScrollIconType.None)
            {
                Visibility = Visibility.Collapsed;
            }
            else
            {
                Visibility = Visibility.Visible;
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var source = _iconType switch
            {
                ScrollIconType.All => Icons.Scroll_All,
                ScrollIconType.Vertical => Icons.Scroll_Vertical,
                ScrollIconType.Horizontal => Icons.Scroll_Horizontal,
                _ => null,
            };
            if (source is not null)
            {
                var bounds = source.Bounds;
                drawingContext.PushTransform(new TranslateTransform(-bounds.Width / 2, -bounds.Height / 2));
                drawingContext.DrawDrawing(source);
                drawingContext.Pop();
            }
        }
    }

    [Flags]
    public enum ScrollIconType
    {
        None,
        Vertical,
        Horizontal,
        All,
    }
}
