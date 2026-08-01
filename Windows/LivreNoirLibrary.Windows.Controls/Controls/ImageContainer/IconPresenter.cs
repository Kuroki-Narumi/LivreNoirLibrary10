using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Win32Api;
using LivreNoirLibrary.Windows.Converters;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    [ContentProperty(nameof(Source))]
    public partial class IconPresenter : ImageContainerBase
    {
        static IconPresenter()
        {
            IsEnabledProperty.OverrideMetadata(typeof(IconPresenter), PropertyUtils.GetMeta(true, OnIsEnabledChanged));
            ClipToBoundsProperty.OverrideMetadata(typeof(IconPresenter), PropertyUtils.GetMeta(true));
        }

        public static readonly FontFamily DefaultFontFamily = new("Segoe MDL2 Assets");
        public static Brush DefaultForeground => Brushes.Black;
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

        private void OnSourceChanged(object? oldValue, object? newValue)
        {
            DetachSourceEvents(oldValue as ImageSource);
            AttachSourceEvents(newValue as ImageSource);
            UpdateText();
        }

        private void OnSourceBitmapChanged(object? sender, EventArgs e)
        {
            InvalidateMeasure();
            InvalidateVisual();
        }

        private void OnSourceDownloadCompleted(object? sender, EventArgs e)
        {
            InvalidateVisual();
            if (Source is BitmapSource b)
            {
                b.DownloadCompleted -= OnSourceDownloadCompleted;
                b.DownloadFailed -= OnSourceDownloadCompleted;
                b.DecodeFailed -= OnSourceDownloadCompleted;
            }
        }

        private void UpdateText()
        {
            _ft = Source?.ToString()?.CreateFormattedText(_option);
            InvalidateMeasure();
            InvalidateVisual();
        }

        public override Size GetNaturalSize()
        {
            switch (Source)
            {
                case LivreNoirLibrary.Media.VectorGraphics.ElementGroup g:
                    return LnIconConverter.Convert(g).Bounds.Size;
                case Drawing d:
                    return d.Bounds.Size;
                case ImageSource image:
                    return new(image.Width, image.Height);
                case MediaPlayer m:
                    var (dpiX, dpiY) = this.GetDisplayScale();
                    return new(m.NaturalVideoWidth * dpiX, m.NaturalVideoHeight * dpiY);
                case UIElement e:
                    e.Measure(new(double.PositiveInfinity, double.PositiveInfinity));
                    return e.DesiredSize;
                default:
                    return _ft is { } ft ? new(ft.Width, ft.Height) : new(0, 0);
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            var (renderW, renderH) = RenderSize;
            if (renderW is 0 || renderH is 0)
            {
                return;
            }
            if (Background is { } background)
            {
                dc.DrawRectangle(background, null, new(0, 0, renderW, renderH));
            }

            Rect sourceRect = new(0, 0, renderW, renderH);
            switch (Source)
            {
                case LivreNoirLibrary.Media.VectorGraphics.ElementGroup g:
                    DrawDrawing(dc, LnIconConverter.Convert(g), renderW, renderH);
                    return;
                case Drawing drawing:
                    DrawDrawing(dc, drawing, renderW, renderH);
                    return;
                case ImageSource image:
                    dc.DrawImage(image, sourceRect);
                    return;
                case MediaPlayer player:
                    dc.DrawVideo(player, sourceRect);
                    return;
                case UIElement element:
                    VisualBrush brush = new(element)
                    {
                        Stretch = Stretch,
                    };
                    dc.DrawRectangle(brush, null, sourceRect);
                    return;
                default:
                    DrawText(dc, _ft, renderW, renderH);
                    return;
            }
        }

        private static void DrawDrawing(DrawingContext dc, Drawing drawing, double width, double height)
        {
            var (sx, sy, sw, sh) = drawing.Bounds;
            var m = Matrix.Identity;
            m.Scale(width / sw, height / sh);
            MatrixTransform t = new(m);
            t.Freeze();
            dc.PushTransform(t);
            dc.DrawDrawing(drawing);
            dc.Pop();
        }

        private static void DrawText(DrawingContext dc, FormattedText? text, double width, double height)
        {
            if (text is not null)
            {
                var m = Matrix.Identity;
                m.Scale(width / text.Width, height / text.Height);
                MatrixTransform t = new(m);
                t.Freeze();
                dc.PushTransform(t);
                dc.DrawText(text, new(0, 0));
                dc.Pop();
            }
        }
    }
}
