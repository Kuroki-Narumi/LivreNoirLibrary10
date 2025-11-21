using System;
using System.Collections.Generic;
using System.Numerics;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public abstract class BarLengthCache<T>(int capacity = 999)
        where T : INumber<T>
    {
        /// <summary>
        /// index: 小節番号 - 1, value: その小節先頭の絶対時刻
        /// </summary>
        private readonly List<T> _cache = new(capacity);

        protected abstract T Convert(Rational value);
        protected abstract Rational ConvertBack(T value);

        /// <summary>
        /// 指定された小節番号以降の時刻キャッシュを削除します。小節長が変更された場合に呼び出す必要があります。
        /// </summary>
        /// <param name="number">長さが変更された小節の番号</param>
        public void Clear(int number = 0)
        {
            if (number is <= 0)
            {
                _cache.Clear();
            }
            else if (number <= _cache.Count)
            {
                // cacheのインデックスは「実際の小節番号 - 1」
                number--;
                _cache.RemoveRange(number, _cache.Count - number);
            }
        }

        public T GetHead(int number, IBarLengthProvider<T> provider)
        {
            if (number is <= 0)
            {
                return T.Zero;
            }
            else
            {
                var cache = _cache;
                var pos = cache.Count is 0 ? T.Zero : cache[^1];
                // cache.Count = キャッシュ済みの小節番号
                for (var num = cache.Count; num < number; num++)
                {
                    pos += provider.GetBarLength(num);
                    cache.Add(pos);
                }
                // cacheのインデックスは「実際の小節番号 - 1」
                return cache[number - 1];
            }
        }

        public T GetAbsolutePosition(BarPosition pos, IBarLengthProvider<T> provider)
        {
            var (number, offset) = pos;
            return offset.IsZero() ? GetHead(number, provider) : GetHead(number, provider) + Convert(offset) * provider.GetBarLength(number);
        }

        public BarPosition GetBarPosition(T absolutePosition, IBarLengthProvider<T> provider)
        {
            if (absolutePosition <= T.Zero)
            {
                return BarPosition.Zero;
            }
            var cache = _cache;
            // 指定値に一致する要素のインデックスを検索
            var index = cache.BinarySearch(absolutePosition);
            // 一致する要素がある
            if (index is >= 0)
            {
                // インデックスは「実際の小節番号 - 1」
                return new(index + 1, 0);
            }
            // 指定値より値の大きい最初の要素のインデックス(存在しない場合はリストの長さに等しい)
            index = ~index;
            // 実際の小節番号
            var number = index - 1;
            // 指定値の直前の小節線の絶対時刻
            var total = index is 0 ? T.Zero : cache[number];
            absolutePosition -= total;
            var length = provider.GetBarLength(number);
            // 指定値の残り部分が小節長以上である限り
            while (absolutePosition >= length)
            {
                // キャッシュの更新
                total += length;
                cache.Add(total);
                // 小節番号の更新
                number++;
                length = provider.GetBarLength(number);
                absolutePosition -= length;
            }
            return new(number, ConvertBack(absolutePosition / length));
        }
    }

    public sealed class RationalBarLengthCache : BarLengthCache<Rational>
    {
        protected override Rational Convert(Rational value) => value;
        protected override Rational ConvertBack(Rational value) => value;
    }

    public sealed class DoubleBarLengthCache : BarLengthCache<double>
    {
        protected override double Convert(Rational value) => (double)value;
        protected override Rational ConvertBack(double value) => Rational.ConvertBySBT(value);
    }
}
