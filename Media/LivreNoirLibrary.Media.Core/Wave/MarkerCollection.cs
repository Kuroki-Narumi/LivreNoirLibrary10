using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Wave
{
    public class MarkerCollection : IMarkerCollection
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

        public bool Contains(long position) => IndexOf(position) is >= 0;
        public void Set(long position, string? name) => SortedList.AddOrReplace(_pos_list, _value_list, position, name);
        public void SetIgnore(long position) => Set(position, Constants.IgnoreMarkerName);
        public bool Remove(long position) => SortedList.Remove(_pos_list, _value_list, position);

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

        public void Load(MarkerCollection source)
        {
            Clear();
            _pos_list.AddRange(source._pos_list);
            _value_list.AddRange(source._value_list);
        }

        public void CopyTo(MarkerCollection target) => SortedList.CopyTo(_pos_list, _value_list, target._pos_list, target._value_list);

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

        public (long NewStart, long NewEnd) Shift(long start, long amount, long limit, bool singleMove)
        {
            var keys = _pos_list;
            var count = keys.Count;
            var index = keys.FindNearestIndex(start);
            var leftLimit = index is > 0 ? keys[index - 1] + 1 : 0;
            var rightLimit = (singleMove && index < count - 1 ? keys[index + 1] : limit) - 1;
            var left = keys[index];
            if (singleMove)
            {
                keys[index] = left = Math.Clamp(left + amount, leftLimit, rightLimit);
            }
            else
            {
                var right = keys[^1];
                amount = Math.Clamp(amount, leftLimit - left, rightLimit - right);
                if (left + amount < leftLimit)
                {
                    amount = leftLimit - left;
                }
                if (right + amount > rightLimit)
                {
                    amount = rightLimit - right;
                }
                left += amount;
                for (var i = index; i < count; i++)
                {
                    keys[i] += amount;
                }
            }
            return (left, rightLimit + 1);
        }

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
                if (name is not Constants.IgnoreMarkerName)
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

        public IEnumerable<MarkerInfo> EnumerateWithLength(long limit, bool skipIgnoreName)
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
                    if (skipIgnoreName || lastName is not Constants.IgnoreMarkerName)
                    {
                        yield return new(j++, lastName, lastPos, pos - lastPos);
                    }
                    lastPos = pos;
                    lastName = name;
                }
                if (skipIgnoreName || lastName is not Constants.IgnoreMarkerName)
                {
                    yield return new(j++, lastName, lastPos, limit - lastPos);
                }
            }
        }

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

        public long GetValidCount()
        {
            var count = 0;
            foreach (var name in _value_list.AsSpan())
            {
                if (name is not Constants.IgnoreMarkerName)
                {
                    count++;
                }
            }
            return count;
        }

        public List<long> GetPosList() => _pos_list;
        public (List<long> PosList, List<string?> ValueList) GetLists() => (_pos_list, _value_list);
    }
}
