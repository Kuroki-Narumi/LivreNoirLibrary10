using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class BarLengthCollection : IEnumerable<KeyValuePair<int, Rational>>, IJsonWriter, IDumpable, ILoadable<BarLengthCollection>
    {
        private readonly List<int> _numbers = [];
        private readonly List<Rational> _values = [];

        public int Count => _numbers.Count;
        public int LastNumber => _numbers.Count is > 0 ? _numbers[^1] : 0;

        public void Clear()
        {
            _numbers.Clear();
            _values.Clear();
        }

        protected bool TryGetIndex(int number, out int index) => SortedList.TryGetIndex(_numbers, number, out index);
        public bool TryGetValue(int number, out Rational value) => SortedList.TryGetValue(_numbers, _values, number, out value);
        public bool Set(int number, in Rational value) => SortedList.AddOrReplace(_numbers, _values, number, value);
        public bool Remove(int number) => SortedList.Remove(_numbers, _values, number);

        public void Insert(int number, in Rational value)
        {
            if (!TryGetIndex(number, out var index))
            {
                index = ~index;
            }
            var c = _numbers.Count;
            for (var i = index; i < c; i++)
            {
                _numbers[i] += 1;
            }
            _numbers.Insert(index, number);
            _values.Insert(index, value);
        }

        public void Delete(int number)
        {
            if (TryGetIndex(number, out var index))
            {
                RemoveItem(index);
            }
            else
            {
                index = ~index;
            }
            var c = _numbers.Count;
            for (var i = index; i < c; i++)
            {
                _numbers[i] -= 1;
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
            for (var i = index; i < c; i++)
            {
                _numbers[i] -= count;
            }
        }

        private void RemoveItem(int index)
        {
            _numbers.RemoveAt(index);
            _values.RemoveAt(index);
        }

        public SortedList.Enumerator<int, Rational> GetEnumerator() => new(_numbers, _values);
        IEnumerator<KeyValuePair<int, Rational>> IEnumerable<KeyValuePair<int, Rational>>.GetEnumerator() => GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        public void Merge(BarLengthCollection source) => SortedList.CopyTo(source._numbers, source._values, _numbers, _values);

        public void Dump(BinaryWriter writer)
        {
            var c = _numbers.Count;
            writer.Write((ushort)c);
            for (var i = 0; i < c; i++)
            {
                writer.Write((short)_numbers[i]);
                writer.Write(_values[i]);
            }
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
                var value = reader.ReadRational();
                _numbers.Add(number);
                _values.Add(value);
            }
        }

        public static BarLengthCollection Load(BinaryReader reader)
        {
            BarLengthCollection result = new();
            result.ProcessLoad(reader);
            return result;
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            var c = _numbers.Count;
            for (var i = 0; i < c; i++)
            {
                writer.WriteString(_numbers[i].ToString(), _values[i].ToString());
            }
            writer.WriteEndObject();
        }
    }
}
