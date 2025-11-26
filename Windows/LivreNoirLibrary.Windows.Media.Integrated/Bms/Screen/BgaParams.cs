using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;
using System;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class BgaParams : ObservableObjectBase
    {
        public LnColor Background { get; set => SetValue(ref field, value); }
        public LnColor Transparent { get; set => SetValue(ref field, value); }

        public bool ShowBaseLayer { get; set => SetValue(ref field, value); } = true;
        public bool ShowLayer1 { get; set => SetValue(ref field, value); } = true;
        public bool ShowLayer2 { get; set => SetValue(ref field, value); } = true;
        public bool ShowMissLayer { get; set => SetValue(ref field, value); } = true;
        public bool HideOnMiss { get; set => SetValue(ref field, value); }
        public double MissLayerDisplayTime { get; set => SetValue(ref field, value); } = 0.5;

        private readonly LayerState _base = new(Channel.Bga_Base);
        private readonly LayerState _layer1 = new(Channel.Bga_Layer1);
        private readonly LayerState _layer2 = new(Channel.Bga_Layer2);
        private readonly LayerState _poor = new(Channel.Bga_Poor);

        public void Setup()
        {
            _base.IsVisible = _layer1.IsVisible = _layer2.IsVisible = _poor.IsVisible = false;
        }

        public void Update(in UpdateArgs args)
        {
            var timings = args.Timings;
            if (args.Timer.TryGet(TimerId.Play_MusicStart, args.AbsoluteTime, out var currentTime))
            {
                var tp = Transparent;
                if (_poor.CheckPoorState(ShowMissLayer, timings, args, currentTime, MissLayerDisplayTime, tp) && HideOnMiss)
                {
                    _base.IsVisible = _layer1.IsVisible = _layer2.IsVisible = false;
                }
                else
                {
                    var cache = args.Media;
                    _base.CheckState(ShowBaseLayer, timings, cache, currentTime, tp);
                    _layer1.CheckState(ShowLayer1, timings, cache, currentTime, tp);
                    _layer2.CheckState(ShowLayer2, timings, cache, currentTime, tp);
                }
            }
            else
            {
                _base.IsVisible = _layer1.IsVisible = _layer2.IsVisible = _poor.IsVisible = false;
            }
        }

        public void Render(IBitmap target, FloatBitmap buffer1, UnmanagedArray<float> buffer2)
        {
            target.Fill(Background);
            _base.Render(target, buffer1, buffer2);
            _layer1.Render(target, buffer1, buffer2);
            _layer2.Render(target, buffer1, buffer2);
            _poor.Render(target, buffer1, buffer2);
        }

        private class LayerState(Channel channel)
        {
            public Channel Channel { get; } = channel;
            public bool IsVisible { get; set; }

            private readonly UIntBitmap _bitmap = new(0, 0);
            private System.Numerics.Vector<float> _colorCorrection = default;

            public bool CheckPoorState(bool show, TimingList timings, in UpdateArgs args, double currentTime, double duration, LnColor transparent)
            {
                if (show &&
                    args.Timer.TryGet(TimerId.Play_Miss, args.AbsoluteTime, out var elapsed) &&
                    elapsed < duration &&
                    timings.TryGetBgaLayer(Channel, currentTime, out _, out var path) &&
                    args.Media.TryGetBitmap(path, elapsed, out var source))
                {
                    ApplyArgb(timings, currentTime, source, transparent);
                }
                else
                {
                    IsVisible = false;
                }
                return IsVisible;
            }

            public void CheckState(bool show, TimingList timings, MediaCache cache, double currentTime, LnColor transparent)
            {
                if (show &&
                    timings.TryGetBgaLayer(Channel, currentTime, out var start, out var path) &&
                    cache.TryGetBitmap(path, currentTime - start, out var source))
                {
                    ApplyArgb(timings, currentTime, source, transparent);
                }
                else
                {
                    IsVisible = false;
                }
            }

            private unsafe void ApplyArgb(TimingList timings, double currentTime, UIntBitmap source, LnColor transparent)
            {
                IsVisible = true;
                if (timings.TryGetColorCorrection(Channel, currentTime, out var vector))
                {
                    _colorCorrection = vector;
                }
                else
                {
                    _colorCorrection = System.Numerics.Vector<float>.One;
                }
                var width = source.Width;
                var height = source.Height;
                _bitmap.Resize(width, height);
                SimdOperations.CopyFrom((uint*)_bitmap.Pointer, (uint*)source.Pointer, width * height);
                _bitmap.SetTransparent(transparent);
            }

            public void Render(IBitmap target, FloatBitmap buffer1, UnmanagedArray<float> buffer2)
            {
                if (IsVisible)
                {
                    var source = _bitmap;
                    var originalWidth = source.Width;
                    var originalHeight = source.Height;
                    // ソースサイズが256ピクセル未満の場合は、256ピクセルとみなす
                    var sourceWidth = Math.Max(originalWidth, 256);
                    var sourceHeight = Math.Max(originalHeight, 256);
                    var destWidth = target.Width;
                    var destHeight = target.Height;
                    // 拡大率
                    var scale = Math.Min((double)destWidth / sourceWidth, destHeight / sourceHeight);
                    // コピー先の座標(中央揃え)
                    var destX = (int)((destWidth - sourceWidth * scale) / 2);
                    var destY = (int)((destHeight - sourceHeight * scale) / 2);
                    // 実際のコピー先サイズ
                    destWidth = (int)(originalWidth * scale);
                    destHeight = (int)(originalHeight * scale);

                    buffer1.Resize(destWidth, destHeight, false);
                    _bitmap.StretchCopy(buffer1, buffer2);
                    target.Blend(buffer1, new System.Drawing.Point(destX, destY), BlendMode.Alpha, _colorCorrection);
                }
            }
        }
    }
}
