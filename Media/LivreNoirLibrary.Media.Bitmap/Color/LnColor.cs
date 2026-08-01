using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media
{
    [StructLayout(LayoutKind.Explicit)]
    [JsonConverter(typeof(LnColorJsonConverter))]
    [TypeConverter(typeof(LnColorTypeConverter))]
    public readonly struct LnColor : IEquatable<LnColor>, IEqualityOperators<LnColor, LnColor, bool>, ISpanParsable<LnColor>
    {
        [FieldOffset(0)]
        public readonly byte B;
        [FieldOffset(1)]
        public readonly byte G;
        [FieldOffset(2)]
        public readonly byte R;
        [FieldOffset(3)]
        public readonly byte A;

        [FieldOffset(0)]
        public readonly uint UintValue;
        [FieldOffset(0)]
        public readonly int IntValue;

        public LnColor(byte a, byte r, byte g, byte b)
        {
            B = b;
            G = g;
            R = r;
            A = a;
        }

        public LnColor(uint value)
        {
            UintValue = value;
        }

        public override int GetHashCode() => IntValue;

        public override bool Equals(object? obj) => obj is LnColor c && Equals(c);
        public bool Equals(LnColor other) => UintValue == other.UintValue;
        public static bool operator ==(LnColor left, LnColor right) => left.UintValue == right.UintValue;
        public static bool operator !=(LnColor left, LnColor right) => left.UintValue != right.UintValue;

        public override string ToString() => ColorUtils.GetColorCode(A, R, G, B);

        public static LnColor FromRgb(byte r, byte g, byte b) => new(255, r, g, b);
        public static LnColor FromFloat(float a, float r, float g, float b) => new(ColorUtils.GetByte(a), ColorUtils.GetByte(r), ColorUtils.GetByte(g), ColorUtils.GetByte(b));
        public static LnColor FromFloat(float r, float g, float b) => new(255, ColorUtils.GetByte(r), ColorUtils.GetByte(g), ColorUtils.GetByte(b));

        public FloatColor ToFloatColor() => FloatColor.FromByte(A, R, G, B);
        public (float A, float R, float G, float B) ToFloat() => (ColorUtils.GetFloat(A), ColorUtils.GetFloat(R), ColorUtils.GetFloat(G), ColorUtils.GetFloat(B));

        public void Deconstruct(out byte a, out byte r, out byte g, out byte b)
        {
            a = A;
            r = R;
            g = G;
            b = B;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out LnColor result) => TryParse(s.AsSpan(), null, out result);
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out LnColor result) => TryParse(s.AsSpan(), provider, out result);
        public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out LnColor result) => TryParse(s, null, out result);
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out LnColor result)
        {
            if (ColorUtils.TryParseColorCodeToByte(s, out var a, out var r, out var g, out var b))
            {
                result = new(a, r, g, b);
                return true;
            }
            result = default;
            return false;
        }

        public static LnColor Parse(string s) => Parse(s.AsSpan(), null);
        public static LnColor Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);
        public static LnColor Parse(ReadOnlySpan<char> s) => Parse(s, null);
        public static LnColor Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (TryParse(s, provider, out var result))
            {
                return result;
            }
            throw new FormatException($"The string '{s}' was not recognized as a valid LnColor.");
        }
    }
}
