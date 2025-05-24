using System;
using System.Runtime.CompilerServices;
using System.Windows;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void ContentSizeChangedEventHandler(object sender, double width, double height);

    public partial class ScrollViewerBase : IScaleProperty
    {
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

        protected static DependencyProperty RegisterBooleanProperty<T>(PropertyChangedCallback callback, bool defaultValue = true, [CallerMemberName] string caller = "")
            => DependencyProperty.Register(PropertyUtils.GetPropertyName(caller), typeof(bool?), typeof(T), PropertyUtils.GetMetaTwoWay(defaultValue, callback));

        public event ContentSizeChangedEventHandler? ContentSizeChanged;

        protected double _scale_x = IScaleProperty.DefaultScale;
        protected double _scale_y = IScaleProperty.DefaultScale;
        [DependencyProperty(SetterScope = Scope.Protected)]
        protected double _contentWidth;
        [DependencyProperty(SetterScope = Scope.Protected)]
        protected double _contentHeight;

        public double MinScaleX { get; protected set; } = double.Epsilon;
        public double MaxScaleX { get; protected set; } = double.MaxValue;
        public double MinScaleY { get; protected set; } = double.Epsilon;
        public double MaxScaleY { get; protected set; } = double.MaxValue;
        public double ScaleX { get => _scale_x; set => SetValue(ScaleXProperty, value); }
        public double ScaleY { get => _scale_y; set => SetValue(ScaleYProperty, value); }

        protected virtual void OnScaleXChanged(double oldValue, double newValue) { }
        protected virtual void OnScaleYChanged(double oldValue, double newValue) { }

        protected virtual void OnContentWidthChanged(double value)
        {
            ContentSizeChanged?.Invoke(this, value, _contentHeight);
        }

        protected virtual void OnContentHeightChanged(double value)
        {
            ContentSizeChanged?.Invoke(this, _contentWidth, value);
        }

        public void SetScale(double scaleX, double scaleY)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
        }
    }
}
