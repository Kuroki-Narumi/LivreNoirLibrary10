using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    [ContentProperty(nameof(Source))]
    public partial class IconPresenter : FrameworkElement
    {
        static IconPresenter()
        {
            IsEnabledProperty.OverrideMetadata(typeof(IconPresenter), PropertyUtils.GetMeta(true, OnIsEnabledChanged));
        }

        public static readonly FontFamily DefaultFontFamily = new("Segoe MDL2 Assets");
        public static Brush DefaultForeground => Brushes.Black;
        public const Stretch DefaultStretch = Stretch.Uniform;
        public const double DefaultSize = 32;

        public static readonly DependencyProperty FontFamilyProperty = PropertyUtils.RegisterTwoWay(typeof(IconPresenter), DefaultFontFamily, OnFontFamilyChanged);
        public static readonly DependencyProperty ForegroundProperty = Control.ForegroundProperty.AddOwner(typeof(IconPresenter), DefaultForeground, OnForegroundChanged);

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconPresenter i)
            {
                i.Opacity = e.NewValue is true ? 1.0 : 0.56;
            }
        }

        private static void OnFontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconPresenter i)
            {
                i._option.FontFamily = (e.NewValue as FontFamily) ?? DefaultFontFamily;
                if (i._ft is not null)
                {
                    i.UpdateText();
                }
            }
        }

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconPresenter i)
            {
                i._option.Foreground = (e.NewValue as Brush) ?? DefaultForeground;
                if (i._ft is not null)
                {
                    i.UpdateText();
                }
            }
        }

        private ImageSource? _imageSource;
        private Drawing? _drawing;
        private Visual? _visual;
        private string? _text;

        private readonly FormattedTextOption _option = new() { FontFamily = DefaultFontFamily, FontSize = DefaultSize, Foreground = DefaultForeground };
        private FormattedText? _ft;

        private double _src_x = 0;
        private double _src_y = 0;
        private double _src_w = double.NaN;
        private double _src_h = double.NaN;

        [DependencyProperty(BindsTwoWayByDefault = true)]
        private object? _source;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private Stretch _stretch = DefaultStretch;

        public FontFamily? FontFamily
        {
            get => _option.FontFamily;
            set => SetValue(FontFamilyProperty, value);
        }

        public Brush? Foreground
        {
            get => _option.Foreground;
            set => SetValue(ForegroundProperty, value);
        }

        public IconPresenter()
        {
            Focusable = false;
            IsHitTestVisible = false;
        }

        private void OnSourceChanged(object? value)
        {
            if (ReferenceEquals(value, this))
            {
                value = null;
            }
            _imageSource = value as ImageSource;
            _drawing = value as Drawing;
            _visual = value as Visual;
            _text = value?.ToString();
            if (_imageSource is not null)
            {
                _src_x = 0;
                _src_y = 0;
                _src_w = _imageSource.Width;
                _src_h = _imageSource.Height;
            }
            else if (_drawing is not null)
            {
                (_src_x, _src_y, _src_w, _src_h) = _drawing.Bounds;
            }
            else if (_visual is null && !string.IsNullOrEmpty(_text))
            {
                UpdateText();
                return;
            }
            else
            {
                (_src_x, _src_y, _src_w, _src_h) = (0, 0, double.NaN, double.NaN);
            }
            _ft = null;
            InvalidateMeasure();
            InvalidateVisual();
        }

        private void UpdateText()
        {
            _ft = _text!.CreateFormattedText(_option);
            _src_x = 0;
            _src_y = 0;
            _src_w = _ft.Width;
            _src_h = _ft.Height;
            InvalidateMeasure();
            InvalidateVisual();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var w = Width;
            var h = Height;
            var w_infinite = !double.IsFinite(w);
            var h_infinite = !double.IsFinite(h);
            if (double.IsFinite(_src_w))
            {
                if (w_infinite)
                {
                    if (h_infinite)
                    {
                        w = Math.Min(_src_w, availableSize.Width);
                        h = Math.Min(_src_h, availableSize.Height);
                    }
                    else
                    {
                        w = _src_w * h / _src_h;
                    }
                }
                else if (h_infinite)
                {
                    h = _src_h * w / _src_w;
                }
            }
            else
            {
                if (w_infinite)
                {
                    w = _visual is not null ? availableSize.Width : 0;
                }
                if (h_infinite)
                {
                    h = _visual is not null ? availableSize.Height : 0;
                }
            }
            return new(w, h);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            RenderSize = finalSize;
            return finalSize;
        }

        private static (double ScaleX, double ScaleY) GetScale(double dstW, double dstH, double srcW, double srcH, Stretch stretch, StretchDirection direction = StretchDirection.Both)
        {
            var scaleX = dstW / srcW;
            var scaleY = dstH / srcH;
            switch (stretch)
            {
                case Stretch.Fill:
                    break;
                case Stretch.Uniform:
                case Stretch.UniformToFill:
                    if (stretch is Stretch.Uniform ? scaleX <= scaleY : scaleX >= scaleY)
                    {
                        scaleY = scaleX;
                    }
                    else
                    {
                        scaleX = scaleY;
                    }
                    break;
                default:
                    scaleX = scaleY = 1;
                    break;
            }
            switch (direction)
            {
                case StretchDirection.UpOnly:
                    if (scaleX < 1) scaleX = 1;
                    if (scaleY < 1) scaleY = 1;
                    break;
                case StretchDirection.DownOnly:
                    if (scaleX > 1) scaleX = 1;
                    if (scaleY > 1) scaleY = 1;
                    break;
            }
            return (scaleX, scaleY);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            var (dstW, dstH) = RenderSize;
            Action renderAction;
            if (_drawing is not null)
            {
                renderAction = () => dc.DrawDrawing(_drawing);
            }
            else if (_imageSource is not null)
            {
                renderAction = () => dc.DrawImage(_imageSource, new(0, 0, _src_w, _src_h));
            }
            else if (_visual is not null)
            {
                VisualBrush brush = new(_visual)
                {
                    Stretch = _stretch,
                    AlignmentX = AlignmentX.Center,
                    AlignmentY = AlignmentY.Center,
                };
                dc.DrawRectangle(brush, null, new(0, 0, dstW, dstH));
                return;
            }
            else if (_ft is not null)
            {
                renderAction = () => dc.DrawText(_ft, new(0, 0));
            }
            else
            {
                return;
            }
            var (scaleX, scaleY) = GetScale(dstW, dstH, _src_w, _src_h, _stretch);
            var ox = (dstW - _src_w * scaleX) / 2;
            var oy = (dstH - _src_h * scaleY) / 2;
            if (UseLayoutRounding)
            {
                ox = Math.Round(ox);
                oy = Math.Round(oy);
            }
            Matrix m = new();
            m.Translate(-_src_x, -_src_y);
            m.Scale(scaleX, scaleY);
            m.Translate(ox, oy);
            MatrixTransform mt = new(m);
            mt.Freeze();
            dc.PushTransform(mt);
            renderAction();
            dc.Pop();
        }
    }
}
