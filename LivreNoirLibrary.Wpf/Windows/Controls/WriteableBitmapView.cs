using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract partial class WriteableBitmapView : FrameworkElement
    {
        public const int DefaultBitmapWidth = 640;
        public const int DefaultBitmapHeight = 640;

        protected double _requiredWidth;
        protected double _requiredHeight;

        protected int _bitmapWidth = DefaultBitmapWidth;
        protected int _bitmapHeight = DefaultBitmapHeight;
        protected WriteableBitmap? _bitmap;

        private bool _needRefresh;

        public BitmapSource? BufferBitmap => _bitmap;

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateRenderSize(sizeInfo.NewSize);
        }

        protected void UpdateRenderSize(Size size) => UpdateRenderSize(size.Width, size.Height);
        protected virtual void UpdateRenderSize(double width, double height)
        {
            bool flag = false;
            if (_requiredWidth != width)
            {
                _requiredWidth = width;
                while (_requiredWidth > _bitmapWidth)
                {
                    _bitmapWidth *= 2;
                    flag = true;
                }
                OnRequiredWidthChanged();
            }
            if (_requiredHeight != height)
            {
                _requiredHeight = height;
                while (_requiredHeight > _bitmapHeight)
                {
                    _bitmapHeight *= 2;
                    flag = true;
                }
                OnRequiredHeightChanged();
            }
            if (flag)
            {
                _bitmap = null;
                ReserveRefresh();
            }
        }

        protected virtual void OnRequiredWidthChanged() { }
        protected virtual void OnRequiredHeightChanged() { }

        public virtual void ReserveRefresh()
        {
            _needRefresh = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (_bitmap is null)
            {
                CreateBitmap();
            }
            if (_needRefresh)
            {
                _needRefresh = false;
                Refresh();
            }
            base.OnRender(dc);
            dc.DrawImage(_bitmap, new(GetBitmapOffsetX(), GetBitmapOffsetY(), _bitmapWidth, _bitmapHeight));
        }

        protected void CreateBitmap()
        {
            _bitmap = Bitmap.Create(_bitmapWidth, _bitmapHeight);
        }

        protected virtual void Refresh() { }
        protected virtual double GetBitmapOffsetX() => 0;
        protected virtual double GetBitmapOffsetY() => 0;
    }
}
