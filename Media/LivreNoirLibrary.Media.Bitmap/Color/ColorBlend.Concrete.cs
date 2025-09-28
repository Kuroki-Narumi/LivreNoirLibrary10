using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media
{
    public static partial class ColorBlend
    {
        public static readonly Blend_Alpha Alpha = new();
        public static readonly Blend_Add Add = new();
        public static readonly Blend_Subtract Subtract = new();
        public static readonly Blend_Multiply Multiply = new();
        public static readonly Blend_Screen Screen = new();
        public static readonly Blend_Overlay Overlay = new();
        public static readonly Blend_Darken Darken = new();
        public static readonly Blend_Lighten Lighten = new();
        public static readonly Blend_ColorDodge ColorDodge = new();
        public static readonly Blend_ColorBurn ColorBurn = new();
        public static readonly Blend_HardLight HardLight = new();
        public static readonly Blend_SoftLight SoftLight = new();
        public static readonly Blend_Difference Difference = new();
        public static readonly Blend_Exclusion Exclusion = new();

        public readonly struct Blend_Alpha : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front)
            {
                return front;
            }
        }

        public readonly struct Blend_Add : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front) => Vector.Min(back + front, Vector<float>.One);
        }

        public readonly struct Blend_Subtract : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front) => Vector.Max(back - front, Vector<float>.Zero);
        }

        public readonly struct Blend_Multiply : IColorBlend
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector<float> StaticBlend(Vector<float> back, Vector<float> front) => back * front;

            public Vector<float> Blend(Vector<float> back, Vector<float> front) => StaticBlend(back, front);
        }

        public readonly struct Blend_Screen : IColorBlend
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector<float> StaticBlend(Vector<float> back, Vector<float> front)
            {
                var one = Vector<float>.One;
                return one - (one - back) * (one - front);
            }

            public Vector<float> Blend(Vector<float> back, Vector<float> front) => StaticBlend(back, front);
        }

        public readonly struct Blend_Overlay : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front) => Blend_HardLight.StaticBlend(front, back);
        }

        public readonly struct Blend_Darken : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front) => Vector.Min(back, front);
        }

        public readonly struct Blend_Lighten : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front) => Vector.Max(back, front);
        }

        public readonly struct Blend_ColorDodge : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front)
            {
                var one = Vector<float>.One;
                return Vector.Min(back / (one - front + Epsilon), one);
            }
        }

        public readonly struct Blend_ColorBurn : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front)
            {
                var one = Vector<float>.One;
                return one - Vector.Min((one - back) / (front + Epsilon), one);
            }
        }

        public readonly struct Blend_HardLight : IColorBlend
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector<float> StaticBlend(Vector<float> back, Vector<float> front)
            {
                var screen = Blend_Screen.StaticBlend(back, front * 2 - Vector<float>.One);
                var multiply = Blend_Multiply.StaticBlend(back, front * 2);
                var condition = Vector.LessThanOrEqual(front, RoundOffset);
                return Vector.ConditionalSelect(condition, multiply, screen);
            }

            public Vector<float> Blend(Vector<float> back, Vector<float> front) => StaticBlend(back, front);
        }

        public readonly struct Blend_SoftLight : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front)
            {
                var one = Vector<float>.One;
                var d1 = ((back * 16 - Vector.Create(12f)) * back + Vector.Create(4f)) * back;
                var d2 = Vector.SquareRoot(back);
                var condition = Vector.LessThanOrEqual(back, Vector.Create(0.25f));
                d1 = Vector.ConditionalSelect(condition, d1, d2);
                var v1 = back - (one - front * 2) * back * (one - back);
                var v2 = back + (front * 2 - one) * (d1 - back);
                condition = Vector.LessThanOrEqual(front, RoundOffset);
                return Vector.ConditionalSelect(condition, v1, v2);
            }
        }

        public readonly struct Blend_Difference : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front) => Vector.Abs(back - front);
        }

        public readonly struct Blend_Exclusion : IColorBlend
        {
            public Vector<float> Blend(Vector<float> back, Vector<float> front) => back + front - back * front * 2;
        }
    }
}
