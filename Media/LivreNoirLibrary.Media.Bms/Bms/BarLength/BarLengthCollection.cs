using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public class BarLengthCollection : IBarLengthCollection, IClear
    {
        private readonly List<int> _numbers = [];
        private readonly List<double> _values = [];

        public int Count => _numbers.Count;
        public int LastNumber => _numbers.Count is > 0 ? _numbers[^1] : 0;

        public void Clear()
        {
            _numbers.Clear();
            _values.Clear();
        }

        protected bool TryGetIndex(int number, out int index) => SortedList.TryGetIndex(_numbers, number, out index);
        public bool TryGetValue(int number, out double value) => SortedList.TryGetValue(_numbers, _values, number, out value);
        public bool Set(int number, double value) => SortedList.AddOrReplace(_numbers, _values, number, value);
        public bool Remove(int number) => SortedList.Remove(_numbers, _values, number);

        public void Insert(int number, int count)
        {
            if (!TryGetIndex(number, out var index))
            {
                index = ~index;
            }
            var c = _numbers.Count;
            for (; index < c; index++)
            {
                _numbers[index] += count;
            }
        }

        public void Delete(int number, int count)
        {
            var index = 0;
            for (var i = 0; i < count; i++)
            {
                if (TryGetIndex(number + i, out index))
                {
                    RemoveItem(index);
                }
                else
                {
                    index = ~index;
                }
            }
            var c = _numbers.Count;
            for (; index < c; index++)
            {
                _numbers[index] -= count;
            }
        }

        private void RemoveItem(int index)
        {
            _numbers.RemoveAt(index);
            _values.RemoveAt(index);
        }

        public SortedList.Enumerator<int, double> GetEnumerator() => SortedList.GetEnumerator(_numbers, _values);
        IEnumerator<(int, double)> IEnumerable<(int, double)>.GetEnumerator() => SortedList.GetSafeEnumerator(_numbers, _values);

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
        }

        public void ProcessLoad(BinaryReader reader)
        {
            Clear();
            var count = (int)reader.ReadUInt16();
            _numbers.EnsureCapacity(count);
            _values.EnsureCapacity(count);
            for (var i = 0; i < count; i++)
            {
                var number = reader.ReadInt16();
                var value = reader.ReadDouble();
                _numbers.Add(number);
                _values.Add(value);
            }
        }
    }
}
