using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media
{
    public delegate Vector<float> BlendFunc(Vector<float> back, Vector<float> front);

    public static partial class ColorBlend
    {
        static readonly Vector<float> Epsilon = Vector.Create(ColorUtils.Epsilon);
        static readonly Vector<int> AlphaSelector = Vector.Equals(Vector<float>.One, VectorUtils.CreateRepeating([0, 0, 0, 1f]));
        static readonly Vector<float> Half = Vector.Create(0.5f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Blend(ref Vector<float> back, in Vector<float> backAlpha, in Vector<float> front, in Vector<float> frontAlpha, in BlendFunc blend)
        {
            // Fb, Ff := Porter Duff 演算の定数 (ここでは Fb = 1 - front.A, Ff = 1)
            // C := 合成後の色
            // a := 合成後のアルファ
            // C' := ブレンド後コンポジット前の色
            var factor = backAlpha * (Vector<float>.One - frontAlpha);
            // a = back.A * Fb + front.A * Ff
            var newAlpha = factor + frontAlpha;
            // C' = back.A * blended + (1 - back.A) * front
            var color = backAlpha * blend(back, front) + (Vector<float>.One - backAlpha) * front;
            // C = (back.A * Fb * back + front.A * Ff * C') / a
            color = (factor * back + frontAlpha * color) / (newAlpha + Epsilon);
            // アルファとそれ以外を分けて反映
            back = Vector.ConditionalSelect(AlphaSelector, newAlpha, color);
        }

        public static bool TryGetBlendFunc(BlendMode mode, [MaybeNullWhen(false)] out BlendFunc blendFunc) => _funcs.TryGetValue(mode, out blendFunc);

        private static readonly Dictionary<BlendMode, BlendFunc> _funcs = new()
        {
            [BlendMode.Alpha] = Alpha,
            [BlendMode.Add] = Add,
            [BlendMode.Subtract] = Subtract,
            [BlendMode.Multiply] = Multiply,
            [BlendMode.Screen] = Screen,
            [BlendMode.Overlay] = Overlay,
            [BlendMode.Darken] = Darken,
            [BlendMode.Lighten] = Lighten,
            [BlendMode.ColorDodge] = ColorDodge,
            [BlendMode.ColorBurn] = ColorBurn,
            [BlendMode.HardLight] = HardLight,
            [BlendMode.SoftLight] = SoftLight,
            [BlendMode.Difference] = Difference,
            [BlendMode.Exclusion] = Exclusion,
        };

        public static Vector<float> Alpha(Vector<float> back, Vector<float> front) => front;

        public static Vector<float> Add(Vector<float> back, Vector<float> front) => Vector.Min(back + front, Vector<float>.One);

        public static Vector<float> Subtract(Vector<float> back, Vector<float> front) => Vector.Max(back - front, Vector<float>.Zero);

        public static Vector<float> Multiply(Vector<float> back, Vector<float> front) => back * front;

        public static Vector<float> Screen(Vector<float> back, Vector<float> front)
        {
            var one = Vector<float>.One;
            return one - (one - back) * (one - front);
        }

        public static Vector<float> Overlay(Vector<float> back, Vector<float> front) => HardLight(front, back);

        public static Vector<float> Darken(Vector<float> back, Vector<float> front) => Vector.Min(back, front);

        public static Vector<float> Lighten(Vector<float> back, Vector<float> front) => Vector.Max(back, front);

        public static Vector<float> ColorDodge(Vector<float> back, Vector<float> front)
        {
            var one = Vector<float>.One;
            return Vector.Min(back / (one - front + Epsilon), one);
        }

        public static Vector<float> ColorBurn(Vector<float> back, Vector<float> front)
        {
            var one = Vector<float>.One;
            return one - Vector.Min((one - back) / (front + Epsilon), one);
        }

        public static Vector<float> HardLight(Vector<float> back, Vector<float> front)
        {
            var screen = Screen(back, front * 2 - Vector<float>.One);
            var multiply = Multiply(back, front * 2);
            var condition = Vector.LessThanOrEqual(front, Half);
            return Vector.ConditionalSelect(condition, multiply, screen);
        }

        public static Vector<float> SoftLight(Vector<float> back, Vector<float> front)
        {
            var one = Vector<float>.One;
            var d1 = ((back * 16 - Vector.Create(12f)) * back + Vector.Create(4f)) * back;
            var d2 = Vector.SquareRoot(back);
            var condition = Vector.LessThanOrEqual(back, Vector.Create(0.25f));
            d1 = Vector.ConditionalSelect(condition, d1, d2);
            var v1 = back - (one - front * 2) * back * (one - back);
            var v2 = back + (front * 2 - one) * (d1 - back);
            condition = Vector.LessThanOrEqual(front, Half);
            return Vector.ConditionalSelect(condition, v1, v2);
        }

        public static Vector<float> Difference(Vector<float> back, Vector<float> front) => Vector.Abs(back - front);

        public static Vector<float> Exclusion(Vector<float> back, Vector<float> front) => back + front - back * front * 2;
    }
}
