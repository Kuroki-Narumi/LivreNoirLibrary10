using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media
{
    public readonly struct GradientColorGetter
    {
        private readonly Dictionary<int, LnColor> colors = [];
        private readonly float invertDen;
        private readonly float a1;
        private readonly float h1;
        private readonly float s1;
        private readonly float v1;
        private readonly float a2;
        private readonly float h2;
        private readonly float s2;
        private readonly float v2;

        public GradientColorGetter(LnColor color1, LnColor color2, float den)
        {
            var (a, r, g, b) = color1.ToFloat();
            a1 = a;
            (h1, s1, v1) = ColorUtils.CalcHSV(r, g, b);
            (a, r, g, b) = color2.ToFloat();
            a2 = a;
            (h2, s2, v2) = ColorUtils.CalcHSV(r, g, b);
            colors.Add(0, color1);
            invertDen = 1f / den;
        }

        public LnColor Get(int num)
        {
            if (!colors.TryGetValue(num, out var value))
            {
                var amount = num * invertDen;
                var a = Blend(a1, a2, amount);
                var h = Blend(h1, h2, amount);
                var s = Blend(s1, s2, amount);
                var v = Blend(v1, v2, amount);
                var (r, g, b) = ColorUtils.CalcRGB(h, s, v);
                value = LnColor.FromFloat(a, r, g, b);
                colors.Add(num, value);
            }
            return value;
        }

        private static float Blend(float v1, float v2, float amount) => (1 - amount) * v1 + amount * v2;
    }
}
