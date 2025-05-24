using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms.RawData
{
    public partial class ChannelData : IComparable, IComparable<ChannelData>
    {
        public static uint LengthLimit { get; set; } = 4233600; // 2^7 * 3^3 * 5^2 * 7^2

        private ushort[] _data;

        public Channel Channel { get; }
        public ReadOnlySpan<ushort> Data => _data;
        public int Length => _data.Length;

        public ushort this[int pos] { get => _data[pos]; set => _data[pos] = value; }

        private ChannelData(Channel channel, ushort[] data)
        {
            Channel = channel;
            _data = data;
        }

        public ChannelData(Channel channel) : this(channel, new ushort[1]) { }
        private ChannelData(Channel channel, int length) : this(channel, new ushort[length]) { }

        public ChannelData(Channel channel, string data, int radix)
        {
            Channel = channel;
            if (BmsUtils.IsHex(channel))
            {
                radix = 16;
            }
            _data = GetData(data, radix);
        }

        public static ChannelData Empty(Channel channel) => new(channel);
        public static ChannelData Create(Channel channel, string data, int radix)
        {
            if (BmsUtils.IsHex(channel))
            {
                radix = 16;
            }
            return new(channel, GetData(data, radix));
        }

        public ChannelData Clone()
        {
            ChannelData data = new(Channel, Length);
            Data.CopyTo(data._data);
            return data;
        }

        public bool IsEmpty()
        {
            return Length is 0 || (Length is 1 && _data[0] is 0);
        }

        private static bool CorrectPosition(ref Rational position)
        {
            var (num, den) = position;
            if (num >= den)
            {
                return true;
            }
            if (den >= LengthLimit)
            {
                var p = position.LimitDen(LengthLimit);
                ExConsole.Write($"WARING: The time resolution is too high ({position.Denominator}), so it is forced to be reduced (to {p.Denominator}).");
                position = p;
            }
            return false;
        }

        public static ChannelData Create(Channel channel, Rational position, int value)
        {
            if (CorrectPosition(ref position))
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }
            var data = new ushort[position.Denominator];
            data[position.Numerator] = (ushort)value;
            return new ChannelData(channel, data);
        }

        public ushort GetAt(in Rational pos)
        {
            var (num, den) = pos;
            if (num is 0)
            {
                return _data[0];
            }
            var len = _data.LongLength;
            return len % den is 0 ? _data[len / den * num] : (ushort)0;
        }

        public void SetAt(in Rational pos, ushort value)
        {
            var (num, den) = pos;
            if (num >= den)
            {
                return;
            }
            var len = _data.LongLength;
            if (len == den)
            {
                _data[num] = value;
            }
            else if (len % den == 0)
            {
                _data[len / den * num] = value;
            }
            else
            {
                var gcd = den.GCD(len);
                var lcm = len / gcd * (den / gcd) * gcd;
                if (len != lcm)
                {
                    _data = Adjust(_data, lcm);
                }
                _data[num * len / gcd] = value;
            }
        }

        private static ushort[] GetData(string data, int radix)
        {
            var limit = data.Length / 2;
            if (limit is 0)
            {
                return [0];
            }
            var result = new ushort[limit];
            var span = data.AsSpan();
            var i = 0;
            foreach (var range in Regex_Channel.EnumerateMatches(span))
            {
                result[i] = (ushort)BasedIndex.ParseToLong(span.Slice(range.Index, range.Length), radix);
                i++;
            }
            return result;
        }

        public string GetDataString(int radix, int digits = 2)
        {
            if (BmsUtils.IsHex(Channel))
            {
                radix = 16;
            }
            StringBuilder sb = new();
            var ary = Compact(_data);
            for (int i = 0; i < ary.Length; i++)
            {
                sb.Append(BasedIndex.ToBased(ary[i], radix, digits));
            }
            return sb.ToString();
        }

        public bool Exists(int index)
        {
            return index >= 0 && index < Data.Length && Data[index] != 0;
        }

        public bool CanMerge(ChannelData other) => CanMerge(other._data.Length);
        public bool CanMerge(int otherLength) => otherLength is 0 || _data.Length.LCM(otherLength) <= LengthLimit;

        public void Merge(ChannelData src) => Merge(src._data);

        public void Merge(ReadOnlySpan<ushort> src)
        {
            if (src.Length is 0)
            {
                return;
            }
            if (Length is 0)
            {
                _data = new ushort[src.Length];
                src.CopyTo(_data);
                return;
            }
            var dst = _data;
            var length = dst.Length.LCM(src.Length);
            var newAry = new ushort[length];
            var interval1 = length / dst.Length;
            var interval2 = length / src.Length;
            for (int i = 0; i < length; i++)
            {
                if (i % interval1 == 0)
                {
                    newAry[i] = dst[i / interval1];
                }
                if (i % interval2 == 0)
                {
                    var j = i / interval2;
                    if (src[j] != 0)
                    {
                        newAry[i] = src[j];
                    }
                }
            }
            _data = newAry;
        }

        public void Compact() => _data = Compact(_data);

        public void AdjustTo(ChannelData target)
        {
            var ary1 = Compact(_data);
            var ary2 = Compact(target._data);
            var length = ary1.Length.LCM(ary2.Length);
            _data = Adjust(ary1, length);
            target._data = Adjust(ary2, length);
        }

        public void AdjustTo(long length)
        {
            _data = Adjust(Compact(_data), length);
        }

        public void AdjustWithoutCompact(long length)
        {
            _data = Adjust(_data, length);
        }

        public static ushort[] Compact(ushort[] ary)
        {
            var length = ary.Length;
            if (length <= 1) { return ary; }
            var divisors = length.Divisors();
            var iLimit = divisors.Length;
            for (int i = 1; i < iLimit; i++)
            {
                var flag = true;
                var d = divisors[i];
                var newAry = new ushort[length / d];
                for (int j = 0; j < length; j++)
                {
                    if (j % d == 0)
                    {
                        newAry[j / d] = ary[j];
                    }
                    else if (ary[j] != 0)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return Compact(newAry);
                }
            }
            return ary;
        }

        public static ushort[] Adjust(ushort[] ary, long length)
        {
            var curLen = ary.LongLength;
            if (curLen == length) { return ary; }
            var result = new ushort[length];
            if (curLen <= 0) { return result; }
            if (length % curLen == 0)
            {
                var interval = length / curLen;
                for (int i = 0; i < curLen; i++)
                {
                    result[i * interval] = ary[i];
                }
            }
            else
            {
                var last_index = -1L;
                for (int i = 0; i < length; i++)
                {
                    var index = i * curLen / length;
                    if (index != last_index)
                    {
                        result[i] = ary[index];
                        last_index = index;
                    }
                }
            }
            return result;
        }

        public int CompareTo(ChannelData? obj) => obj is not null ? Channel.CompareTo(obj.Channel) : 1;
        public int CompareTo(object? obj) => Channel.CompareTo(obj as ChannelData);

        [GeneratedRegex("[0-9a-zA-Z]{2}")]
        private static partial Regex Regex_Channel { get; }
    }
}
