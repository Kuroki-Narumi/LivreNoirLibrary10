using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    [JsonConverter(typeof(BarPositionJsonConverter))]
    [TypeConverter(typeof(BarPositionTypeConverter))]
    public readonly partial struct BarPosition(int bar, Rational offset) : 
        IEquatable<BarPosition>, IComparable<BarPosition>, IFormattable, IParsable<BarPosition>, IDumpable<BarPosition>,
        IEqualityOperators<BarPosition, BarPosition, bool>, IComparisonOperators<BarPosition, BarPosition, bool>
    {
        public const int MaxNumber = short.MaxValue;
        public const string BarTextFormat = "#{0:D3}";

        public static readonly BarPosition Zero = new(0, Rational.Zero);
        public static readonly BarPosition Invalid = new(-1, Rational.Zero);
        public static readonly BarPosition MaxValue = new(MaxNumber, Rational.Zero);

        private readonly int _bar = bar;
        private readonly Rational _offset = offset;

        public int Bar => _bar;
        public Rational Offset => _offset;

        public BarPosition(int bar) : this(bar, Rational.Zero) { }
        public BarPosition(int bar, long beatNum, long beatDen = 1) : this(bar, new Rational(beatNum, beatDen)) { }

        public static implicit operator BarPosition(Rational value) => new(0, value);

        public int CompareTo(BarPosition other) => _bar == other._bar ? _offset.CompareTo(other._offset) : _bar.CompareTo(other._bar);
        public bool Equals(BarPosition other) => this == other;
        public override bool Equals(object? obj) => obj is BarPosition pos && Equals(pos);
        public override int GetHashCode() => HashCode.Combine(_bar, _offset);

        public static bool operator ==(BarPosition left, BarPosition right) => left._bar == right._bar && left._offset == right._offset;
        public static bool operator !=(BarPosition left, BarPosition right) => left._bar != right._bar || left._offset != right._offset;
        public static bool operator <(BarPosition left, BarPosition right) => left._bar < right._bar || left._bar == right._bar && left._offset < right._offset;
        public static bool operator <=(BarPosition left, BarPosition right) => left._bar < right._bar || left._bar == right._bar && left._offset <= right._offset;
        public static bool operator >(BarPosition left, BarPosition right) => left._bar > right._bar || left._bar == right._bar && left._offset > right._offset;
        public static bool operator >=(BarPosition left, BarPosition right) => left._bar > right._bar || left._bar == right._bar && left._offset >= right._offset;

        public static BarPosition Max(BarPosition x, BarPosition y) => x < y ? y : x;
        public static BarPosition Min(BarPosition x, BarPosition y) => x > y ? y : x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetBarText(int number) => string.Format(BarTextFormat, number);
        public override string ToString() => $"{GetBarText(_bar)}:{_offset}";
        public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

        public void Dump(BinaryWriter writer)
        {
            writer.Write((short)_bar);
            writer.Write(_offset);
        }

        public static BarPosition Load(BinaryReader reader)
        {
            var bar = reader.ReadInt16();
            var beat = reader.ReadRational();
            return new(bar, beat);
        }

        public void Deconstruct(out int bar, out Rational beat)
        {
            bar = _bar;
            beat = _offset;
        }

        public void Deconstruct(out int bar, out long beatNumerator, out long beatDenominator)
        {
            bar = _bar;
            beatNumerator = _offset.Numerator;
            beatDenominator = _offset.Denominator;
        }

        private static void ThrowFormatException(string message = "") => throw new FormatException(message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BarPosition Parse(string s) => Parse(s, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BarPosition Parse(string s, IFormatProvider? provider)
        {
            if (!TryParse(s, provider, out var result))
            {
                ThrowFormatException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse([NotNullWhen(true)] string? s, out BarPosition result) => TryParse(s, null, out result);
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out BarPosition result)
        {
            result = default;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            var match = BarPositionRegex.Match(s);
            if (match.Success && int.TryParse(match.Groups["bar"].Value, provider, out var bar))
            {
                var bt = match.Groups["beat"];
                if (bt.Success)
                {
                    if (Rational.TryParse(bt.Value, out var beat))
                    {
                        result = new(bar, beat);
                        return true;
                    }
                }
                else
                {
                    result = new(bar);
                    return true;
                }
            }
            return false;
        }

        [GeneratedRegex(@"^\s*?(?:#\s*?(?<bar>\d+)\s*?)(?::(?<beat>.+))?$")]
        public static partial Regex BarPositionRegex { get; }
    }
}
