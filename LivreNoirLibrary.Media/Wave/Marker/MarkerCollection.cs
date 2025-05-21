using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Wave
{
    public class MarkerCollection : ICollection<Marker>
    {
        private readonly List<long> _pos_list = [];
        private readonly List<string?> _value_list = [];

        public int Count => _pos_list.Count;
        private int IndexOf(long position) => _pos_list.BinarySearch(position);

        public void Clear()
        {
            _pos_list.Clear();
            _value_list.Clear();
        }

        public void Set(long position, string? name) => _pos_list.AddOrReplace(_value_list, position, name);
        public void SetIgnore(long position) => Set(position, Marker.IgnoreName);
        public bool Remove(long position) => _pos_list.Remove(_value_list, position);

        public bool RemoveRange(long position, long length)
        {
            var pos = _pos_list;
            var value = _value_list;
            var limit = position + length;
            var index = pos.FindIndex(position, SearchMode.NextOrEqual);
            var flag = false;
            if (index is >= 0)
            {
                while (index < pos.Count)
                {
                    if (pos[index] < limit)
                    {
                        pos.RemoveAt(index);
                        value.RemoveAt(index);
                        flag = true;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            return flag;
        }

        public bool Contains(long position) => IndexOf(position) is >= 0;

        public void Add(Marker item) => Set(item.Position, item.Name);
        public void Set(in Marker item) => Set(item.Position, item.Name);
        public bool Remove(Marker item) => Remove(item.Position);
        public bool Contains(Marker item) => Contains(item.Position);

        public void Load(ReadOnlySpan<Marker> source)
        {
            Clear();
            foreach (var item in source)
            {
                Add(item);
            }
        }

        public void Load(MarkerCollection source)
        {
            Clear();
            _pos_list.AddRange(source._pos_list);
            _value_list.AddRange(source._value_list);
        }

        public void CopyTo(MarkerCollection target)
        {
            var pos = _pos_list;
            var value = _value_list;
            var count = pos.Count;
            for (var i = 0; i < count; i++)
            {
                target.Set(pos[i], value[i]);
            }
        }

        public Marker[] ToArray()
        {
            var pos = _pos_list;
            var value = _value_list;
            var count = pos.Count;
            var result = new Marker[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = new(pos[i], value[i]);
            }
            return result;
        }

        public Marker[] ToArray(long first, long last, bool includeEnd)
        {
            var pos = _pos_list;
            var value = _value_list;
            var (s, l) = pos.IndexRange(RangeUtils.Get(first, last, includeEnd));
            var e = s + l;
            var result = new Marker[l];
            for (var i = s; i < e; i++)
            {
                result[i - s] = new(pos[i], value[i]);
            }
            return result;
        }

        public IEnumerator<Marker> GetEnumerator()
        {
            var pos = _pos_list;
            var value = _value_list;
            var count = pos.Count;
            for (var i = 0; i < count; i++)
            {
                yield return new(pos[i], value[i]);
            }
        }

        public IEnumerable<Marker> Range(long first, long last, bool includeEnd)
        {
            var pos = _pos_list;
            var value = _value_list;
            var (s, l) = pos.IndexRange(RangeUtils.Get(first, last, includeEnd));
            var e = s + l;
            for (var i = s; i < e; i++)
            {
                yield return new(pos[i], value[i]);
            }
        }

        public bool TryGet(long position, SearchMode mode, out Marker marker)
        {
            if (_pos_list.TrySearch(position, mode, out var index, out var actPos))
            {
                marker = new(actPos, _value_list[index]);
                return true;
            }
            else
            {
                marker = default;
                return false;
            }
        }

        public bool TryGetNearest(long position, out Marker marker)
        {
            if (_pos_list.FindNearest(position, out var index, out var actPos))
            {
                marker = new(actPos, _value_list[index]);
                return true;
            }
            else
            {
                marker = default;
                return false;
            }
        }

        public bool TryGetByName(string name, out Marker marker)
        {
            var vals = _value_list;
            var index = vals.IndexOf(name);
            if (index is >= 0)
            {
                marker = new(_pos_list[index], vals[index]);
                return true;
            }
            else
            {
                marker = default;
                return false;
            }
        }

        public long GetLength(long position, long limit) => TryGet(position, SearchMode.Next, out var next) ? next.Position - position : Math.Max(limit - position, 0);
        public long GetLength(in Marker item, long limit) => GetLength(item.Position, limit);

        public bool EnsureName()
        {
            var modified = false;
            Dictionary<string, int> duplicated = [];
            var values = _value_list;
            var count = values.Count;
            var fmt = SliceUtils.GetIndexFormat(count);
            for (var i = 0; i < count; i++)
            {
                var name = values[i];
                if (name is not Marker.IgnoreName)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        name = string.Format(fmt, i);
                    }
                    if (duplicated.TryGetValue(name, out var dc))
                    {
                        dc++;
                        duplicated[name] = dc;
                        var newName = $"{name}_{dc}";
                        values[i] = newName;
                        modified = true;
                        duplicated[newName] = 1;
                    }
                    else
                    {
                        duplicated.Add(name, 1);
                    }
                }
            }
            return modified;
        }

        public IEnumerable<MarkerInfo> EachMarkerWithLength(long limit, bool skipIgnoreName)
        {
            var poss = _pos_list;
            var values = _value_list;
            var count = Count;
            skipIgnoreName = !skipIgnoreName;
            if (count is > 0)
            {
                var lastPos = poss[0];
                var lastName = values[0];
                if (lastPos >= limit)
                {
                    yield break;
                }
                var i = 1;
                var j = 0;
                for (; i < count; i++)
                {
                    var pos = poss[i];
                    var name = values[i];
                    if (pos >= limit)
                    {
                        break;
                    }
                    if (skipIgnoreName || lastName is not Marker.IgnoreName)
                    {
                        yield return new(j++, lastName, lastPos, pos - lastPos);
                    }
                    lastPos = pos;
                    lastName = name;
                }
                if (skipIgnoreName || lastName is not Marker.IgnoreName)
                {
                    yield return new(j++, lastName, lastPos, limit - lastPos);
                }
            }
        }

        bool ICollection<Marker>.IsReadOnly => false;

        void ICollection<Marker>.CopyTo(Marker[] array, int arrayIndex)
        {
            var count = Count;
            if (arrayIndex + count > array.Length)
            {
                throw new IndexOutOfRangeException();
            }
            var pos = _pos_list;
            var value = _value_list;
            for (var i = 0; i < count; i++)
            {
                array[arrayIndex++] = new(pos[i], value[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public long GetValidCount()
        {
            var count = 0;
            foreach (var name in CollectionsMarshal.AsSpan(_value_list))
            {
                if (name is not Marker.IgnoreName)
                {
                    count++;
                }
            }
            return count;
        }

        internal List<long> GetPosList() => _pos_list;
        internal (List<long> PosList, List<string?> ValueList) GetLists() => (_pos_list, _value_list);
    }
}
