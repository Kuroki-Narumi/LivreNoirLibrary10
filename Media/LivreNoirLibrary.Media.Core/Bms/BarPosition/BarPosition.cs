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
        public static BarPosition Zero { get; } = default;
        public static BarPosition MaxValue { get; } = new(BmsConstants.MaxBarNumber + 1);

        public readonly int Bar;
        public readonly double Offset;

        public Rational RationalOffset => Offset.ToRational(BmsConstants.MaxInnerResolution);

        public BarPosition(int bar, double offset = 0)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(bar, nameof(bar));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bar, BmsConstants.MaxBarNumber, nameof(bar));
            ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(offset, 1, nameof(offset));
            Bar = bar;
            Offset = offset;
        }

        public BarPosition(double offset) : this((int)offset, offset % 1) { }

        public int CompareTo(BarPosition other) => (Bar + Offset).CompareTo(other.Bar + Offset);

        public bool Equals(BarPosition other) => Bar == other.Bar && Offset == other.Offset;
        public override bool Equals(object? obj) => obj is BarPosition pos && Equals(pos);
        public override int GetHashCode() => (Bar + Offset).GetHashCode();

        public static bool operator ==(BarPosition left, BarPosition right) => left.Equals(right);
        public static bool operator !=(BarPosition left, BarPosition right) => !left.Equals(right);
        public static bool operator <(BarPosition left, BarPosition right) => (left.Bar + left.Offset) < (right.Bar + right.Offset);
        public static bool operator <=(BarPosition left, BarPosition right) => (left.Bar + left.Offset) <= (right.Bar + right.Offset);
        public static bool operator >(BarPosition left, BarPosition right) => (left.Bar + left.Offset) > (right.Bar + right.Offset);
        public static bool operator >=(BarPosition left, BarPosition right) => (left.Bar + left.Offset) >= (right.Bar + right.Offset);

        public void Deconstruct(out int bar, out double offset)
        {
            bar = Bar;
            offset = Offset;
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

        public static BarPosition Max(BarPosition x, BarPosition y) => x < y ? y : x;
        public static BarPosition Min(BarPosition x, BarPosition y) => x < y ? x : y;

        public void Dump(BinaryWriter writer)
        {
            writer.Write((ushort)Bar);
            writer.Write(Offset);
        }

        public static BarPosition Load(BinaryReader reader)
        {
            var bar = reader.ReadUInt16();
            var offset = reader.ReadDouble();
            return new(bar, offset);
        }

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
            if (TryParseCore(s, provider, out var bar, out var offset) && bar is < BmsConstants.MaxBarNumber + 1)
            {
                result = new BarPosition(bar, offset);
                return true;
            }
            result = default;
            return false;
        }

        public static bool TryParseCore(ReadOnlySpan<char> s, IFormatProvider? provider, out int bar, out double offset)
        {
            bar = 0;
            offset = 0;
            if (s.IsEmpty)
            {
                return false;
            }
            // 小節番号とオフセットを分割するためのバッファ
            var ranges = (stackalloc Range[3]);
            // 区切り文字 ':' で分割
            var count = s.Split(ranges, ':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            // 小節番号部分(小節記号は取り除く)
            var barExpr = s[ranges[0]].TrimStart('#');
            // 分割無し: [番号].[オフセット]
            if (count is 1)
            {
                // 精度確保のためdoubleではなくdecimalとしてパース
                if (decimal.TryParse(barExpr, provider, out var decValue) && decValue is >= 0)
                {
                    bar = (int)decValue;
                    offset = (double)(decValue - bar);
                    return bar is >= 0;
                }
                else
                {
                    return false;
                }
            }
            // 分割数が2でない || 小節番号を整数として解釈できない || 負の小節番号
            if (count is not 2 || !int.TryParse(barExpr, provider, out bar) || bar is < 0)
            {
                return false;
            }
            // オフセット
            s = s[ranges[1]];
            // 区切り文字 '/' で分割
            count = s.Split(ranges, '/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            // 分割数1(分子のみ)または2(分子/分母)のみ受け付ける
            if (count is not (1 or 2) || !(double.TryParse(s[ranges[0]], provider, out offset) && double.IsFinite(offset)))
            {
                return false;
            }
            // 分母(存在する場合)
            if (count is 2)
            {
                if (!(double.TryParse(s[ranges[1]], provider, out var den) && double.IsFinite(den) && den is not 0))
                {
                    return false;
                }
                offset /= den;
            }
            return true;
        }
    }
}
