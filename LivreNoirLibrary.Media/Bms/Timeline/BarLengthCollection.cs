using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public class BarLengthCollection : IEnumerable<(int Number, Rational Length)>, IJsonWriter, IDumpable<BarLengthCollection>
    {
        public BarLengthCollection? Parent { get; set; }

        private readonly List<int> _number_list = [];
        private readonly List<Rational> _value_list = [];

        public Rational this[int number]
        {
            get => Get(number);
            set => Set(number, value);
        }

        public int Count => _number_list.Count;
        public bool IsEmpty() => _number_list.Count is 0;
        public int LastNumber => _number_list.Count is > 0 ? _number_list[^1] : 0;

        public void Clear()
        {
            _number_list.Clear();
            _value_list.Clear();
        }

        protected bool TryGetIndex(int number, out int index)
        {
            index = _number_list.BinarySearch(number);
            return index is >= 0;
        }

        public bool TryGetValue(int number, out Rational value)
        {
            if (TryGetIndex(number, out var index))
            {
                value = _value_list[index];
                return true;
            }
            value = default;
            return false;
        }

        public Rational Get(int number)
        {
            if (TryGetValue(number, out Rational value))
            {
                return value;
            }
            if (Parent is not null)
            {
                return Parent.Get(number);
            }
            return Constants.DefaultBarLength;
        }

        public Rational GetDefault(int number)
        {
            if (Parent is not null)
            {
                if (Parent.TryGetValue(number, out Rational value))
                {
                    return value;
                }
                return Parent.GetDefault(number);
            }
            return Constants.DefaultBarLength;
        }

        public bool Remove(int number)
        {
            if (TryGetIndex(number, out var index))
            {
                RemoveItem(index);
                return true;
            }
            return false;
        }

        public bool Set(int number, in Rational value)
        {
            if ((uint)number is > Constants.MaxBarNumber) { return false; }
            if (value == GetDefault(number))
            {
                return Remove(number);
            }
            else
            {
                return SetItem(number, value);
            }
        }

        private bool SetItem(int number, in Rational value)
        {
            if (TryGetIndex(number, out var index))
            {
                if (_value_list[index] == value)
                {
                    return false;
                }
                _value_list[index] = value;
            }
            else
            {
                InsertItem(~index, number, value);
            }
            return true;
        }

        public void Insert(int number, in Rational value)
        {
            if (!TryGetIndex(number, out var index))
            {
                index = ~index;
            }
            var c = _number_list.Count;
            for (var i = index; i < c; i++)
            {
                _number_list[i] += 1;
            }
            if (GetDefault(number) != value)
            {
                InsertItem(index, number, value);
            }
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
            var c = _number_list.Count;
            for (var i = index; i < c; i++)
            {
                _number_list[i] -= 1;
            }
        }

        public void MergeLines(int number, int count)
        {
            var length = Rational.Zero;
            for (var i = 0; i < count; i++)
            {
                length += Get(number + i);
            }
            Set(number, length);
            Delete(number + 1, count - 1);
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
            var c = _number_list.Count;
            for (var i = index; i < c; i++)
            {
                _number_list[i] -= count;
            }
        }

        private void RemoveItem(int index)
        {
            _number_list.RemoveAt(index);
            _value_list.RemoveAt(index);
        }

        private void InsertItem(int index, int number, Rational value)
        {
            _number_list.Insert(index, number);
            _value_list.Insert(index, value);
        }

        public IEnumerator<(int Number, Rational Length)> GetEnumerator()
        {
            var c = _number_list.Count;
            for (int i = 0; i < c; i++)
            {
                yield return (_number_list[i], _value_list[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Merge(BarLengthCollection source)
        {
            var ns = source._number_list;
            var vs = source._value_list;
            var c = ns.Count;
            for (int i = 0; i < c; i++)
            {
                SetItem(ns[i], vs[i]);
            }
        }

        public void Dump(BinaryWriter writer)
        {
            var c = _number_list.Count;
            writer.Write(c);
            for (int i = 0; i < c; i++)
            {
                writer.Write((short)_number_list[i]);
                writer.Write(_value_list[i]);
            }
        }

        public void ProcessLoad(BinaryReader reader)
        {
            Clear();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var number = reader.ReadInt16();
                var value = reader.ReadRational();
                _number_list.Add(number);
                _value_list.Add(value);
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
            var c = _number_list.Count;
            for (int i = 0; i < c; i++)
            {
                writer.WriteString(_number_list[i].ToString(), _value_list[i].ToString());
            }
            writer.WriteEndObject();
        }
    }
}
