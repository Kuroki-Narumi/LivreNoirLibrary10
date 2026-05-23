using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

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
        public const StretchDirection DefaultStretchDirection = StretchDirection.Both;
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

        private readonly FormattedTextOptions _option = new() { FontFamily = DefaultFontFamily, FontSize = DefaultSize, Foreground = DefaultForeground };
        private FormattedText? _ft;

        private double _src_x = 0;
        private double _src_y = 0;
        private double _src_w = double.NaN;
        private double _src_h = double.NaN;

        [DependencyProperty(AffectsMeasure = true)]
        private object? _source;
        [DependencyProperty(AffectsMeasure = true)]
        private Stretch _stretch = DefaultStretch;
        [DependencyProperty(AffectsMeasure = true)]
        private StretchDirection _stretchDirection = DefaultStretchDirection;

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
            (Source as UIElement)?.Measure(new(double.PositiveInfinity, double.PositiveInfinity));
            return MeasureArrangeHelper(availableSize);

                /*
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
            if (Source is Drawing d && Icons.IconList.FirstOrDefault(i => i.Drawing == d) is { } info)
            {
                ExConsole.Write($"IconPresenter Measure: Source={info.Name}, Size=({w},{h})");
            }
            return new(0, 0);
                */
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = MeasureArrangeHelper(finalSize);
            RenderSize = size;
            return size;
        }

        private Size MeasureArrangeHelper(Size inputSize, [CallerMemberName]string? caller = null)
        {
            Size desiredSize;
            switch (Source)
            {
                case UIElement element:
                    desiredSize = element.DesiredSize;
                    break;
                case MediaPlayer mp:
                    desiredSize = new(mp.NaturalVideoWidth, mp.NaturalVideoHeight);
                    break;
                case ImageSource image:
                    desiredSize = new(image.Width, image.Height);
                    break;
                case Drawing drawing:
                    var bounds = drawing.Bounds;
                    desiredSize = new(bounds.Width, bounds.Height);
                    break;
                default:
                    if (_ft is { } ft)
                    {
                        desiredSize = new(ft.Width, ft.Height);
                        break;
                    }
                    else
                    {
                        return new(0, 0);
                    }
            }
            var scale = ComputeScaleFactor(inputSize, desiredSize, Stretch, StretchDirection);
            var w = desiredSize.Width * scale.Width;
            var h = desiredSize.Height * scale.Height;
            if (UseLayoutRounding)
            {
                w = Math.Round(w);
                h = Math.Round(h);
            }
            ExConsole.Write($"{caller}: ({w}, {h})");
            return new(w, h);
        }

        internal static Size ComputeScaleFactor(Size availableSize, Size contentSize, Stretch stretch, StretchDirection stretchDirection)
        {
            double sx = 1, sy = 1;
            var isWidthFinite = double.IsFinite(availableSize.Width);
            var isHeightFinite = double.IsFinite(availableSize.Height);
            if (stretch is Stretch.Fill or Stretch.Uniform or Stretch.UniformToFill & (isWidthFinite || isHeightFinite))
            {
                sx = contentSize.Width == 0 ? 0 : availableSize.Width / contentSize.Width;
                sy = contentSize.Height == 0 ? 0 : availableSize.Height / contentSize.Height;
                if (!isWidthFinite)
                {
                    sx = sy;
                }
                else if (!isHeightFinite)
                {
                    sy = sx;
                }
                else
                {
                    switch (stretch)
                    {
                        case Stretch.Uniform:
                            sx = sy = Math.Min(sx, sy);
                            break;
                        case Stretch.UniformToFill:
                            sx = sy = Math.Max(sx, sy);
                            break;
                    }
                }
                switch (stretchDirection)
                {
                    case StretchDirection.UpOnly:
                        sx = Math.Max(sx, 1);
                        sy = Math.Max(sy, 1);
                        break;
                    case StretchDirection.DownOnly:
                        sx = Math.Min(sx, 1);
                        sy = Math.Min(sy, 1);
                        break;
                }
            }
            return new(sx, sy);
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
            var (w, h) = RenderSize;
            switch (Source)
            {
                case UIElement element:
                    VisualBrush brush = new(element)
                    {
                        Stretch = Stretch,
                        AlignmentX = AlignmentX.Center,
                        AlignmentY = AlignmentY.Center,
                    };
                    ExConsole.Write((element.RenderSize, (w, h)));
                    dc.DrawRectangle(brush, null, new(0, 0, w, h));
                    break;
                case MediaPlayer player:
                    dc.DrawVideo(player, new(0, 0, w, h));
                    return;
                case ImageSource image:
                    dc.DrawImage(image, new(0, 0, w, h));
                    return;
                case Drawing drawing:
                    var bounds = drawing.Bounds;
                    var m = Matrix.Identity;
                    m.Translate(-bounds.X, -bounds.Y);
                    m.Scale(w / bounds.Width, h / bounds.Height);
                    MatrixTransform transform = new(m);
                    transform.Freeze();
                    dc.PushTransform(transform);
                    dc.DrawDrawing(drawing);
                    dc.Pop();
                    return;
                default:
                    if (_ft is { } ft)
                    {
                        m = Matrix.Identity;
                        m.Scale(w / ft.Width, h / ft.Height);
                        transform = new(m);
                        transform.Freeze();
                        dc.PushTransform(transform);
                        dc.DrawText(ft, new(0, 0));
                        dc.Pop();
                    }
                    return;
            }

            /*
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
            //var (scaleX, scaleY) = GetScale(dstW, dstH, _src_w, _src_h, _stretch);
            var scaleX = dstW / _src_w;
            var scaleY = dstH / _src_h;
            var ox = (dstW - _src_w * scaleX) * 0.5;
            var oy = (dstH - _src_h * scaleY) * 0.5;
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
             */
        }
    }
}
