using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Converters;
using LivreNoirLibrary.Windows.Media;
using System;
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
            ClipToBoundsProperty.OverrideMetadata(typeof(IconPresenter), PropertyUtils.GetMeta(true));
        }

        public static readonly FontFamily DefaultFontFamily = new("Segoe MDL2 Assets");
        public static Brush DefaultForeground => Brushes.Black;
        public const Stretch DefaultStretch = Stretch.Uniform;
        public const StretchDirection DefaultStretchDirection = StretchDirection.Both;
        public const double DefaultSize = 32;

        public static readonly DependencyProperty BackgroundProperty = Control.BackgroundProperty.AddOwner(typeof(IconPresenter));
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
                i.UpdateText();
            }
        }

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconPresenter i)
            {
                i._option.Foreground = (e.NewValue as Brush) ?? DefaultForeground;
                i.UpdateText();
            }
        }

        private readonly FormattedTextOptions _option = new() { FontFamily = DefaultFontFamily, FontSize = DefaultSize, Foreground = DefaultForeground };
        private FormattedText? _ft;

        [DependencyProperty(AffectsMeasure = true, AffectsRender = true)]
        private object? _source;
        [DependencyProperty(AffectsMeasure = true, AffectsRender = true)]
        private Stretch _stretch = DefaultStretch;
        [DependencyProperty(AffectsMeasure = true, AffectsRender = true)]
        private StretchDirection _stretchDirection = DefaultStretchDirection;
        [DependencyProperty(AffectsRender = true)]
        private AlignmentX _alignmentX = AlignmentX.Center;
        [DependencyProperty(AffectsRender = true)]
        private AlignmentY _alignmentY = AlignmentY.Center;

        public Brush? Background { get => GetValue(BackgroundProperty) as Brush; set => SetValue(BackgroundProperty, value); }

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

        private void OnSourceChanged() => UpdateText();

        private void UpdateText()
        {
            _ft = Source?.ToString()?.CreateFormattedText(_option);
            InvalidateMeasure();
            InvalidateVisual();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return MeasureArrangeHelper(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = MeasureArrangeHelper(finalSize);
            RenderSize = size;
            return size;
        }

        private Size MeasureArrangeHelper(Size inputSize)
        {
            var (sourceW, sourceH) = GetContentSize();
            var size = ComputeSize(Width, Height, inputSize.Width, inputSize.Height, sourceW, sourceH, Stretch, StretchDirection);
            return size;
        }

        public Size GetContentSize() => Source switch
        {
            LivreNoirLibrary.Media.VectorGraphics.ElementGroup g => LnIconConverter.Convert(g).Bounds.Size,
            Drawing drawing => drawing.Bounds.Size,
            ImageSource image => new(image.Width, image.Height),
            MediaPlayer mp => ApplyDisplayScale(mp.NaturalVideoWidth, mp.NaturalVideoHeight),
            UIElement element => element.DesiredSize,
            _ => _ft is { } ft ? new(ft.Width, ft.Height) : new(0, 0),
        };

        private Size ApplyDisplayScale(double w, double h)
        {
            var (dpiX, dpiY) = this.GetDisplayScale();
            return new(w * dpiX, h * dpiY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double ValidateScale(double value, StretchDirection stretchDirection) => stretchDirection switch
        {
            StretchDirection.UpOnly => Math.Max(value, 1),
            StretchDirection.DownOnly => Math.Min(value, 1),
            _ => value,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double ApplyStretch(double input, double source, double refScale, Stretch stretch, StretchDirection stretchDirection) => stretch switch
        {
            Stretch.None => source == 0 ? input.Validate(0) : source, // sourceが確定していればその値、未確定ならinput
            Stretch.Fill or Stretch.UniformToFill => input.Validate(source), // inputが有限ならその値、無限ならsource(0になる可能性あり)
            Stretch.Uniform => Math.Min(source * ValidateScale(refScale, stretchDirection), input).Validate(0), // 与えられた拡大率に合わせるが、inputを上限とする
            _ => 0,
        };

        public static Size ComputeSize(double specifiedW, double specifiedH, double inputW, double inputH, double sourceW, double sourceH, Stretch stretch, StretchDirection stretchDirection)
        {
            /**
             * 想定されるパターン
             * specified: 有限値 or NaN(未指定)
             * input: 有限値 or +Infinity(無制限)
             * source: 有限値 or 0(決定不能)
             * 
             * 返すべき値
             * specified が有限 -> specified
             * source が 0 -> input が無限なら 0, それ以外は input
             * それ以外 -> source と input の小さい方
             */
            double w = 0, h = 0;
            if (double.IsFinite(specifiedW))
            {
                w = specifiedW;
                if (double.IsFinite(specifiedH)) // 全て指定
                {
                    h = specifiedH;
                }
                else // 幅のみ指定
                {
                    h = ApplyStretch(inputH, sourceH, specifiedW / sourceW, stretch, stretchDirection);
                }
            }
            else
            {
                if (double.IsFinite(specifiedH)) // 高さのみ指定
                {
                    w = ApplyStretch(inputW, sourceW, specifiedH / sourceH, stretch, stretchDirection);
                    h = specifiedH;
                }
                else // どちらも未定義
                {
                    switch (stretch)
                    {
                        case Stretch.None:
                            w = sourceW;
                            h = sourceH;
                            break;
                        case Stretch.Fill:
                        case Stretch.UniformToFill:
                            w = inputW.Validate(sourceW);
                            h = inputH.Validate(sourceH);
                            break;
                        case Stretch.Uniform:
                            var scale = Math.Min(
                                sourceW == 0 ? 1 : inputW / sourceW,
                                sourceH == 0 ? 1 : inputH / sourceH
                                );
                            scale = ValidateScale(scale, stretchDirection).Validate(1);
                            w = sourceW * scale;
                            h = sourceH * scale;
                            break;
                    }
                }
            }
            return new(w, h);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            var (renderW, renderH) = RenderSize;

            if (Background is { } background)
            {
                dc.DrawRectangle(background, null, new(0, 0, renderW, renderH));
            }

            Action<DrawingContext> renderAction;
            Rect sourceRect;
            switch (Source)
            {
                case LivreNoirLibrary.Media.VectorGraphics.ElementGroup g:
                    var gd = LnIconConverter.Convert(g);
                    sourceRect = gd.Bounds;
                    renderAction = dc => dc.DrawDrawing(gd);
                    break;
                case Drawing drawing:
                    sourceRect = drawing.Bounds;
                    renderAction = dc => dc.DrawDrawing(drawing);
                    break;
                case ImageSource image:
                    sourceRect = new(0, 0, image.Width, image.Height);
                    renderAction = dc => dc.DrawImage(image, sourceRect);
                    break;
                case MediaPlayer player:
                    sourceRect = new(new Point(0, 0), ApplyDisplayScale(player.NaturalVideoWidth, player.NaturalVideoHeight));
                    renderAction = dc => dc.DrawVideo(player, sourceRect);
                    break;
                case UIElement element:
                    sourceRect = new(0, 0, 1, 1);
                    renderAction = dc =>
                    {
                        VisualBrush brush = new(element)
                        {
                            Stretch = Stretch,
                            AlignmentX = AlignmentX.Center,
                            AlignmentY = AlignmentY.Center,
                        };
                        dc.DrawRectangle(brush, null, sourceRect);
                    };
                    break;
                default:
                    if (_ft is { } ft)
                    {
                        sourceRect = new(0, 0, ft.Width, ft.Height);
                        renderAction = dc => dc.DrawText(ft, new(0, 0));
                        break;
                    }
                    else
                    {
                        return;
                    }
            }

            var stretch = Stretch;
            var stretchDirection = StretchDirection;
            var alignX = (int)AlignmentX;
            var alignY = (int)AlignmentY;
            var sx = renderW / sourceRect.Width;
            var sy = renderH / sourceRect.Height;
            switch (stretch)
            {
                case Stretch.None:
                    sx = sy = 1;
                    break;
                case Stretch.Uniform:
                    sx = sy = Math.Min(sx, sy);
                    break;
                case Stretch.UniformToFill:
                    sx = sy = Math.Max(sx, sy);
                    break;
            }
            sx = ValidateScale(sx, stretchDirection);
            sy = ValidateScale(sy, stretchDirection);
            var actualW = sourceRect.Width * sx;
            var actualH = sourceRect.Height * sy;
            var offsetX = (renderW - actualW) * alignX * 0.5;
            var offsetY = (renderH - actualH) * alignY * 0.5;
            var m = Matrix.Identity;
            m.Translate(-sourceRect.X, -sourceRect.Y);
            m.Scale(sx, sy);
            m.Translate(offsetX, offsetY);
            MatrixTransform t = new(m);
            t.Freeze();
            dc.PushTransform(t);
            renderAction(dc);
            dc.Pop();
        }
    }
}
