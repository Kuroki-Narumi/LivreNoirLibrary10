using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension(IDefList obj)
        {
            public void Merge(IDefList source)
            {
                if (obj is DefList tgt && source is DefList src)
                {
                    SortedList.CopyTo(src._keys, src._values, tgt._keys, tgt._values);
                }
            }

            public void Swap(short key1, short key2)
            {
                obj.TryGetValue(key2, out var value2);
                if (obj.TryGetValue(key1, out var value1))
                {
                    obj.Set(key2, value1);
                }
                else
                {
                    obj.Remove(key2);
                }
                if (value2 is not null)
                {
                    obj.Set(key1, value2);
                }
                else
                {
                    obj.Remove(key1);
                }
            }

            public void Map(DefIndexMap map)
            {
                var old = obj.ToList();
                obj.Clear();
                foreach (var (key, value) in old.AsSpan())
                {
                    var newKey = map[key];
                    if (newKey is >= 0)
                    {
                        obj.Set(newKey, value);
                    }
                }
            }

            public bool ClearWithoutZero(DefIndexMap? map = null)
            {
                var modified = false;
                string? zeroValue = null;
                foreach (var (k, value) in obj)
                {
                    if (k is 0)
                    {
                        zeroValue = value;
                    }
                    else
                    {
                        map?.SetRemove(k);
                        modified = true;
                    }
                }
                obj.Clear();
                if (!string.IsNullOrEmpty(zeroValue))
                {
                    obj.Set(0, zeroValue);
                }
                return modified;
            }

            public int RemoveAll(Func<short, string, bool> selector, DefIndexMap? map = null)
            {
                var remove = ObjectPool.Rent<List<short>>();
                try
                {
                    foreach (var (key, value) in obj)
                    {
                        if (key is not 0 && selector(key, value))
                        {
                            remove.Add(key);
                            map?.SetRemove(key);
                        }
                    }
                    foreach (var key in remove.AsSpan())
                    {
                        obj.Remove(key);
                    }
                    var result = remove.Count;
                    return result;
                }
                finally
                {
                    ObjectPool.Return(remove);
                }
            }

            public int RemoveWithBasename(string basename, DefIndexMap? map = null) => obj.RemoveAll((key, value) => value.StartsWith(basename, StringComparison.Ordinal), map);

            public int RemoveUnused(HashSet<short> used, DefIndexMap? map = null) => obj.RemoveAll((key, value) => !used.Contains(key), map);

            public DefIndexMap GetSortedMap(HashSet<short> used, HashSet<short> @fixed, int headroom, bool sortByName)
            {
                HashSet<(short, string?)> targets = [];
                foreach (var (key, value) in obj)
                {
                    targets.Add((key, value));
                }
                foreach (var key in used)
                {
                    targets.Add(new(key, obj.TryGetValue(key, out var value) ? value : null));
                }
                DefIndexMap result = new();
                var mapped = ArrayPool<byte>.Shared.Rent(BmsConstants.DefMax_Extended);
                try
                {
                    Array.Clear(mapped);
                    var index = (short)headroom;
                    foreach (var (key, _) in targets.Order(sortByName ? SortItemComparer_Value.Instance : SortItemComparer_Id.Instance))
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
        }

        private class SortItemComparer_Id : IComparer<(short, string?)>
        {
            public static SortItemComparer_Id Instance { get; } = new();
            public int Compare((short, string?) x, (short, string?) y) => x.Item1.CompareTo(y.Item1);
        }

        private class SortItemComparer_Value : IComparer<(short, string?)>
        {
            public static SortItemComparer_Value Instance { get; } = new();
            public int Compare((short, string?) x, (short, string?) y)
            {
                var (xi, xv) = x;
                var (yi, yv) = y;
                var c = StringExtensions.CompareByNaturalOrder(xv, yv, false);
                return c is 0 ? xi.CompareTo(yi) : c;
            }
        }
    }
}