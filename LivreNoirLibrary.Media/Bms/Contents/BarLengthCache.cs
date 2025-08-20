using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class BarLengthCache
    {
        /// <summary>
        /// index: 小節番号, value: その小節頭の累計beats
        /// </summary>
        private readonly Rational[] _pos_cache = new Rational[Constants.MaxBarNumber + 1];
        /// <summary>
        /// キャッシュ済みの小節番号
        /// </summary>
        private int _cached_number = 0;

        public void Clear(int number = 0)
        {
            if (number is >= 0 && _cached_number > number)
            {
                _cached_number = number;
            }
        }

        private void EnsureBarPosCache(int number, BarLengthCollection bars)
        {
            var num = _cached_number;
            var cache = _pos_cache;
            var pos = cache[num];
            for (; num < number; num++)
            {
                pos += bars.Get(num);
                cache[num + 1] = pos;
            }
            _cached_number = num;
        }

        public Rational GetHead(int number, BarLengthCollection bars)
        {
            if ((uint)number is <= Constants.MaxBarNumber)
            {
                EnsureBarPosCache(number, bars);
                return _pos_cache[number];
            }
            return Rational.Zero;
        }

        public Rational GetAbsolutePosition(BarPosition pos, BarLengthCollection bars)
        {
            var head = GetHead(pos.Bar, bars);
            return head + pos.Offset * bars.Get(pos.Bar);
        }

        public BarPosition GetBarPosition(Rational absolutePosition, BarLengthCollection bars)
        {
            if (absolutePosition.IsNegativeOrZero())
            {
                return BarPosition.Zero;
            }
            var cache = _pos_cache;
            var cachedNum = _cached_number;
            // 指定値に一致する要素のインデックスを検索
            var number = Array.BinarySearch(cache, 0, cachedNum + 1, absolutePosition);
            // 一致する要素がある
            if (number is >= 0)
            {
                return new(number, 0);
            }
            // ~number - 1 = 指定値より小さい最大の要素のインデックス
            number = ~number - 1;
            var total = cache[number];
            absolutePosition -= total;
            for (; ; number++)
            {
                var length = bars.Get(number);
                if (absolutePosition < length)
                {
                    _cached_number = cachedNum;
                    return new(number, absolutePosition / length);
                }
                if (number is < Constants.MaxBarNumber)
                {
                    total += length;
                    cache[number + 1] = total;
                    cachedNum++;
                }
                absolutePosition -= length;
            }
        }

        public IEnumerable<BarInfo> EnumBars(int first, int last, BarLengthCollection bars)
        {
            var pos = GetHead(first, bars);
            for (int i = first; i <= last; i++)
            {
                var len = bars.Get(i);
                yield return new(i, new(len), pos, len);
                pos += len;
            }
        }
    }
}
