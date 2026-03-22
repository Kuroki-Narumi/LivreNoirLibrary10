using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefList : IDefList
    {
        internal readonly List<short> _keys;
        internal readonly List<string> _values;

        public DefList()
        {
            _keys = new(BmsConstants.DefMax_Extended);
            _values = new(BmsConstants.DefMax_Extended);
        }

        public int Count => _keys.Count;
        public int MaxIndex => Count is 0 ? 0 : _keys[^1];
        public IEnumerable<short> Keys => _keys.AsReadOnly();

        public void Clear()
        {
            _keys.Clear();
            _values.Clear();
        }

        public bool ContainsKey(short key) => SortedList.ContainsKey(_keys, key);
        public bool TryGetValue(short key, [MaybeNullWhen(false)] out string value) => SortedList.TryGetValue(_keys, _values, key, out value);
        public bool TryGetKey(string value, out short key) => SortedList.TryGetKey(_keys, _values, value, out key);

        public void Set(short key, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Remove(key);
                return;
            }
            SortedList.AddOrReplace(_keys, _values, key, value);
        }

        public bool Remove(short key) => SortedList.Remove(_keys, _values, key);

        public void Dump(BinaryWriter writer)
        {
            writer.Write(_keys.Count);
            foreach (var (key, value) in this)
            {
                writer.Write(key);
                writer.Write(value);
            }
        }

        public void ProcessLoad(BinaryReader reader)
        {
            Clear();
            var keys = _keys;
            var values = _values;
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                keys.Add(reader.ReadInt16());
                values.Add(reader.ReadString());
            }
        }

        public SortedList.Enumerator<short, string> GetEnumerator() => SortedList.GetEnumerator(_keys, _values);
        IEnumerator<(short, string)> IEnumerable<(short, string)>.GetEnumerator() => SortedList.GetSafeEnumerator(_keys, _values);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => SortedList.GetSafeEnumerator(_keys, _values);
    }
}
