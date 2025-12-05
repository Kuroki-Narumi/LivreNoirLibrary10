using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public sealed class NumberElement(Number source) : ScreenElement(source)
    {
        private readonly Number _source = source;
        private TextureData _digits;
        private TextureData _padding;
        private TextureData _point;
        private TextureData _separator;
        private int _minDigits;

        private string? _value;
        private readonly List<(UIntBitmap, DrRect)> _bitmaps = [];

        public override void DetermineExpressions(Skin skin, IVariableProvider? provider)
        {
            base.DetermineExpressions(skin, provider);

            var s = _source;
            skin.TryGetTexture(s.Digits, provider, out _digits);
            skin.TryGetTexture(s.Padding, provider, out _padding);
            skin.TryGetTexture(s.Point, provider, out _point);
            skin.TryGetTexture(s.Separator, provider, out _separator);
            _minDigits = skin.ResolveValue(s.MinDigits, provider, 0);
            _value = null;
        }

        public override void Update(in UpdateArgs args)
        {
            base.Update(args);
            var cache = args.Textures;
            var bitmaps = _bitmaps;
            bitmaps.Clear();
            if (args.Timer.TryGet(_source.SourceTimer, args.AbsoluteTime, out var relativeTime) &&
                args.Skin.TryResolveReflection(_source.Value, args.VariableProvider, out var value) &&
                cache.TryGetTexture(_digits, BmsTimer.GetFrameIndex(relativeTime, _digits), out var digits, out var digitRect))
            {
                cache.TryGetTexture(_padding, BmsTimer.GetFrameIndex(relativeTime, _padding), out var padding, out var paddingRect);
                cache.TryGetTexture(_point, BmsTimer.GetFrameIndex(relativeTime, _point), out var point, out var pointRect);
                cache.TryGetTexture(_separator, BmsTimer.GetFrameIndex(relativeTime, _separator), out var separator, out var separatorRect);
                var (rx, ry, rw, rh) = digitRect;
                var dw = digitRect.Width / 10;
                var w = 0;
                var valueSpan = value.AsSpan();
                var split = (stackalloc Range[3]);
                var splitCount = valueSpan.Split(split, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                var integer = valueSpan[split[0]];
                var separatorCount = (integer.Length - 1) / 3;
                // (最小桁数 - (値の桁数 + 追加される区切り文字の数)) が正の場合、その分だけ先頭にパディングを追加する
                for (var i = _minDigits - (valueSpan.Length + separatorCount); i > 0; i--)
                {
                    if (padding is not null)
                    {
                        bitmaps.Add((padding, paddingRect));
                    }
                    else
                    {
                        bitmaps.Add((digits, new(0, 0, dw, 0)));
                    }
                    w += paddingRect.Width;
                }
                // 整数部分
                var restDigits = integer.Length;
                foreach (var digit in integer)
                {
                    AddDigit(digit);
                    // 3桁ごとに区切り文字を追加
                    if (separator is not null && --restDigits % 3 is 0)
                    {
                        bitmaps.Add((separator, separatorRect));
                        w += separatorRect.Width;
                    }
                }
                // 小数点
                if (splitCount is >= 2 && point is not null)
                {
                    bitmaps.Add((point, pointRect));
                    w += pointRect.Width;
                    // 小数部分
                    var fractional = valueSpan[split[1]];
                    foreach (var digit in fractional)
                    {
                        AddDigit(digit);
                    }
                }

                DestWidth = w;
                IsVisible = bitmaps.Count > 0;

                void AddDigit(char digit)
                {
                    if (digit is >= '0' and <= '9')
                    {
                        bitmaps.Add((digits, new(rx + dw * (digit - '0'), ry, dw, rh)));
                    }
                    w += dw;
                }
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
            foreach (var (bitmap, rect) in _bitmaps.AsSpan())
            {
                if (rect.Width * rect.Height is > 0)
                {
                    RenderSource(args, bitmap, rect, x, y, rect.Width, h, blend, color);
                }
                x += rect.Width;
            }
        }
    }
}
