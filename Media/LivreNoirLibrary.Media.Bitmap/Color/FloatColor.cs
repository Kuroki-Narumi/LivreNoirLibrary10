using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media
{
    [StructLayout(LayoutKind.Explicit)]
    [JsonConverter(typeof(FloatColorJsonConverter))]
    [TypeConverter(typeof(FloatColorTypeConverter))]
    public readonly struct FloatColor : IEquatable<FloatColor>, IEqualityOperators<FloatColor, FloatColor, bool>, ISpanParsable<FloatColor>
    {
        public static FloatColor White { get; } = new(1, 1, 1, 1);
        public static FloatColor Black { get; } = new(1, 0, 0, 0);

        [FieldOffset(0)]
        public readonly Vector<float> Vector;
        [FieldOffset(0)]
        public readonly float B;
        [FieldOffset(1)]
        public readonly float G;
        [FieldOffset(2)]
        public readonly float R;
        [FieldOffset(3)]
        public readonly float A;

        private FloatColor(Vector<float> vector)
        {
            Vector = vector;
        }

        public FloatColor(float a, float r, float g, float b) : this(VectorUtils.CreateRepeating([b, g, r, a])) { }

        public override int GetHashCode() => Vector.GetHashCode();
        public override bool Equals(object? obj) => obj is FloatColor c && Equals(c);
        public bool Equals(FloatColor other) => Vector == other.Vector;
        public static bool operator ==(FloatColor left, FloatColor right) => left.Vector == right.Vector;
        public static bool operator !=(FloatColor left, FloatColor right) => left.Vector != right.Vector;
        public override string ToString() => ColorUtils.GetColorCode(A, R, G, B);

        public static explicit operator FloatColor(Vector<float> value) => new(value);

        public static FloatColor FromRgb(float r, float g, float b) => new(1, r, g, b);
        public static FloatColor FromByte(byte a, byte r, byte g, byte b) => new(ColorUtils.GetFloat(a), ColorUtils.RgbToScRgb(r), ColorUtils.RgbToScRgb(g), ColorUtils.RgbToScRgb(b));
        public static FloatColor FromByte(byte r, byte g, byte b) => new(1, ColorUtils.RgbToScRgb(r), ColorUtils.RgbToScRgb(g), ColorUtils.RgbToScRgb(b));

        public LnColor ToByteColor() => new(ColorUtils.GetByte(A), ColorUtils.ScRgbToRgb(R), ColorUtils.ScRgbToRgb(G), ColorUtils.ScRgbToRgb(B));
        public (byte a, byte r, byte g, byte b) ToByte() => (ColorUtils.GetByte(A), ColorUtils.ScRgbToRgb(R), ColorUtils.ScRgbToRgb(G), ColorUtils.ScRgbToRgb(B));

        public void Deconstruct(out float a, out float r, out float g, out float b)
        {
            a = A;
            r = R;
            g = G;
            b = B;
        }

        public static FloatColor operator +(FloatColor left, FloatColor right) => new(left.Vector + right.Vector);
        public static FloatColor operator -(FloatColor left, FloatColor right) => new(left.Vector - right.Vector);
        public static FloatColor operator *(FloatColor left, FloatColor right) => new(left.Vector * right.Vector);
        public static FloatColor operator /(FloatColor left, FloatColor right) => new(left.Vector / right.Vector);

        public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out FloatColor result) => TryParse(s.AsSpan(), null, out result);
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out FloatColor result) => TryParse(s.AsSpan(), provider, out result);
        public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out FloatColor result) => TryParse(s, null, out result);
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out FloatColor result)
        {
            if (ColorUtils.TryParseColorCode(s, out var a, out var r, out var g, out var b))
            {
                result = new(a, r, g, b);
                return true;
            }
            result = default;
            return false;
        }

        public static FloatColor Parse(string s) => Parse(s.AsSpan(), null);
        public static FloatColor Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);
        public static FloatColor Parse(ReadOnlySpan<char> s) => Parse(s, null);
        public static FloatColor Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (TryParse(s, provider, out var result))
            {
                return result;
            }
            throw new FormatException($"The string '{s}' was not recognized as a valid LnColor.");
        }

        private static readonly Dictionary<ColorFlags, FloatColor> _masks = new()
        {
            [ColorFlags.A] = new(1, 0, 0, 0),
            [ColorFlags.R] = new(0, 1, 0, 0),
            [ColorFlags.G] = new(0, 0, 1, 0),
            [ColorFlags.B] = new(0, 0, 0, 1),
        };

        public static FloatColor GetMask(ColorFlags flags)
        {
            if (!_masks.TryGetValue(flags, out var value))
            {
                var vector = System.Numerics.Vector.Create(0f);
                void Update(ColorFlags reference)
                {
                    if ((flags & reference) is not 0)
                    {
                        vector += _masks[reference].Vector;
                    }
                }
                Update(ColorFlags.A);
                Update(ColorFlags.R);
                Update(ColorFlags.G);
                Update(ColorFlags.B);
                value = new(vector);
                _masks[flags] = value;
            }
            return value;
        }

        static readonly Vector128<int> _shuffle128 = Vector128.Create(3, 3, 3, 3);
        static readonly Vector256<int> _shuffle256 = Vector256.Create(3, 3, 3, 3, 7, 7, 7, 7);
        static readonly Vector512<int> _shuffle512 = Vector512.Create(3, 3, 3, 3, 7, 7, 7, 7, 11, 11, 11, 11, 15, 15, 15, 15);

        public static Vector<float> FillAlphaToAll(Vector<float> vector)
        {
            var count = Vector<float>.Count;
            if (count == Vector128<float>.Count)
            {
                return Vector128.ShuffleNative(vector.AsVector128(), _shuffle128).AsVector();
            }
            else if (count == Vector256<float>.Count)
            {
                return Vector256.ShuffleNative(vector.AsVector256(), _shuffle256).AsVector();
            }
            else if (count == Vector512<float>.Count)
            {
                return Vector512.ShuffleNative(vector.AsVector512(), _shuffle512).AsVector();
            }
            return vector;
        }

        public static Vector128<float> FillAlphaToAll(Vector128<float> vector) => Vector128.ShuffleNative(vector, _shuffle128);
        public static Vector256<float> FillAlphaToAll(Vector256<float> vector) => Vector256.ShuffleNative(vector, _shuffle256);
        public static Vector512<float> FillAlphaToAll(Vector512<float> vector) => Vector512.ShuffleNative(vector, _shuffle512);
    }
}
