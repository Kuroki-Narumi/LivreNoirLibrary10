using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public class BarLengthCollection : IBarLengthCollection
    {
        private readonly List<short> _numbers = [];
        private readonly List<double> _values = [];

        public int Count => _numbers.Count;

        public void Clear()
        {
            _numbers.Clear();
            _values.Clear();
        }

        private static short ValidateNumber(int number, int limit = BmsConstants.MaxBarNumber)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(number, nameof(number));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(number, limit, nameof(number));
            return (short)number;
        }

        public bool TryGetValue(int number, out double value)
        {
            if (number is >= 0 and <= BmsConstants.MaxBarNumber)
            {
                return SortedList.TryGetValue(_numbers, _values, (short)number, out _, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool Set(int number, double value)
        {
            if (value is 0)
            {
                return Remove(number);
            }
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            return SortedList.AddOrReplace(_numbers, _values, ValidateNumber(number), value);
        }

        public bool Remove(int number) => SortedList.Remove(_numbers, _values, ValidateNumber(number));

        protected int GetInsertIndex(short number)
        {
            var index = _numbers.BinarySearch(number);
            return index is < 0 ? ~index : index;
        }

        public void Insert(int number, int count)
        {
            var sCount = (short)count;
            var index = GetInsertIndex(ValidateNumber(number));
            var numbers = _numbers;
            var c = numbers.Count;
            var span = numbers.AsSpan();
            for (; index < c; index++)
            {
                span[index] += sCount;
                if (span[index] is > BmsConstants.MaxBarNumber)
                {
                    break;
                }
            }
            // 小節番号が上限を超える領域は削除する
            if (index < c)
            {
                c -= index;
                numbers.RemoveRange(index, c);
                _values.RemoveRange(index, c);
            }
        }

        public void Delete(int number, int count)
        {
            var numbers = _numbers;
            var c = numbers.Count;
            // 削除開始インデックス
            var startIndex = GetInsertIndex(ValidateNumber(number));
            var sCount = ValidateNumber(count, BmsConstants.MaxBarNumber - number);
            var endNumber = number + count;
            // 削除終了インデックス(このインデックスを含まない)
            var endIndex = startIndex;
            var span = numbers.AsSpan();
            for (; endIndex < c && span[endIndex] < endNumber; endIndex++) ;
            var removeCount = endIndex - startIndex;
            numbers.RemoveRange(startIndex, removeCount);
            _values.RemoveRange(startIndex, removeCount);
            // 残り部分の小節番号を減らす
            c = numbers.Count;
            span = numbers.AsSpan();
            for (; startIndex < c; startIndex++)
            {
                span[startIndex] -= sCount;
            }
        }

        public SortedList.Enumerator<short, double> GetEnumerator() => SortedList.GetEnumerator(_numbers, _values);
        IEnumerator<(short, double)> IEnumerable<(short, double)>.GetEnumerator() => SortedList.GetSafeEnumerator(_numbers, _values);

        public void Merge(IBarLengthCollection source)
        {
            if (source is BarLengthCollection b)
            {
                SortedList.CopyTo(b._numbers, b._values, _numbers, _values);
            }
            else
            {
                foreach (var (number, value) in source)
                {
                    Set(number, value);
                }
            }
        }

        public void Dump(BinaryWriter writer)
        {
            var count = Count;
            writer.Write((ushort)count);
            foreach (var (number, value) in SortedList.GetEnumerator(_numbers, _values))
            {
                writer.Write(number);
                writer.Write(value);
            }
        }

        public void ProcessLoad(BinaryReader reader)
        {
            Clear();
            var count = (int)reader.ReadUInt16();
            var numbers = _numbers;
            var values = _values;
            numbers.EnsureCapacity(count);
            values.EnsureCapacity(count);
            for (var i = 0; i < count; i++)
            {
                var number = reader.ReadInt16();
                var value = reader.ReadDouble();
                numbers.Add(number);
                values.Add(value);
            }
        }
    }
}
