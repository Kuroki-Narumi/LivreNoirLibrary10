using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bms
{
    [JsonConverter(typeof(BarPositionJsonConverter))]
    [TypeConverter(typeof(BarPositionTypeConverter))]
    public readonly partial struct BarPosition :
        IEquatable<BarPosition>, IComparable<BarPosition>, IFormattable, ISpanParsable<BarPosition>, IDumpable, ILoadable<BarPosition>,
        IEqualityOperators<BarPosition, BarPosition, bool>, IComparisonOperators<BarPosition, BarPosition, bool>
    {
        internal readonly double _value;

        public static BarPosition Zero { get; } = default;
        public static BarPosition MaxValue { get; } = new(BmsConstants.MaxBarNumber + 1, true);

        public int Bar => (int)Math.Truncate(_value);
        public double Offset => _value - Math.Truncate(_value);
        public Rational RationalOffset => Offset.ToRational(BmsConstants.MaxInnerResolution);

        internal BarPosition(double value, bool _)
        {
            _value = value;
        }

        public BarPosition(double value)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, BmsConstants.MaxBarNumber + 1, nameof(value));
            _value = value;
        }

        public BarPosition(int bar, double offset)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(bar, nameof(bar));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bar, BmsConstants.MaxBarNumber, nameof(bar));
            ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(offset, 1, nameof(offset));
            _value = bar + offset;
        }

        public int CompareTo(BarPosition other) => _value.CompareTo(other._value);
        public bool Equals(BarPosition other) => _value == other._value;
        public override bool Equals(object? obj) => obj is BarPosition pos && Equals(pos);
        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(BarPosition left, BarPosition right) => left._value == right._value;
        public static bool operator !=(BarPosition left, BarPosition right) => left._value != right._value;
        public static bool operator <(BarPosition left, BarPosition right) => left._value < right._value;
        public static bool operator <=(BarPosition left, BarPosition right) => left._value <= right._value;
        public static bool operator >(BarPosition left, BarPosition right) => left._value > right._value;
        public static bool operator >=(BarPosition left, BarPosition right) => left._value >= right._value;

        public void Deconstruct(out int bar, out double offset)
        {
            var v = _value;
            var intPart = Math.Truncate(v);
            bar = (int)intPart;
            offset = v - intPart;
        }

        public string GetBarText() => $"#{Bar:D3}";
        public string GetOffsetText()
        {
            var (num, den) = Rational.RationalizeUnsafe(Offset, BmsConstants.MaxInnerResolution);
            return den is 1 ? $"{num}" : $"{num}/{den}";
        }

        public override string ToString()
        {
            var (bar, offset) = this;
            if (offset is 0)
            {
                return $"#{bar:D3}:0";
            }
            var (num, den) = Rational.RationalizeUnsafe(offset, BmsConstants.MaxInnerResolution);
            return $"#{bar:D3}:{num}/{den}";
        }

        public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

        public static explicit operator double(BarPosition pos) => pos._value;
        public static explicit operator float(BarPosition pos) => (float)pos._value;
        public static explicit operator BarPosition(int bar) => new(bar);
        public static explicit operator BarPosition(double value) => new(value);
        public static explicit operator BarPosition(float value) => new(value);

        public static BarPosition Max(BarPosition x, BarPosition y) => new(Math.Max(x._value, y._value), true);
        public static BarPosition Min(BarPosition x, BarPosition y) => new(Math.Min(x._value, y._value), true);

        public void Dump(BinaryWriter writer) => writer.Write(_value);
        public static BarPosition Load(BinaryReader reader) => new(reader.ReadDouble(), true);

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
            if (TryParseCore(s, provider, out var bar, out var offsetNum, out var offsetDen) && bar is < BmsConstants.MaxBarNumber + 1)
            {
                var offset = offsetNum / offsetDen;
                if (offset is >= 0 and < 1)
                {
                    result = new BarPosition(bar + offset, true);
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryParseCore(ReadOnlySpan<char> s, IFormatProvider? provider, out double bar, out double offsetNum, out double offsetDen)
        {
            bar = offsetNum = offsetDen = 0;
            if (s.IsEmpty)
            {
                return false;
            }
            // 小節番号とオフセットを分割するためのバッファ
            var ranges = (stackalloc Range[3]);
            // 区切り文字 ':' で分割
            var count = s.Split(ranges, ':', StringSplitOptions.TrimEntries);
            // 1st check
            // 分割数1(番号のみ)または2(番号:オフセット)のみ受け付ける
            if (count is not (1 or 2) ||
                // 小節番号(小節を表す '#' は削除)
                !double.TryParse(s[ranges[0]].TrimStart('#'), provider, out bar) ||
                // 0以上1000未満のみ受け付ける
                bar is < 0)
            {
                return false;
            }
            // 小節番号のみ
            if (count is 1)
            {
                offsetDen = 1;
                return true;
            }
            // オフセットを含む場合、小節番号の小数部分は無視する
            bar = Math.Truncate(bar);
            // オフセットの全体
            s = s[ranges[1]];
            // 区切り文字 '/' で分割
            count = s.Split(ranges, '/', StringSplitOptions.TrimEntries);
            // 2nd check
            // 分割数1(分子のみ)または2(分子/分母)のみ受け付ける
            if (count is not (1 or 2) ||
                // オフセット
                !(double.TryParse(s[ranges[0]], provider, out offsetNum) && double.IsFinite(offsetNum)))
            {
                return false;
            }
            // 分母(存在する場合)
            if (count is 2)
            {
                if (!(double.TryParse(s[ranges[1]], provider, out offsetDen) && double.IsFinite(offsetDen) && offsetDen is not 0))
                {
                    return false;
                }
            }
            else
            {
                offsetDen = 1;
            }
            return true;
        }
    }
}
