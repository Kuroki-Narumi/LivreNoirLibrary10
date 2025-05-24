using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    internal delegate Rational LengthGetter(int number);

    partial class BmsData
    {
        /// <summary>
        /// index: 小節番号, value: その小節頭の累計beats
        /// </summary>
        private readonly Rational[] _pos_cache = new Rational[Constants.MaxBarNumber + 1];
        /// <summary>
        /// キャッシュ済みの小節番号
        /// </summary>
        private int _cached_number = 0;

        public void ClearBarCache()
        {
            _cached_number = 0;
        }

        public void ClearBarCache(int number)
        {
            if (number is >= 0 && _cached_number > number)
            {
                _cached_number = number;
            }
        }

        private void EnsureBarPosCache(int number, LengthGetter getter)
        {
            var pos = _pos_cache[_cached_number];
            for (; _cached_number < number; _cached_number++)
            {
                pos += getter(_cached_number);
                _pos_cache[_cached_number + 1] = pos;
            }
        }

        internal Rational GetHead(int number, LengthGetter getter)
        {
            if ((uint)number is <= Constants.MaxBarNumber)
            {
                EnsureBarPosCache(number, getter);
                return _pos_cache[number];
            }
            return Rational.Zero;
        }

        internal BarPosition GetPosition(Rational beat, LengthGetter getter)
        {
            if (beat.IsNegative())
            {
                return BarPosition.Zero;
            }
            // 指定値に一致する要素のインデックスを検索
            var number = Array.BinarySearch(_pos_cache, 0, _cached_number + 1, beat);
            // 一致する要素がある
            if (number is >= 0)
            {
                return new(number, 0);
            }
            // ~number - 1 = 指定値より小さい最大の要素のインデックス
            number = ~number - 1;
            var total = _pos_cache[number];
            beat -= total;
            for (; ; number++)
            {
                var length = getter(number);
                if (beat < length)
                {
                    return new(number, beat);
                }
                if (number is < Constants.MaxBarNumber)
                {
                    total += length;
                    _pos_cache[number + 1] = total;
                    _cached_number++;
                }
                beat -= length;
            }
        }

        internal IEnumerable<BarInfo> EnumBars(int first, int last, LengthGetter getter)
        {
            var pos = GetHead(first, getter);
            for (int i = first; i <= last; i++)
            {
                var len = getter(i);
                yield return new(i, new(len), pos, len);
                pos += len;
            }
        }
    }
}
