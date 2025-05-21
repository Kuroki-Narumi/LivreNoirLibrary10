using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public static class FrameRates
    {
        public static readonly Rational Fps10 = new(10, 1);
        public static readonly Rational Fps12 = new(12, 1);
        public static readonly Rational Fps15 = new(15, 1);
        public static readonly Rational Fps23_976 = new(24000, 1001);
        public static readonly Rational Fps24 = new(24, 1);
        public static readonly Rational Fps25 = new(25, 1);
        public static readonly Rational Fps29_97 = new(30000, 1001);
        public static readonly Rational Fps30 = new(30, 1);
        public static readonly Rational Fps50 = new(50, 1);
        public static readonly Rational Fps59_94 = new(60000, 1001);
        public static readonly Rational Fps60 = new(60, 1);
        public static readonly Rational Fps120 = new(120, 1);
        public static readonly Rational Fps144 = new(144, 1);
        public static readonly Rational Fps240 = new(240, 1);
    }
}
