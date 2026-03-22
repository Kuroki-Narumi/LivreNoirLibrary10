using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Numerics;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Midi
{
    [JsonConverter(typeof(BarPositionJsonConverter))]
    [TypeConverter(typeof(BarPositionTypeConverter))]
    public readonly partial struct BarPosition : 
        IEquatable<BarPosition>, IComparable<BarPosition>, IFormattable, IParsable<BarPosition>,
        IEqualityOperators<BarPosition, BarPosition, bool>, IComparisonOperators<BarPosition, BarPosition, bool>
    {
        public const int MaxNumber = short.MaxValue;

        public static BarPosition Zero { get; } = new(0, Rational.Zero);
        public static BarPosition Invalid { get; } = new(-1, Rational.Zero);
        public static BarPosition MaxValue { get; } = new(MaxNumber, Rational.Zero);

        public int Bar { get; }
        public Rational Offset { get; }

        public BarPosition(int bar, Rational offset)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(bar, nameof(bar));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bar, MaxNumber, nameof(bar));
            ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
            Bar = bar;
            Offset = offset;
        }

        public BarPosition(int bar) : this(bar, Rational.Zero) { }
        public BarPosition(int bar, long offsetNum, long offsetDen) : this(bar, new Rational(offsetNum, offsetDen)) { }

        public int CompareTo(BarPosition other) => Bar == other.Bar ? Offset.CompareTo(other.Offset) : Bar.CompareTo(other.Bar);
        public bool Equals(BarPosition other) => this == other;
        public override bool Equals(object? obj) => obj is BarPosition pos && Equals(pos);
        public override int GetHashCode() => HashCode.Combine(Bar, Offset);

        public static bool operator ==(BarPosition left, BarPosition right) => left.Bar == right.Bar && left.Offset == right.Offset;
        public static bool operator !=(BarPosition left, BarPosition right) => left.Bar != right.Bar || left.Offset != right.Offset;
        public static bool operator <(BarPosition left, BarPosition right) => left.Bar < right.Bar || left.Bar == right.Bar && left.Offset < right.Offset;
        public static bool operator <=(BarPosition left, BarPosition right) => left.Bar < right.Bar || left.Bar == right.Bar && left.Offset <= right.Offset;
        public static bool operator >(BarPosition left, BarPosition right) => left.Bar > right.Bar || left.Bar == right.Bar && left.Offset > right.Offset;
        public static bool operator >=(BarPosition left, BarPosition right) => left.Bar > right.Bar || left.Bar == right.Bar && left.Offset >= right.Offset;

        public void Deconstruct(out int bar, out Rational offset)
        {
            bar = Bar;
            offset = Offset;
        }

        public void Deconstruct(out int bar, out long offsetNumerator, out long offsetDenominator)
        {
            bar = Bar;
            offsetNumerator = Offset.Numerator;
            offsetDenominator = Offset.Denominator;
        }

        public string GetBarText() => $"#{Bar:D3}";
        public string GetOffsetText() => Offset.ToString();

        public override string ToString() => $"{GetBarText()}:{GetOffsetText()}";
        public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BarPosition Parse(string s) => Parse(s.AsSpan(), null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BarPosition Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BarPosition Parse(ReadOnlySpan<char> s) => Parse(s, null);

        public static BarPosition Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (!TryParse(s, provider, out var result))
            {
                throw new FormatException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse([NotNullWhen(true)] string? s, out BarPosition result) => TryParse(s.AsSpan(), null, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out BarPosition result) => TryParse(s.AsSpan(), provider, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ReadOnlySpan<char> s, out BarPosition result) => TryParse(s, null, out result);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BarPosition result)
        {
            if (Bms.BarPosition.TryParseCore(s, provider, out var bar, out var offsetNum, out var offsetDen) && bar is <= MaxNumber)
            {
                var offset = offsetNum / offsetDen;
                if (offset is >= 0)
                {
                    result = new((int)bar, offset.ToRational());
                    return true;
                }
            }
            result = default;
            return false;
        }
    }
}
