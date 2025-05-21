using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ScrollViewerBase : IScaleProperty
    {
        public double MinScaleX { get; protected set; } = double.Epsilon;
        public double MaxScaleX { get; protected set; } = double.MaxValue;
        public double MinScaleY { get; protected set; } = double.Epsilon;
        public double MaxScaleY { get; protected set; } = double.MaxValue;

        public static readonly DependencyProperty ScaleXProperty = IScaleProperty.RegisterScaleX<ScrollViewerBase>(OnScaleXChanged, CoerceScaleX);
        public static readonly DependencyProperty ScaleYProperty = IScaleProperty.RegisterScaleY<ScrollViewerBase>(OnScaleYChanged, CoerceScaleY);

        protected static object CoerceScaleX(DependencyObject d, object baseValue)
        {
            if (d is ScrollViewerBase v)
            {
                return Math.Clamp((double)baseValue, v.MinScaleX, v.MaxScaleX);
            }
            return baseValue;
        }

        protected static object CoerceScaleY(DependencyObject d, object baseValue)
        {
            if (d is ScrollViewerBase v)
            {
                return Math.Clamp((double)baseValue, v.MinScaleY, v.MaxScaleY);
            }
            return baseValue;
        }

        protected static void OnScaleXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewerBase v)
            {
                var nv = (double)e.NewValue;
                var ov = v._scale_x;
                v._scale_x = nv;
                if (!v._initializing && ov != nv)
                {
                    v.OnScaleXChanged(ov, nv);
                }
            }
        }

        protected static void OnScaleYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewerBase v)
            {
                var nv = (double)e.NewValue;
                var ov = v._scale_y;
                v._scale_y = nv;
                if (!v._initializing && ov != nv)
                {
                    v.OnScaleYChanged(ov, nv);
                }
            }
        }

        protected double _scale_x = IScaleProperty.DefaultScale;
        protected double _scale_y = IScaleProperty.DefaultScale;

        protected virtual void OnScaleXChanged(double oldValue, double newValue) { }
        protected virtual void OnScaleYChanged(double oldValue, double newValue) { }

        public double ScaleX { get => _scale_x; set => SetValue(ScaleXProperty, value); }
        public double ScaleY { get => _scale_y; set => SetValue(ScaleYProperty, value); }

        public void SetScale(double scaleX, double scaleY)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
        }
    }
}
