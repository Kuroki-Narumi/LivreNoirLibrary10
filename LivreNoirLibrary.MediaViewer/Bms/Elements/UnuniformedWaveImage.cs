using System;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Controls.Wave;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class UnuniformedWaveImage : WaveImageBase
    {
        public static readonly Color DefaultColor = Color.FromRgb(192, 192, 224);

        public static readonly DependencyProperty ScaleXProperty = IScaleProperty.RegisterScaleX<UnuniformedWaveImage>(OnScaleXChanged);
        public static readonly DependencyProperty ScaleYProperty = IScaleProperty.RegisterScaleY<UnuniformedWaveImage>(OnScaleYChanged);

        private static void OnScaleXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UnuniformedWaveImage i)
            {
                i._scaleX = (double)e.NewValue;
                i.UpdateWidth();
            }
        }

        private static void OnScaleYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UnuniformedWaveImage i)
            {
                i._scaleY = (double)e.NewValue;
                i.ReserveRefresh();
            }
        }

        [DependencyProperty]
        private Rational _positionOffset;
        [DependencyProperty]
        private long _sampleOffset;
        [DependencyProperty]
        private TimeCounter? _timeCounter;
        [DependencyProperty]
        private Color _color = DefaultColor;
        [DependencyProperty]
        private int _laneWidth = 1;
        [DependencyProperty]
        private double _bottom;
        private double _scaleX = IScaleProperty.DefaultScale;
        /// <summary>
        /// Represents pixels per beat.
        /// </summary>
        private double _scaleY = IScaleProperty.DefaultScale;

        public double ScaleX { get => _scaleX; set => SetValue(ScaleXProperty, value); }
        public double ScaleY { get => _scaleY; set => SetValue(ScaleYProperty, value); }

        public UnuniformedWaveImage()
        {
            Orientation = WaveOrientation.TopToBottom;
        }

        private void OnPositionOffsetChanged() => ReserveRefresh();
        private void OnSampleOffsetChanged() => ReserveRefresh();
        private void OnTimeCounterChanged() => ReserveRefresh();
        private void OnColorChanged() => ReserveRefresh();
        private void OnLaneWidthChanged() => UpdateWidth();
        private void OnBottomChanged() => ReserveRefresh();

        public void UpdateWidth()
        {
            var w = _laneWidth * _scaleX; ;
            Width = w;
            ReserveRefresh();
        }

        public override void ReserveRefresh()
        {
            base.ReserveRefresh();
            var s = Width / 2;
            if (Source is IWaveBuffer source)
            {
                var (mn, mx) = source.Data.MinMax();
                var normalize = Math.Max(mx, -mn);
                if (normalize is not 0 and < 1)
                {
                    s /= normalize;
                }
            }
            LevelScale = Math.Clamp(s, MinimumLevelScale, MaximumLevelScale);
        }

        public Rational GetBeat(long position, long resolution)
        {
            if (Source is IWaveBuffer source && _timeCounter is TimeCounter t)
            {
                var secondOffset = t.Beat2Second(_positionOffset);
                var beat = t.Second2Beat(secondOffset + new Rational(position - _sampleOffset, source.SampleRate));
                return beat.Round(new Rational(1, resolution));
            }
            return default;
        }

        public Rational GetLastBeat(long resolution)
        {
            if (Source is IWaveBuffer source)
            {
                return GetBeat(source.SampleLength, resolution);
            }
            return default;
        }

        public long GetPosition(Rational position)
        {
            if (Source is IWaveBuffer source && _timeCounter is TimeCounter t)
            {
                var rate = source.SampleRate;
                return (long)((double)t.Interval(_positionOffset, position) * rate) + _sampleOffset;
            }
            return 0;
        }

        protected override unsafe void RenderWaveImage(IWaveBuffer source, int* bitPtr, double offset, int top, int bottom, int bitmapWidth)
        {
            if (_timeCounter is not TimeCounter counter)
            {
                return;
            }
            var data = source.Data;
            var dataLength = data.Length;
            var sampleRate = source.SampleRate;
            var sampleOffset = _sampleOffset;
            var channels = source.Channels;
            var levelScale = LevelScale;
            var cx = bitmapWidth / 2;
            var scaleY = (long)_scaleY;
            var position = _positionOffset;
            var color = ColorOperation.ToInt(_color);
            var contentHeight = _bottom;

            // 参照する位置
            int GetPosition(double offset)
            {
                /**
                 * A = contentHeight - (offset + y) : 描画すべきピクセルの絶対位置
                 * B = A / scaleY : 描画すべき位置(wn)
                 * B - position : 参照する位置(wn)
                 */
                var pos = new Rational((long)(contentHeight - offset), scaleY);
                var sample = (double)counter.Interval(position, pos) * sampleRate + sampleOffset;
                return (int)Math.Clamp(sample * channels, 0, dataLength);
            }
            int GetX(float value) => (int)(value * levelScale) + cx;
            var lastPos = GetPosition(offset + top);
            for (var y = top; y < bottom; y++, bitPtr += bitmapWidth)
            {
                // 描画するサンプル範囲の計算
                var pos = GetPosition(offset + y + 1);
                var length = lastPos - pos;
                new Span<int>(bitPtr, bitmapWidth).Clear();
                if (length is > 0)
                {
                    var (min, max) = data.Slice(pos, length).MinMax();
                    var left = Math.Clamp(GetX(min), 0, cx - 1);
                    var right = Math.Clamp(GetX(max), cx, bitmapWidth);
                    new Span<int>(bitPtr + left, right - left).Fill(color);
                }
                lastPos = pos;
            }
        }
    }
}
