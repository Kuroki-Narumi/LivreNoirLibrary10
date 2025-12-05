using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public sealed class JudgeElement(Judge source) : ScreenElement(source)
    {
        private readonly Judge _source = source;
        private readonly Dictionary<JudgeType, (TextureData Name, TextureData Combo)> _textures = [];
        private double _padding;
        private UIntBitmap? _nameBitmap;
        private DrRect _nameRect;
        private UIntBitmap? _comboBitmap;
        private readonly List<DrRect> _comboRects = [];

        public override void DetermineExpressions(Skin skin, IVariableProvider? provider)
        {
            base.DetermineExpressions(skin, provider);

            var s = _source;
            Add(JudgeType.Perfect, s.Perfect, s.PerfectCombo);
            Add(JudgeType.Great, s.Great, s.GreatCombo);
            Add(JudgeType.Good, s.Good, s.GoodCombo);
            Add(JudgeType.Bad, s.Bad, s.BadCombo);
            Add(JudgeType.Miss, s.Miss, s.MissCombo);

            void Add(JudgeType type, string? name, string? combo)
            {
                skin.TryGetTexture(name, provider, out var nameData);
                skin.TryGetTexture(combo, provider, out var comboData);
                _textures[type] = (nameData, comboData);
            }

            _padding = skin.ResolveValue(s.Padding, provider, 0d);
        }

        public override void Update(in UpdateArgs args)
        {
            base.Update(args);
            var judge = args.Score;
            var cache = args.Textures;
            var relativeTime = args.AbsoluteTime - judge.LastJudgeTime;
            if ((judge.IsActive || relativeTime < judge.DisplayTime) &&
                _textures.TryGetValue(judge.JudgeType, out var item))
            {
                var w = 0;
                if (cache.TryGetTexture(item.Name, BmsTimer.GetFrameIndex(relativeTime, item.Name), out _nameBitmap, out _nameRect))
                {
                    w += _nameRect.Width;
                }
                var combo = judge.Combo;
                if (combo is > 1 &&
                    cache.TryGetTexture(item.Combo, BmsTimer.GetFrameIndex(relativeTime, item.Combo), out _comboBitmap, out var rect))
                {
                    var (rx, ry, rw, rh) = rect;
                    var cw = rw / 10;
                    var rects = _comboRects;
                    rects.Clear();
                    foreach (var digit in combo.ToString())
                    {
                        var index = digit - '0';
                        rects.Add(new(rx + cw * index, ry, cw, rh));
                        w += cw;
                    }
                }
                else
                {
                    _comboBitmap = null;
                }
                DestWidth = w;
                IsVisible = _nameBitmap is not null || _comboBitmap is not null;
            }
            else
            {
                IsVisible = false;
            }
        }

        protected override void RenderCore(in RenderArgs args)
        {
            var x = DestX - DestWidth * OriginX;
            var y = DestY - DestHeight * OriginY;
            var h = DestHeight;
            var blend = BlendMode;
            var color = OpacityMask;
            if (_nameBitmap is { } name)
            {
                RenderSource(args, name, _nameRect, x, y, _nameRect.Width, h, blend, color);
                x += _nameRect.Width;
            }
            if (_comboBitmap is { } combo)
            {
                if (_nameBitmap is not null)
                {
                    x += _padding;
                }
                foreach (var rect in _comboRects.AsSpan())
                {
                    RenderSource(args, combo, rect, x, y, rect.Width, h, blend, color);
                    x += rect.Width;
                }
            }
        }
    }
}
