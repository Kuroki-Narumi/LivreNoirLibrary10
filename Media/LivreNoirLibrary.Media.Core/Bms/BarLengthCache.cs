using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class BarLengthCache
    {
        /// <summary>
        /// index: 小節番号 - 1, value: その小節先頭の絶対時刻
        /// </summary>
        private readonly List<double> _cache = new(BmsConstants.MaxBarNumber);

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

        public double GetHead(int number, IBarLengthProvider provider)
        {
            if (number is <= 0)
            {
                return 0;
            }
            else
            {
                var cache = _cache;
                var pos = cache.Count is 0 ? 0 : cache[^1];
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

        public double GetAbsolutePosition(BarPosition pos, IBarLengthProvider provider)
        {
            var (number, offset) = pos;
            return GetHead(number, provider) + offset * provider.GetBarLength(number);
        }

        public BarPosition GetBarPosition(double absolutePosition, IBarLengthProvider provider)
        {
            if (absolutePosition is <= 0)
            {
                return default;
            }
            var cache = _cache;
            // 指定値に一致する要素のインデックスを検索
            var index = cache.BinarySearch(absolutePosition);
            // 一致する要素がある
            if (index is >= 0)
            {
                // インデックスは「実際の小節番号 - 1」
                return new(index + 1, true);
            }
            // 指定値より値の大きい最初の要素のインデックス(存在しない場合はリストの長さに等しい)
            // = 指定値の直後の小節番号 - 1 = 指定値の直前の小節番号
            var number = ~index;
            // 指定値の直前の小節線の絶対時刻
            var total = number is 0 ? 0 : cache[number - 1];
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
            return new(number + absolutePosition / length, true);
        }
    }
}
