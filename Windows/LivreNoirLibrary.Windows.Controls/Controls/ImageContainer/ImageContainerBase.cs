using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract partial class ImageContainerBase : FrameworkElement
    {
        public const Stretch DefaultStretch = Stretch.Uniform;
        public const StretchDirection DefaultStretchDirection = StretchDirection.Both;

        [DependencyProperty(AffectsMeasure = true, AffectsRender = true)]
        private Stretch _stretch = DefaultStretch;
        [DependencyProperty(AffectsMeasure = true, AffectsRender = true)]
        private StretchDirection _stretchDirection = DefaultStretchDirection;

        protected void AttachSourceEvents(ImageSource? bitmap)
        {
            if (bitmap is WriteableBitmap)
            {
                bitmap.Changed += OnSourceBitmapChanged;
            }
            else if (bitmap is BitmapSource { IsDownloading: true } b)
            {
                b.DownloadCompleted += OnSourceDownloadCompleted;
                b.DownloadFailed += OnSourceDownloadCompleted;
                b.DecodeFailed += OnSourceDownloadCompleted;
            }
        }

        protected void DetachSourceEvents(ImageSource? bitmap)
        {
            if (bitmap is WriteableBitmap)
            {
                bitmap.Changed -= OnSourceBitmapChanged;
            }
            else if (bitmap is BitmapSource b)
            {
                b.DownloadCompleted -= OnSourceDownloadCompleted;
                b.DownloadFailed -= OnSourceDownloadCompleted;
                b.DecodeFailed -= OnSourceDownloadCompleted;
            }
        }

        private void OnSourceBitmapChanged(object? sender, EventArgs e)
        {
            InvalidateMeasure();
            InvalidateVisual();
        }

        private void OnSourceDownloadCompleted(object? sender, EventArgs e)
        {
            InvalidateVisual();
            if (sender is BitmapSource b)
            {
                b.DownloadCompleted -= OnSourceDownloadCompleted;
                b.DownloadFailed -= OnSourceDownloadCompleted;
                b.DecodeFailed -= OnSourceDownloadCompleted;
            }
        }

        public abstract Size GetNaturalSize();

        protected override Size MeasureOverride(Size availableSize) => MeasureArrangeHelper(availableSize);
        protected override Size ArrangeOverride(Size finalSize) => MeasureArrangeHelper(finalSize);

        protected Size MeasureArrangeHelper(Size inputSize)
        {
            var naturalSize = GetNaturalSize();
            if (!double.IsFinite(naturalSize.Width) || !double.IsFinite(naturalSize.Height))
            {
                return new(0, 0);
            }
            var scale = Viewbox.ComputeScaleFactor(inputSize, naturalSize, Stretch, StretchDirection);
            return new(naturalSize.Width * scale.Width, naturalSize.Height * scale.Height);
        }
    }
}
