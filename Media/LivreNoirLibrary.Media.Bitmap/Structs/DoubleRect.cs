using LivreNoirLibrary.Numerics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media
{
    public readonly struct DoubleRect(double x, double y, double width, double height) : IEquatable<DoubleRect>
    {
        public readonly double X = x;
        public readonly double Y = y;
        public readonly double Width = width;
        public readonly double Height = height;

        public bool IsEmpty => Width is <= 0 || Height is <= 0;

        public System.Drawing.Rectangle Round() => new(X.RoundToInt(), Y.RoundToInt(), Width.RoundToInt(), Height.RoundToInt());

        public static explicit operator System.Drawing.Rectangle(in DoubleRect rect) => rect.Round();
        public static implicit operator DoubleRect(in System.Drawing.Rectangle rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        public void Deconstruct(out double x, out double y, out double width, out double height)
        {
            x = X;
            y = Y;
            width = Width; 
            height = Height;
        }

        public DoubleRect Intersect(in DoubleRect rect)
        {
            var left = Math.Max(X, rect.X);
            var right = Math.Min(X + Width, rect.X + rect.Width);
            var top = Math.Max(Y, rect.Y);
            var bottom = Math.Min(Y + Height, rect.Y + rect.Height);
            return new(left, top, right - left, bottom - top);
        }

        public bool Equals(DoubleRect other) => X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is DoubleRect rect && Equals(rect);

        public static bool operator ==(DoubleRect left, DoubleRect right) => left.Equals(right);
        public static bool operator !=(DoubleRect left, DoubleRect right) => !(left == right);

        public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        public override string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";
    }
}
