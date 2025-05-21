using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface ICanvas
    {
        public const double DefaultPosition = 0;
        public const double DefaultSize = 1;

        public static readonly DependencyProperty ViewportLeftProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(CanvasBase), DefaultPosition, OnViewportLeftChanged);
        public static readonly DependencyProperty ViewportTopProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(CanvasBase), DefaultPosition, OnViewportTopChanged);
        public static readonly DependencyProperty ViewportWidthProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(CanvasBase), DefaultSize, OnViewportWidthChanged);
        public static readonly DependencyProperty ViewportHeightProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(CanvasBase), DefaultSize, OnViewportHeightChanged);
        
        private static void OnViewportLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ICanvas c)
            {
                c.OnViewportLeftChanged((double)e.NewValue);
                c.ReserveViewportRefresh();
            }
        }

        private static void OnViewportTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ICanvas c)
            {
                c.OnViewportTopChanged((double)e.NewValue);
                c.ReserveViewportRefresh();
            }
        }

        private static void OnViewportWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ICanvas c)
            {
                c.OnViewportWidthChanged((double)e.NewValue);
                c.ReserveViewportRefresh();
            }
        }

        private static void OnViewportHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ICanvas c)
            {
                c.OnViewportHeightChanged((double)e.NewValue);
                c.ReserveViewportRefresh();
            }
        }

        public double ViewportLeft { get; set; }
        public double ViewportTop { get; set; }
        public double ViewportWidth { get; set; }
        public double ViewportHeight { get; set; }

        public void ReserveViewportRefresh();

        public void SetViewport(double x, double y, double width, double height)
        {
            ViewportLeft = x;
            ViewportTop = y;
            ViewportWidth = width;
            ViewportHeight = height;
        }

        public void SetViewport(in Rect rect) => SetViewport(rect.X, rect.Y, rect.Width, rect.Height);

        void OnViewportLeftChanged(double value) { }
        void OnViewportTopChanged(double value) { }
        void OnViewportWidthChanged(double value) { }
        void OnViewportHeightChanged(double value) { }
    }
}
