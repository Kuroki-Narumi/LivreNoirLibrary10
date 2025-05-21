using System;
using System.Windows.Media;
using Drawing = System.Drawing;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media
{
    public readonly partial struct HsvColor : 
        IEquatable<HsvColor>, IEquatable<Color>, IEquatable<Drawing.Color>,
        IEqualityOperators<HsvColor, HsvColor, bool>, IEqualityOperators<HsvColor, Color, bool>, IEqualityOperators<HsvColor, Drawing.Color, bool>,
        IAdditionOperators<HsvColor, HsvColor, HsvColor>, IAdditionOperators<HsvColor, Color, HsvColor>, IAdditionOperators<HsvColor, Drawing.Color, HsvColor>,
        ISubtractionOperators<HsvColor, HsvColor, HsvColor>, ISubtractionOperators<HsvColor, Color, HsvColor>, ISubtractionOperators<HsvColor, Drawing.Color, HsvColor>,
        IMultiplyOperators<HsvColor, HsvColor, HsvColor>, IMultiplyOperators<HsvColor, Color, HsvColor>, IMultiplyOperators<HsvColor, Drawing.Color, HsvColor>,
        IDivisionOperators<HsvColor, HsvColor, HsvColor>, IDivisionOperators<HsvColor, Color, HsvColor>, IDivisionOperators<HsvColor, Drawing.Color, HsvColor>
    {
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj switch
            {
                HsvColor c => Equals(c),
                Color c => Equals(c),
                Drawing.Color c => Equals(c),
                _ => false
            };
        }

        public override int GetHashCode() => ToColor().GetHashCode();

        public bool Equals(HsvColor other) => A == other.A && R == other.R && G == other.G && B == other.B;
        public bool Equals(Color other) => A == other.A && R == other.R && G == other.G && B == other.B;
        public bool Equals(Drawing.Color other) => A == other.A && R == other.R && G == other.G && B == other.B;
        public static bool operator ==(HsvColor left, HsvColor right) => left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B;
        public static bool operator !=(HsvColor left, HsvColor right) => left.A != right.A || left.R != right.R || left.G != right.G || left.B == right.B;
        public static bool operator ==(HsvColor left, Color right) => left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B;
        public static bool operator !=(HsvColor left, Color right) => left.A != right.A || left.R != right.R || left.G != right.G || left.B == right.B;
        public static bool operator ==(HsvColor left, Drawing.Color right) => left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B;
        public static bool operator !=(HsvColor left, Drawing.Color right) => left.A != right.A || left.R != right.R || left.G != right.G || left.B == right.B;
        public static bool operator ==(Color left, HsvColor right) => left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B;
        public static bool operator !=(Color left, HsvColor right) => left.A != right.A || left.R != right.R || left.G != right.G || left.B == right.B;
        public static bool operator ==(Drawing.Color left, HsvColor right) => left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B;
        public static bool operator !=(Drawing.Color left, HsvColor right) => left.A != right.A || left.R != right.R || left.G != right.G || left.B == right.B;

        public static HsvColor operator +(HsvColor left, HsvColor right)
        {
            return FromArgb(left.A, left.R + right.R * right.A, left.G + right.G * right.A, left.B + right.B * right.A);
        }
        public static HsvColor operator +(HsvColor left, Color right) => left + new HsvColor(right);
        public static HsvColor operator +(HsvColor left, Drawing.Color right) => left + new HsvColor(right);
        public static HsvColor operator +(Color left, HsvColor right) => new HsvColor(left) + right;
        public static HsvColor operator +(Drawing.Color left, HsvColor right) => new HsvColor(left) + right;

        public static HsvColor operator -(HsvColor left, HsvColor right)
        {
            return FromArgb(left.A, left.R - right.R * right.A, left.G - right.G * right.A, left.B - right.B * right.A);
        }
        public static HsvColor operator -(HsvColor left, Color right) => left - new HsvColor(right);
        public static HsvColor operator -(HsvColor left, Drawing.Color right) => left - new HsvColor(right);
        public static HsvColor operator -(Color left, HsvColor right) => new HsvColor(left) - right;
        public static HsvColor operator -(Drawing.Color left, HsvColor right) => new HsvColor(left) - right;

        public static HsvColor operator *(HsvColor left, float right)
        {
            return FromArgb(left.A * right, left.R * right, left.G * right, left.B * right);
        }
        public static HsvColor operator *(HsvColor left, HsvColor right)
        {
            return FromArgb(left.A * right.A, left.R * right.R, left.G * right.G, left.B * right.B);
        }
        public static HsvColor operator *(HsvColor left, Color right) => left * new HsvColor(right);
        public static HsvColor operator *(HsvColor left, Drawing.Color right) => left * new HsvColor(right);
        public static HsvColor operator *(Color left, HsvColor right) => new HsvColor(left) * right;
        public static HsvColor operator *(Drawing.Color left, HsvColor right) => new HsvColor(left) * right;

        public static HsvColor operator /(HsvColor left, HsvColor right)
        {
            var (a1, r1, g1, b1) = left;
            var (a2, r2, g2, b2) = right;
            if (a2 is > 0)
            {
                a1 /= a2;
            }
            if (r2 is > 0)
            {
                r1 /= r2;
            }
            if (g2 is > 0)
            {
                g1 /= g2;
            }
            if (b2 is > 0)
            {
                b1 /= b2;
            }
            return FromArgb(a1, r1, g1, b1);
        }
        public static HsvColor operator /(HsvColor left, Color right) => left * new HsvColor(right);
        public static HsvColor operator /(HsvColor left, Drawing.Color right) => left / new HsvColor(right);
        public static HsvColor operator /(Color left, HsvColor right) => new HsvColor(left) / right;
        public static HsvColor operator /(Drawing.Color left, HsvColor right) => new HsvColor(left) / right;
    }
}
