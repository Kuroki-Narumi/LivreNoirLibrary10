using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract partial class BmsCanvasBase : CanvasBase
    {
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private double _scaleY = IScaleProperty.DefaultScale;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private double _bottom;

        private bool _need_refresh_vertical;

        protected virtual void OnScaleYChanged() => ReserveRefreshVertical();
        protected virtual void OnBottomChanged() => ReserveRefreshVertical();

        protected void ReserveRefreshVertical()
        {
            _need_refresh_vertical = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_need_refresh_vertical)
            {
                RefreshVertical();
                _need_refresh_vertical = false;
            }
            base.OnRender(drawingContext);
        }

        protected virtual void RefreshVertical() { }

        public double GetVisualY(double absolutePosition) => GetVisualY(absolutePosition, _bottom, _scaleY);
        public double GetAbsolutePosition(double visualY) => GetAbsolutePosition(visualY, _bottom, _scaleY);

        public static double GetVisualY(double absolutePosition, double bottom, double scaleY) => bottom - absolutePosition * scaleY;
        public static double GetAbsolutePosition(double visualY, double bottom, double scaleY) => (bottom - visualY) / scaleY;
    }
}
