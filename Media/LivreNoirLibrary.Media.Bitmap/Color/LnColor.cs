using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct LnColor : IEquatable<LnColor>, IEqualityOperators<LnColor, LnColor, bool>
    {
        public readonly byte B;
        public readonly byte G;
        public readonly byte R;
        public readonly byte A;

        public LnColor(byte a, byte r, byte g, byte b) => (A, R, G, B) = (a, r, g, b);
        public LnColor(byte r, byte g, byte b) => (A, R, G, B) = (255, r, g, b);

        public LnColor(int a, int r, int g, int b) => (A, R, G, B) = ((byte)a, (byte)r, (byte)g, (byte)b);
        public LnColor(int r, int g, int b) => (A, R, G, B) = (255, (byte)r, (byte)g, (byte)b);

        public LnColor(float alpha, uint rgb)
        {
            var a = ColorUtils.GetByte(alpha);
            rgb |= (uint)a << 24;
            this = *(LnColor*)&rgb;
        }

        public override int GetHashCode()
        {
            var c = this;
            return *(int*)&c;
        }
        public override bool Equals(object? obj) => obj is LnColor c && Equals(c);
        public bool Equals(LnColor other) => this == other;
        public static bool operator ==(LnColor left, LnColor right) => *(uint*)&left == *(uint*)&right;
        public static bool operator !=(LnColor left, LnColor right) => *(uint*)&left != *(uint*)&right;

        public override string ToString() => ColorUtils.GetColorCode(A, R, G, B);

        public static implicit operator LnColor((byte, byte, byte, byte)value) => new(value.Item1, value.Item2, value.Item3, value.Item4);
        public static implicit operator LnColor((byte, byte, byte)value) => new(value.Item1, value.Item2, value.Item3);
        public static implicit operator LnColor(uint value) => *(LnColor*)&value;

        public static implicit operator uint(LnColor value) => *(uint*)&value;

        public uint RGB => (uint)this & ~ColorUtils.Mask_A;

        public void Deconstruct(out float alpha, out uint rgb)
        {
            alpha = ColorUtils.GetFloat(A);
            rgb = RGB;
        }

        public void Deconstruct(out byte a, out byte r, out byte g, out byte b)
        {
            a = A;
            r = R;
            g = G;
            b = B;
        }

        public (float A, float R, float G, float B) ToFloat() => (ColorUtils.GetFloat(A), ColorUtils.GetFloat(R), ColorUtils.GetFloat(G), ColorUtils.GetFloat(B));
        public static LnColor FromFloat(float a, float r, float g, float b) => new(ColorUtils.GetByte(a), ColorUtils.GetByte(r), ColorUtils.GetByte(g), ColorUtils.GetByte(b));
    }
}
