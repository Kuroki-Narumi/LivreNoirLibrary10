using System;
using System.Buffers;
using System.Windows;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Wave
{
    public partial class WaveImage : LeftToRightBitmapView, IWaveSourceProperty, IPixelOffsetProperty
    {
        public const double MinimumLevelScale = 1;
        public const double MaximumLevelScale = 8388608;
        public const double MinimumSamplesPerPixel = 1;

        public static readonly DependencyProperty SourceProperty = WaveProperties.SourceProperty.AddOwner(typeof(WaveImage));
        public static readonly DependencyProperty PixelOffsetProperty = WaveProperties.PixelOffsetProperty.AddOwner(typeof(WaveImage));

        private IWaveBuffer? _source;
        private double _pixelOffset;
        [DependencyProperty]
        private double _levelScale = IScaleProperty.DefaultScale;
        [DependencyProperty]
        private double _samplesPerPixel = MinimumSamplesPerPixel;

        private double _lastPixelOffset = -1;
        private double _lastHeight = -1;

        public IWaveBuffer? Source { get => _source; set => SetValue(SourceProperty, value); }
        public double PixelOffset { get => _pixelOffset; set => SetValue(PixelOffsetProperty, value); }

        private static double CoerceLevelScale(double value) => Math.Clamp(value, MinimumLevelScale, MaximumLevelScale);
        private static double CoerceSamplesPerPixel(double value) => Math.Max(value, MinimumSamplesPerPixel);

        void IWaveSourceProperty.OnSourceChanged(IWaveBuffer? data)
        {
            _source = data;
            ReserveRefresh();
        }

        void IPixelOffsetProperty.OnPixelOffsetChanged(double value)
        {
            _pixelOffset = value;
            ReserveRefresh_Partial();
        }

        private void OnLevelScaleChanged() => ReserveRefresh();
        private void OnSamplesPerPixelChanged() => ReserveRefresh();

        public override void ReserveRefresh()
        {
            _lastPixelOffset = -1;
            base.ReserveRefresh();
        }

        public void ReserveRefresh_Partial() => base.ReserveRefresh();

        protected override void OnRequiredWidthChanged(double value) => InvalidateVisual();
        protected override void OnRequiredHeightChanged(double value) => ReserveRefresh();

        protected override unsafe void Refresh()
        {
            if (_source is not { } source || Bitmap is not { } b)
            {
                return;
            }
            var offset = _pixelOffset;
            using var bitmap = b.BeginWrite();
            var stride = bitmap.Width;
            var h = (int)RequiredHeight;
            AdjustRefreshArea((int)_lastHeight, h, offset, _lastPixelOffset, bitmap, out var top, out var bottom);

            // 新規描画
            var channels = Math.Min(source.Channels, 2);
            var levelScale = _levelScale;
            var timeScale = _samplesPerPixel;
            var intTimeScale = (int)timeScale;
            var limitX = (int)RequiredWidth;
            var centerX = limitX / 2;
            // 描画用のピクセルデータ
            var colors = stackalloc uint[2];
            colors[0] = ColorUtils.GetMask(ColorFlags.R | ColorFlags.A);
            colors[1] = ColorUtils.GetMask(ColorFlags.B | ColorFlags.A);

            int GetX(float value) => (int)(value * levelScale) + centerX;

            var buffer = ArrayPool<float>.Shared.Rent(intTimeScale);
            try
            {
                var bufferSpan = buffer.AsSpan(0, intTimeScale);
                var ptr = (uint*)bitmap.Offset(top);
                for (var y = top; y < bottom; y++, ptr += stride)
                {
                    // この一列の内容をクリア
                    SimdOperations.Clear(ptr, stride);
                    // 参照するサンプル位置
                    var pos = ((offset + y) * timeScale).RoundToInt();
                    for (var c = 0; c < channels; c++)
                    {
                        // チャンネルごとのこの区間のサンプルを取得
                        source.GetChannel(bufferSpan, c, pos);
                        var (min, max) = bufferSpan.MinMax();
                        var left = Math.Clamp(GetX(min), 0, centerX);
                        var right = Math.Clamp(GetX(max), centerX, limitX);
                        SimdOperations.Or(ptr + left, colors[c], right - left);
                    }
                }
            }
            finally
            {
                ArrayPool<float>.Shared.Return(buffer);
            }
            _lastHeight = RequiredHeight;
            _lastPixelOffset = offset;
        }

        /// <summary>
        /// 前回描画した部分と重複する領域の除外
        /// </summary>
        public static unsafe void AdjustRefreshArea(int lastBottom, int requiredHeight, double offset, double lastOffset, BitmapPointer bitmap, out int top, out int bottom)
        {
            var pointer = (uint*)bitmap.Pointer;
            top = 0;
            bottom = requiredHeight;
            if (lastOffset is >= 0)
            {
                var w = bitmap.Width;
                var dif = (int)(offset - lastOffset);
                if (dif is > 0) // 右に移動した
                {
                    var remain = requiredHeight - dif;
                    if (remain is > 0)
                    {
                        top = remain;
                        remain *= w * 4;
                        Buffer.MemoryCopy(pointer, pointer + dif * w, remain, remain);
                    }
                }
                else if (dif is < 0) // 左に移動した
                {
                    var remain = requiredHeight + dif;
                    if (remain is > 0)
                    {
                        bottom = -dif;
                        remain *= w;
                        Buffer.MemoryCopy(pointer - dif * w, pointer, remain, remain);
                    }
                }
                else // 移動していない
                {
                    top = lastBottom;
                }
            }
        }
    }
}
