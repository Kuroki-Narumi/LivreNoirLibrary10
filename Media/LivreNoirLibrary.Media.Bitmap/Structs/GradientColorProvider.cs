using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;

namespace LivreNoirLibrary.Media
{
    public readonly struct GradientColorProvider
    {
        private readonly Dictionary<int, LnColor>? _cache;
        private readonly Vector128<float> _color1;
        private readonly Vector128<float> _color2;
        private readonly float _den;

        public GradientColorProvider(LnColor color1, LnColor color2, float den, bool cacheEnabled = false)
        {
            var (a, r, g, b) = color1.ToFloat();
            var (h, s, v) = ColorUtils.CalcHSV(r, g, b);
            _color1 = Vector128.Create(a, h, s, v);
            (a, r, g, b) = color2.ToFloat();
            (h, s, v) = ColorUtils.CalcHSV(r, g, b);
            _color2 = Vector128.Create(a, h, s, v);

            _den = 1f / den;
            if (cacheEnabled)
            {
                _cache = [];
                _cache[0] = color1;
            }
        }

        public LnColor Get(int num) => (_cache is { } cache) ? cache.GetOrAdd(num, GetImpl) : GetImpl(num);

        private LnColor GetImpl(int num)
        {
            var amount = Vector128.Create(num * _den);
            var color = _color1 * (Vector128<float>.One - amount) + _color2 * amount;
            var (r, g, b) = ColorUtils.CalcRGB(color[1], color[2], color[3]);
            var value = LnColor.FromFloat(color[0], r, g, b);
            _cache?[num] = value;
            return value;
        }
    }
}
