using System;
using System.Windows.Media;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Wave;

namespace LivreNoirLibrary.Windows.Controls.Wave
{
    public partial class WaveImageBase : WaveBitmapViewBase
    {
        public const double MinimumLevelScale = 1;
        public const double MaximumLevelScale = 8388608;
        public const WaveOrientation DefaultOrientation = WaveOrientation.LeftToRight;

        [DependencyProperty]
        private IWaveBuffer? _source;
        [DependencyProperty]
        private double _offset;
        [DependencyProperty]
        private double _levelScale = 320;
        [DependencyProperty]
        private WaveOrientation _orientation = DefaultOrientation;

        private double _lastOffset = -1;
        private double _lastHeight = -1;

        private void OnSourceChanged() => ReserveRefresh();
        private void OnOffsetChanged() => ReservePartialRefresh();
        private static double CoerceLevelScale(double value) => Math.Clamp(value, MinimumLevelScale, MaximumLevelScale);
        private void OnLevelScaleChanged() => ReserveRefresh();
        private void OnOrientationChanged() => UpdateRenderSize(ActualWidth, ActualHeight);

        public void ReservePartialRefresh() => base.ReserveRefresh();
        public override void ReserveRefresh()
        {
            _lastOffset = -1;
            base.ReserveRefresh();
        }

        protected override void UpdateRenderSize(double width, double height)
        {
            if (_orientation is <= WaveOrientation.RightToLeft)
            {
                (width, height) = (height, width);
            }
            base.UpdateRenderSize(width, height);
        }

        protected override void OnRequiredWidthChanged() => InvalidateVisual();
        protected override void OnRequiredHeightChanged() => ReserveRefresh();

        protected override void UpdateMatrix(ref Matrix matrix)
        {
            var offset = Math.Ceiling((_requiredWidth - _bitmapWidth) / 2 + 0.5);
            switch (_orientation)
            {
                case WaveOrientation.LeftToRight:
                    matrix.Rotate(-90);
                    matrix.OffsetY = offset + _bitmapWidth;
                    break;
                case WaveOrientation.RightToLeft:
                    matrix.Scale(1, -1);
                    matrix.Rotate(-90);
                    matrix.OffsetX = _requiredHeight;
                    matrix.OffsetY = offset + _bitmapWidth;
                    break;
                case WaveOrientation.TopToBottom:
                    matrix.OffsetX = offset;
                    break;
                case WaveOrientation.BottomToTop:
                    matrix.Scale(1, -1);
                    matrix.OffsetX = offset;
                    matrix.OffsetY = _requiredHeight;
                    break;
            }
        }

        protected override double GetBitmapOffsetY() => Math.Round(_offset);

        protected override unsafe void Refresh()
        {
            if (_source is null || _bitmap is null) { return; }
            var source = _source;
            var offset = _offset;

            var lastOffset = _lastOffset;
            var bitmap = _bitmap;
            var bitmapWidth = _bitmapWidth;
            var stride = bitmap.BackBufferStride;
            var h = (int)Math.Ceiling(_requiredHeight);

            try
            {
                bitmap.Lock();
                // ピクセル(4byte)単位で操作するためのポインタ
                var bitPtr = (uint*)bitmap.BackBuffer;
                var top = 0;
                var bottom = h;
                // 前回描画した部分と重複する領域の除外
                if (lastOffset is >= 0)
                {
                    var dif = (int)(offset - lastOffset);
                    if (dif is > 0) // 右に移動した
                    {
                        var remain = h - dif;
                        if (remain is > 0)
                        {
                            top = remain;
                            dif *= stride;
                            remain *= stride;
                            bitmap.MoveMemory(dif, remain);
                        }
                    }
                    else if (dif is < 0) // 左に移動した
                    {
                        var remain = h + dif;
                        if (remain is > 0)
                        {
                            bottom = -dif;
                            dif *= stride;
                            remain *= stride;
                            bitmap.MoveMemory(dif, remain);
                        }
                    }
                    else // 移動していない
                    {
                        top = (int)_lastHeight;
                    }
                }

                // 描画開始位置
                bitPtr += top * bitmapWidth;
                RenderWaveImage(source, bitPtr, offset, top, bottom, bitmapWidth);
            }
            finally
            {
                _lastHeight = _requiredHeight;
                _lastOffset = offset;
                _bitmap.AddDirtyRect(new(0, 0, bitmapWidth, h));
                _bitmap.Unlock();
            }
        }

        protected virtual unsafe void RenderWaveImage(IWaveBuffer source, uint* bitPtr, double offset, int top, int bottom, int bitmapWidth)
        {
        }
    }
}
