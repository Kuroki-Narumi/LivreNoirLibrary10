using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Bmst
{
    public abstract class DefListBase<T> : ISafeEnumerable<(int, T)>, ICount, IClear
    {
        private readonly List<int> _keys = [];
        private readonly List<T> _values = [];

        public int Count => _keys.Count;
        public int MaxIndex => Count is 0 ? 0 : _keys[^1];

        public void Clear()
        {
            _keys.Clear();
            _values.Clear();
        }

        public bool ContainsKey(int key) => SortedList.ContainsKey(_keys, key);
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out T value) => SortedList.TryGetValue(_keys, _values, key, out _, out value);
        public bool TryGetKey(T value, out int key) => SortedList.TryGetKey(_keys, _values, value, out _, out key);

        public void Set(int key, T value)
        {
            if (IsEmptyValue(value))
            {
                Remove(key);
                return;
            }
            SortedList.AddOrReplace(_keys, _values, key, value);
        }

        public bool Remove(int key) => SortedList.Remove(_keys, _values, key);

        public void Dump(BinaryWriter writer, Action<BinaryWriter, T> writeFunc)
        {
            writer.Write(_keys.Count);
            foreach (var (key, value) in this)
            {
                writer.Write(key);
                writeFunc(writer, value);
            }
        }

        public void ProcessLoad(BinaryReader reader, Func<BinaryReader, T> readFunc)
        {
            Clear();
            var keys = _keys;
            var values = _values;
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                keys.Add(reader.ReadInt16());
                values.Add(readFunc(reader));
            }
        }

        public SortedList.Enumerator<int, T> GetEnumerator() => SortedList.GetEnumerator(_keys, _values);
        IEnumerator<(int, T)> IEnumerable<(int, T)>.GetEnumerator() => SortedList.GetSafeEnumerator(_keys, _values);

        protected virtual bool IsEmptyValue(T value) => false;
    }
}
