using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefList : IDefList, IDumpable, ILoadable<DefList>
    {
        internal readonly List<short> _keys;
        internal readonly List<string> _values;
        private int _version;

        public DefList()
        {
            _keys = [];
            _values = [];
        }

        public DefList(int capacity)
        {
            _keys = new(capacity);
            _values = new(capacity);
        }

        private DefList(List<short> keys, List<string> values)
        {
            _keys = keys;
            _values = values;
        }

        public int Count => _keys.Count;
        public int MaxIndex => Count is 0 ? 0 : _keys[^1];

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

        public void Merge(DefList source)
        {
            SortedList.CopyTo(source._keys, source._values, _keys, _values);
            _version++;
        }

        public void Swap(short key1, short key2)
        {
            if (TryGetValue(key1, out var value1))
            {
                if (TryGetValue(key2, out var value2))
                {
                    _values[key1] = value2;
                    _values[key2] = value1;
                }
                else
                {
                    Remove(key1);
                    Set(key2, value1);
                }
            }
            else if (TryGetValue(key2, out var value2))
            {
                Remove(key2);
                Set(key1, value2);
            }
            _version++;
        }

        public void Map(DefIndexMap map)
        {
            var old = Clone();
            Clear();
            foreach (var (key, value) in old)
            {
                var newKey = map[key];
                if (newKey is >= 0)
                {
                    Set(newKey, value);
                }
            }
            _version++;
        }

        public void ClearWithoutZero(DefIndexMap map)
        {
            switch (Count)
            {
                case 1:
                    var key = _keys[0];
                    if (key is not 0)
                    {
                        map.SetRemove(key);
                        Clear();
                    }
                    break;
                case > 1:
                    string? zeroValue = null;
                    foreach (var (k, value) in this)
                    {
                        if (k is 0)
                        {
                            zeroValue = value;
                        }
                        else
                        {
                            map.SetRemove(k);
                        }
                    }
                    Clear();
                    if (!string.IsNullOrEmpty(zeroValue))
                    {
                        Set(0, zeroValue);
                    }
                    break;
            }
            _version++;
        }

        public void RemoveWithBasename(string basename, ICollection<short> removedKeys)
        {
            var keys = _keys;
            var values = _values;
            for (var i = 0; i < keys.Count;)
            {
                var key = keys[i];
                if (key is not 0 && values[i].StartsWith(basename, StringComparison.Ordinal))
                {
                    removedKeys.Add(key);
                    keys.RemoveAt(i);
                    values.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void RemoveUnused(HashSet<short> used, DefIndexMap map)
        {
            var keys = _keys;
            var values = _values;
            for (var i = 0; i < keys.Count;)
            {
                var key = keys[i];
                if (key is not 0 && !used.Contains(key))
                {
                    map.SetRemove(key);
                    keys.RemoveAt(i);
                    values.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            _version++;
        }

        public DefIndexMap GetSortedMap(HashSet<short> used, HashSet<short> @fixed, int headroom, bool sortByName)
        {
            HashSet<(short, string?)> targets = [];
            foreach (var (key, value) in this)
            {
                targets.Add(new(key, value));
            }
            foreach (var key in used)
            {
                targets.Add(new(key, TryGetValue(key, out var value) ? value : null));
            }
            DefIndexMap result = new();
            var mapped = ArrayPool<byte>.Shared.Rent(Constants.DefMax_Extended);
            try
            {
                Array.Clear(mapped);
                var index = (short)headroom;
                foreach (var (key, _) in targets.Order(sortByName ? new SortItemComparer_Value() : new SortItemComparer_Id()))
                {
                    if (key <= headroom || @fixed.Contains(key))
                    {
                        mapped[key] = 1;
                    }
                    else
                    {
                        while (mapped[index] is 1)
                        {
                            index++;
                        }
                        result.Set(key, index);
                        mapped[index] = 1;
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(mapped);
            }
            return result;
        }

        private readonly struct SortItemComparer_Id : IComparer<(short, string?)>
        {
            public int Compare((short, string?) x, (short, string?) y) => x.Item1.CompareTo(y.Item1);
        }

        private readonly struct SortItemComparer_Value : IComparer<(short, string?)>
        {
            public int Compare((short, string?) x, (short, string?) y)
            {
                var (xi, xv) = x;
                var (yi, yv) = y;
                var c = StringExtensions.CompareByNaturalOrder(xv, yv, false);
                return c is 0 ? xi.CompareTo(yi) : c;
            }
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(Count);
            foreach (var (key, value) in this)
            {
                writer.Write(key);
                writer.Write(value);
            }
        }

        public static DefList Load(BinaryReader stream)
        {
            var count = stream.ReadInt32();
            List<short> keys = new(count);
            List<string> values = new(count);
            for (var i = 0; i < count; i++)
            {
                keys.Add(stream.ReadInt16());
                values.Add(stream.ReadString());
            }
            return new(keys, values);
        }

        public DefList Clone() => new([.. _keys], [.. _values]);

        public void WriteJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            foreach (var (key, value) in this)
            {
                writer.WriteString(key.ToString(), value);
            }
            writer.WriteEndObject();
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
                _keys = CollectionsMarshal.AsSpan(defList._keys);
                _values = CollectionsMarshal.AsSpan(defList._values);
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
