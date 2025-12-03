using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Numerics;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class BgaSource : ObservableObjectBase
    {
        public LnColor Background { get; set => SetValue(ref field, value); } = LnColor.FromRgb(0, 0, 0);
        public LnColor Transparent { get; set => SetValue(ref field, value); } = LnColor.FromRgb(0, 0, 0);

        public BgaShowFlags ShowFlags
        {
            get;
            set => SetValue(ref field, value, [nameof(ShowBaseLayer), nameof(ShowLayer1), nameof(ShowLayer2), nameof(ShowMissLayer), nameof(HideOnMiss)]);
        } = BgaShowFlags.Default;

        public bool ShowBaseLayer { get => GetShowFlag(BgaShowFlags.Base); set => SetShowFlag(BgaShowFlags.Base, value); }
        public bool ShowLayer1 { get => GetShowFlag(BgaShowFlags.Layer1); set => SetShowFlag(BgaShowFlags.Layer1, value); }
        public bool ShowLayer2 { get => GetShowFlag(BgaShowFlags.Layer2); set => SetShowFlag(BgaShowFlags.Layer2, value); }
        public bool ShowMissLayer { get => GetShowFlag(BgaShowFlags.Miss); set => SetShowFlag(BgaShowFlags.Miss, value); }
        public bool HideOnMiss { get => GetShowFlag(BgaShowFlags.HideOnMiss); set => SetShowFlag(BgaShowFlags.HideOnMiss, value); }

        public double MissLayerDisplayTime { get; set => SetValue(ref field, value); } = 0.5;

        private readonly LayerState _base = new(Channel.Bga_Base);
        private readonly LayerState _layer1 = new(Channel.Bga_Layer1);
        private readonly LayerState _layer2 = new(Channel.Bga_Layer2);
        private readonly LayerState _poor = new(Channel.Bga_Poor);

        private bool GetShowFlag(BgaShowFlags flag) => (ShowFlags & flag) is not 0;
        private void SetShowFlag(BgaShowFlags flags, bool value)
        {
            if (value)
            {
                ShowFlags |= flags;
            }
            else
            {
                ShowFlags &= ~flags;
            }
        }

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

                    _bitmap.CopyTo(_bitmap.Rect, target, parentRect, new(destX, destY, destWidth, destHeight), BlendMode.Alpha, _colorCorrection * colorCorrection, buffer);
                }
            }
        }
    }
}
