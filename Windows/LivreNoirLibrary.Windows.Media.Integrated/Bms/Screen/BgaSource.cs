using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Numerics;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class BgaSource(IBgaVisibilityProvider provider) : ObservableObjectBase
    {
        public IBgaVisibilityProvider Provider { get; set => SetValue(ref field, value); } = provider;

        public LnColor Background { get; set => SetValue(ref field, value); } = LnColor.FromRgb(0, 0, 0);
        public LnColor Transparent { get; set => SetValue(ref field, value); } = LnColor.FromRgb(0, 0, 0);

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
                var p = Provider;
                var tp = Transparent;
                if (_poor.CheckPoorState(p.ShowBgaMissLayer, timings, args, currentTime, p.MissLayerDisplayTime, tp) && p.HideBgaOnMiss)
                {
                    _base.IsVisible = _layer1.IsVisible = _layer2.IsVisible = false;
                }
                else
                {
                    var cache = args.Media;
                    _base.CheckState(p.ShowBgaBase, timings, cache, currentTime, tp);
                    _layer1.CheckState(p.ShowBgaLayer, timings, cache, currentTime, tp);
                    _layer2.CheckState(p.ShowBgaLayer2, timings, cache, currentTime, tp);
                }
            }
            else
            {
                _base.IsVisible = _layer1.IsVisible = _layer2.IsVisible = _poor.IsVisible = false;
            }
        }

        public void Render(in RenderArgs args)
        {
            args.RenderTarget.Fill(args.Rect.Round(), Background);
            _base.Render(args);
            _layer1.Render(args);
            _layer2.Render(args);
            _poor.Render(args);
        }

        private class LayerState(Channel channel)
        {
            public Channel Channel { get; } = channel;
            public bool IsVisible { get; set; }

            private readonly UIntBitmap _bitmap = new(0, 0);
            private FloatColor _colorCorrection = default;

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
                    _colorCorrection = (FloatColor)vector;
                }
                else
                {
                    _colorCorrection = FloatColor.White;
                }
                var width = source.Width;
                var height = source.Height;
                _bitmap.Resize(width, height);
                SimdOperations.CopyFrom((uint*)_bitmap.Pointer, (uint*)source.Pointer, width * height);
                _bitmap.SetTransparent(transparent);
            }

            public void Render(in RenderArgs args)
            {
                if (IsVisible)
                {
                    var (target, buffer, parentRect, colorCorrection) = args;
                    var source = _bitmap;
                    var originalWidth = source.Width;
                    var originalHeight = source.Height;
                    // ソースサイズが256ピクセル未満の場合は、256ピクセルとみなす
                    var sourceWidth = Math.Max(originalWidth, 256);
                    var sourceHeight = Math.Max(originalHeight, 256);
                    var destWidth = parentRect.Width.RoundToInt();
                    var destHeight = parentRect.Height.RoundToInt();
                    // 拡大率
                    var scale = Math.Min((double)destWidth / sourceWidth, (double)destHeight / sourceHeight);
                    // コピー先の座標(中央揃え)
                    var destX = parentRect.X + (int)((destWidth - sourceWidth * scale) / 2);
                    var destY = parentRect.Y + (int)((destHeight - sourceHeight * scale) / 2);
                    // 実際のコピー先サイズ
                    destWidth = (originalWidth * scale).RoundToInt();
                    destHeight = (originalHeight * scale).RoundToInt();

                    _bitmap.BlendWithScale(_bitmap.Rect, target, parentRect, new(destX, destY, destWidth, destHeight), BlendMode.Alpha, _colorCorrection * colorCorrection, buffer);
                }
            }
        }
    }
}
