using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefList : SortedList<short, string>, IDumpable<DefList>
    {
        public int MaxIndex => Count is 0 ? 0 : Keys[^1];

        public new bool TryGetValue(short index, [MaybeNullWhen(false)] out string value) => base.TryGetValue(index, out value) && value.Length is > 0;

        public bool TryFindIndex(string value, out short index)
        {
            foreach (var (i, val) in this)
            {
                if (val == value)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        public void Merge(DefList src)
        {
            foreach (var kv in src)
            {
                this[kv.Key] = kv.Value;
            }
        }

        public void Swap(short index1, short index2)
        {
            if (TryGetValue(index1, out var value1))
            {
                if (TryGetValue(index2, out var value2))
                {
                    this[index1] = value2;
                    this[index2] = value1;
                }
                else
                {
                    Add(index2, value1);
                    Remove(index1);
                }
            }
            else if (TryGetValue(index2, out var value2))
            {
                Add(index1, value2);
                Remove(index2);
            }
        }

        public void Swap(int index1, int index2) => Swap((short)index1, (short)index2);

        public void Map(DefIndexMap map)
        {
            var old = Clone();
            Clear();
            foreach (var (index, value) in old)
            {
                var newIndex = map[index];
                if (newIndex is >= 0)
                {
                    this[newIndex] = value;
                }
            }
        }

        internal void ClearWithoutZero(DefIndexMap map)
        {
            switch (Count)
            {
                case 1:
                    var key = Keys[0];
                    if (key is not 0)
                    {
                        map.SetRemove(key);
                        Remove(key);
                    }
                    break;
                case > 1:
                    string? zeroValue = null;
                    foreach (var (index, value) in this)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (index is 0)
                            {
                                zeroValue = value;
                            }
                            else
                            {
                                map.SetRemove(index);
                            }
                        }
                    }
                    Clear();
                    if (zeroValue is not null)
                    {
                        Add(0, zeroValue);
                    }
                    break;
            }
        }

        internal void RemoveUnused(HashSet<short> used, DefIndexMap map)
        {
            var keys = Keys.ToArray();
            foreach (var index in keys)
            {
                if (!used.Contains(index))
                {
                    map.SetRemove(index);
                    Remove(index);
                }
            }
        }

        internal DefIndexMap GetSortedMap(HashSet<short> used, HashSet<short> @fixed, int headroom, bool sortByName)
        {
            HashSet<SortItem> targets = [];
            foreach (var (id, value) in this)
            {
                targets.Add(new(id, value));
            }
            foreach (var id in used)
            {
                targets.Add(new(id, TryGetValue(id, out var value) ? value : null));
            }
            DefIndexMap result = new();
            var mapped = ArrayPool<byte>.Shared.Rent(Constants.DefMax_Extended);
            try
            {
                Array.Clear(mapped);
                var index = (short)headroom;
                foreach (var (id, _) in targets.Order(sortByName ? new SortItemComarer_Value() : new SortItemComparer_Id()))
                {
                    if (id <= headroom || @fixed.Contains(id))
                    {
                        mapped[id] = 1;
                    }
                    else
                    {
                        while (mapped[index] is 1)
                        {
                            index++;
                        }
                        result.Set(id, index);
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

        private readonly struct SortItem(short id, string? value)
        {
            public readonly short Id = id;
            public readonly string? Value = value;

            public override int GetHashCode() => Id.GetHashCode();
            public bool Equals(SortItem other) => Id == other.Id;
            public override bool Equals([NotNullWhen(true)] object? obj) => obj is SortItem s && Equals(s);

            public void Deconstruct(out short id, out string? value)
            {
                id = Id;
                value = Value;
            }
        }

        private readonly struct SortItemComparer_Id : IComparer<SortItem>
        {
            public int Compare(SortItem x, SortItem y) => x.Id.CompareTo(y.Id);
        }

        private readonly struct SortItemComarer_Value : IComparer<SortItem>
        {
            public int Compare(SortItem x, SortItem y)
            {
                var (xi, xv) = x;
                var (yi, yv) = y;
                if (!string.IsNullOrEmpty(xv))
                {
                    if (!string.IsNullOrEmpty(yv))
                    {
                        var c = xv.CompareByNaturalOrder(yv);
                        if (c is not 0)
                        {
                            return c;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (!string.IsNullOrEmpty(yv))
                {
                    return 1;
                }
                return xi.CompareTo(yi);
            }
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(Count);
            foreach (var (index, value) in this)
            {
                writer.Write(index);
                writer.Write(value);
            }
        }

        public static DefList Load(BinaryReader stream)
        {
            DefList result = [];
            int count = stream.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var index = stream.ReadInt16();
                var value = stream.ReadString();
                result.Add(index, value);
            }
            return result;
        }

        public DefList Clone()
        {
            DefList result = [];
            foreach (var (index, value) in this)
            {
                result.Add(index, value);
            }
            return result;
        }

        public void WriteJson(Utf8JsonWriter writer, int radix)
        {
            writer.WriteStartObject();
            foreach (var (index, value) in this)
            {
                writer.WriteString(BmsUtils.ToBased(index, radix), value);
            }
            writer.WriteEndObject();
        }
    }
}
