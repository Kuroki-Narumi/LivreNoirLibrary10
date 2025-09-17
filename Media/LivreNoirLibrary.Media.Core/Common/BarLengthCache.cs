using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public class BarLengthCache(int capacity = 999)
    {
        /// <summary>
        /// index: 小節番号 - 1, value: その小節先頭の絶対時刻
        /// </summary>
        private readonly List<Rational> _cache = new(capacity);

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
            else
            {
                // cacheのインデックスは「実際の小節番号 - 1」
                number--;
                _cache.RemoveRange(number, _cache.Count - number);
            }
        }

        public Rational GetHead(int number, IBarPositionProvider provider)
        {
            if (number is <= 0)
            {
                return Rational.Zero;
            }
            else
            {
                var cache = _cache;
                var pos = cache.Count is 0 ? Rational.Zero : cache[^1];
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

        public Rational GetAbsolutePosition(BarPosition pos, IBarPositionProvider provider)
        {
            var head = GetHead(pos.Bar, provider);
            return head + pos.Offset * provider.GetBarLength(pos.Bar);
        }

        public BarPosition GetBarPosition(Rational absolutePosition, IBarPositionProvider provider)
        {
            if (absolutePosition.IsNegativeOrZero())
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
            var total = index is 0 ? Rational.Zero : cache[number];
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
            return new(number, absolutePosition / length);
        }

        public IEnumerable<BarInfo> EnumerateBars(int first, int last, IBarPositionProvider provider)
        {
            var pos = GetHead(first, provider);
            for (var i = first; i <= last; i++)
            {
                var len = provider.GetBarLength(i);
                yield return new(i, new(len), pos, len);
                pos += len;
            }
        }
    }
}
