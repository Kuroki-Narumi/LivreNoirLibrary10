using System;
using System.Buffers;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Windows.Controls.Wave;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class WaveImage : WaveImageBase
    {
        public const double MinimumSamplesPerPixel = 1;

        [DependencyProperty]
        private double _samplesPerPixel = MinimumSamplesPerPixel;

        private static double CoerceSamplesPerPixel(double value) => Math.Max(value, MinimumSamplesPerPixel);

        protected override unsafe void RenderWaveImage(IWaveBuffer source, uint* bitPtr, double offset, int top, int bottom, int bitmapWidth)
        {
            var channels = Math.Min(source.Channels, 2);
            var levelScale = LevelScale;
            var timeScale = _samplesPerPixel;
            var intSamplesPerPixel = (int)timeScale;
            var cx = bitmapWidth / 2;

            // 描画用のピクセルデータ
            var colors = stackalloc uint[2];
            colors[0] = ColorOperation.Mask_Red | ColorOperation.Mask_Alpha;
            colors[1] = ColorOperation.Mask_Blue | ColorOperation.Mask_Alpha;

            int GetX(float value) => (int)(value * levelScale) + cx;

            var buffer = ArrayPool<float>.Shared.Rent(intSamplesPerPixel);
            var bufferSpan = buffer.AsSpan(0, intSamplesPerPixel);
            try
            {
                for (var y = top; y < bottom; y++, bitPtr += bitmapWidth)
                {
                    // この一列の内容をクリア
                    SimdOperations.Clear(bitPtr, bitmapWidth);
                    // 参照するサンプル位置
                    var pos = ((offset + y) * timeScale).RoundToInt();
                    for (int c = 0; c < channels; c++)
                    {
                        // チャンネルごとのこの区間のサンプルを取得
                        source.GetChannel(bufferSpan, c, pos);
                        var (min, max) = bufferSpan.MinMax();
                        var left = Math.Clamp(GetX(min), 0, cx);
                        var right = Math.Clamp(GetX(max), cx, bitmapWidth);
                        SimdOperations.Or(bitPtr + left, colors[c], right - left);
                    }
                }
            }
            finally
            {
                ArrayPool<float>.Shared.Return(buffer);
            }
        }
    }
}
