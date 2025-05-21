using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract class CanvasBase : TextViewerBase, ICanvas
    {
        public static readonly DependencyProperty ViewportLeftProperty = ICanvas.ViewportLeftProperty;
        public static readonly DependencyProperty ViewportTopProperty = ICanvas.ViewportTopProperty;
        public static readonly DependencyProperty ViewportWidthProperty = ICanvas.ViewportWidthProperty;
        public static readonly DependencyProperty ViewportHeightProperty = ICanvas.ViewportHeightProperty;

        protected double _vx = ICanvas.DefaultPosition;
        protected double _vy = ICanvas.DefaultPosition;
        protected double _vw = ICanvas.DefaultSize;
        protected double _vh = ICanvas.DefaultSize;
        public double ViewportLeft { get => _vx; set => SetValue(ViewportHeightProperty, value); }
        public double ViewportTop { get => _vy; set => SetValue(ViewportWidthProperty, value); }
        public double ViewportWidth { get => _vw; set => SetValue(ViewportWidthProperty, value); }
        public double ViewportHeight { get => _vh; set => SetValue(ViewportHeightProperty, value); }

        void ICanvas.OnViewportLeftChanged(double value)
        {
            _vx = value;
            OnViewportLeftChanged(value);
        }

        void ICanvas.OnViewportTopChanged(double value)
        {
            _vy = value;
            OnViewportTopChanged(value);
        }

        void ICanvas.OnViewportWidthChanged(double value)
        {
            _vw = value;
            OnViewportWidthChanged(value);
        }

        void ICanvas.OnViewportHeightChanged(double value)
        {
            _vh = value;
            OnViewportHeightChanged(value);
        }

        protected virtual void OnViewportLeftChanged(double value) { }
        protected virtual void OnViewportTopChanged(double value) { }
        protected virtual void OnViewportWidthChanged(double value) { }
        protected virtual void OnViewportHeightChanged(double value) { }

        private bool _need_refresh;

        public CanvasBase()
        {
            Focusable = false;
            IsHitTestVisible = false;
        }

        public virtual void ReserveViewportRefresh()
        {
            if (!_need_refresh)
            {
                _need_refresh = true;
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            RefreshChildrenIfNeeded();
            base.OnRender(drawingContext);
        }

        protected void RefreshChildrenIfNeeded()
        {
            if (_need_refresh)
            {
                RefreshVisible();
                _need_refresh = false;
            }
        }

        protected abstract void RefreshVisible();
    }
}
