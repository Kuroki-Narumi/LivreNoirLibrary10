using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefList : IDefList
    {
        internal readonly List<short> _keys;
        internal readonly List<string> _values;
        private int _version;

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
            _version++;
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
            _version++;
        }

        public bool Remove(short key)
        {
            if (SortedList.Remove(_keys, _values, key))
            {
                _version++;
                return true;
            }
            return false;
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

        public Enumerator GetEnumerator() => new(this);

        private IEnumerator<(short, string)> GetEnumeratorInternal()
        {
            var version = _version;
            var keys = _keys;
            var values = _values;
            var count = keys.Count;
            for (var i = 0; i < count; i++)
            {
                if (_version != version)
                {
                    throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                }
                yield return (keys[i], values[i]);
            }
        }

        IEnumerator<(short, string)> IEnumerable<(short, string)>.GetEnumerator() => GetEnumeratorInternal();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumeratorInternal();

        public ref struct Enumerator : IEnumerator<(short Key, string Value)>
        {
            private readonly DefList _defList;
            private readonly ReadOnlySpan<short> _keys;
            private readonly ReadOnlySpan<string> _values;
            private readonly int _version;
            private readonly int _count;
            private int _index = 0;
            private (short, string) _current;

            internal Enumerator(DefList defList)
            {
                _defList = defList;
                _keys = defList._keys.AsSpan();
                _values = defList._values.AsSpan();
                _version = defList._version;
                _count = _keys.Length;
            }

            public readonly (short Key, string Value) Current => _current;
            readonly object System.Collections.IEnumerator.Current => _current;

            public readonly void Dispose() { }

            public bool MoveNext()
            {
                if (_version == _defList._version && _index < _count)
                {
                    _current = (_keys[_index], _values[_index]);
                    _index++;
                    return true;
                }
                else
                {
                    return MoveRare();
                }
            }

            private bool MoveRare()
            {
                if (_version != _defList._version)
                {
                    throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                }
                _index = _count + 1;
                _current = default;
                return false;
            }

            public void Reset()
            {
                _index = 0;
            }
        }
    }
}
