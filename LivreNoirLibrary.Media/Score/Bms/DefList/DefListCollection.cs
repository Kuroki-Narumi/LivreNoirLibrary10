using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using System.Diagnostics.Metrics;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class DefListCollection : SortedList<DefType, DefList>, IDumpable<DefListCollection>
    {
        public DefListCollection? Parent { get; set; }

        public int MaxIndex
        {
            get
            {
                var c = 0;
                foreach (var (_, list) in this)
                {
                    var cc = list.MaxIndex;
                    if (cc > c)
                    {
                        c = cc;
                    }
                }
                return c;
            }
        }

        public bool IsEmpty()
        {
            foreach (var (_, list) in this)
            {
                if (list.Count is > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ContainsKey(DefType type, int index) => TryGetValue(type, out var list) && list.ContainsKey((short)index);

        public bool TryGetValue(DefType type, int index, [MaybeNullWhen(false)]out string value)
        {
            if (TryGetValue(type, out var list) && list.TryGetValue((short)index, out value))
            {
                return true;
            }
            value = null;
            return false;
        }

        public bool ContainsInherited(DefType type, int index) => ContainsKey(type, index) || (Parent is not null && Parent.ContainsInherited(type, index));

        public DefList GetNew(DefType type)
        {
            if (!TryGetValue(type, out var list))
            {
                list = [];
                Add(type, list);
            }
            return list;
        }

        public string? Get(DefType type, int index, string? ifnone = null) => TryGetValue(type, index, out var value) ? value : ifnone;

        public string? GetParent(DefType type, int index, string? ifnone = null)
        {
            if (Parent is not null)
            {
                if (Parent.TryGetValue(type, index, out var value))
                {
                    return value;
                }
                return Parent.GetParent(type, index, ifnone);
            }
            return ifnone;
        }

        public string? GetInherited(DefType type, int index, string? ifnone = null)
        {
            if (TryGetValue(type, index, out var value))
            {
                return value;
            }
            if (Parent is not null)
            {
                return Parent.GetInherited(type, index, ifnone);
            }
            return ifnone;
        }

        public int FindFreeIndex(DefType type, int start = 1)
        {
            while (ContainsInherited(type, start))
            {
                start++;
            }
            return start;
        }

        public int ReverseFindFreeIndex(DefType type, int start)
        {
            while (start is > 0 && !ContainsInherited(type, start))
            {
                start--;
            }
            return start + 1;
        }

        public int FindIndex(DefType type, string value, bool addIfNone = false)
        {
            var exists = TryGetValue(type, out var list);
            if (!(exists && list!.TryFindIndex(value, out var index)))
            {
                index = (short)(Parent is not null ? Parent.FindIndex(type, value, false) : -1);
                if (index is < 0 && addIfNone)
                {
                    index = (short)FindFreeIndex(type);
                    if (!exists)
                    {
                        list = [];
                        Add(type, list);
                    }
                    list!.Add(index, value);
                }
            }
            return index;
        }

        public bool Set(DefType type, int index, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Remove(type, index);
            }
            if (Parent?.TryGetValue(type, out var list) is true && list.GetValueOrDefault((short)index, "") == value)
            {
                return false;
            }
            // compatible
            switch (type)
            {
                case DefType.ExWav:
                    value = $"<{Constants.DefType_ExWav}> {value}";
                    type = DefType.Wav;
                    break;
                case DefType.Bga:
                    value = $"<{Constants.DefType_Bga}> {value}";
                    type = DefType.Bmp;
                    break;
                case DefType.ExBmp:
                    value = $"<{Constants.DefType_ExBmp}> {value}";
                    type = DefType.Bmp;
                    break;
                case DefType.AtBga:
                    value = $"<{Constants.DefType_AtBga}> {value}";
                    type = DefType.Bmp;
                    break;
            }
            if (!TryGetValue(type, out list))
            {
                list = [];
                Add(type, list);
            }
            var i = (short)index;
            if (list.TryGetValue(i, out var current))
            {
                if (current != value)
                {
                    list[i] = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                list.Add(i, value);
                return true;
            }
        }

        public bool Remove(DefType type, int index)
        {
            // compatible
            switch (type)
            {
                case DefType.ExWav:
                    type = DefType.Wav;
                    break;
                case DefType.Bga:
                case DefType.ExBmp:
                case DefType.AtBga:
                    type = DefType.Bmp;
                    break;
            }
            if (TryGetValue(type, out var list))
            {
                if (list.Remove((short)index))
                {
                    if (list.Count is 0)
                    {
                        Remove(type);
                    }
                    return true;
                }
            }
            return false;
        }

        public void Merge(DefList src, DefType type)
        {
            if (!TryGetValue(type, out var dst))
            {
                dst = [];
                Add(type, dst);
            }
            dst.Merge(src);
        }

        public void Merge(DefListCollection src)
        {
            foreach (var vk in src)
            {
                Merge(vk.Value, vk.Key);
            }
        }

        internal void RemoveUnused(DefIndexMapCollection maps, DefIndexCollection used)
        {
            List<DefType> remove = [];
            foreach (var (type, list) in this)
            {
                var map = maps.GetOrAdd(type);
                if (used.TryGetValue(type, out var set))
                {
                    list.RemoveUnused(set, map);
                }
                else
                {
                    list.ClearWithoutZero(map);
                }
                if (list.Count is 0)
                {
                    remove.Add(type);
                }
            }
            foreach (var type in remove)
            {
                Remove(type);
            }
        }

        public IEnumerable<(int Index, string Name)> EachValue(DefType type, int maxIndex = Constants.DefMax_Default)
        {
            for (var i = 1; i < maxIndex; i++)
            {
                var name = GetInherited(type, i);
                if (!string.IsNullOrEmpty(name))
                {
                    yield return (i, name);
                }
            }
        }

        private Dictionary<string, List<(int, string)>> CreateDefDictionary()
        {
            Dictionary<string, List<(int, string)>> dic = [];
            foreach (var type in Enum.GetValues<DefType>())
            {
                if (TryGetValue(type, out var list))
                {
                    if (type is DefType.Wav)
                    {
                        foreach (var (index, value) in list)
                        {
                            var match = Regex_Wav.Match(value);
                            if (match.Success)
                            {
                                dic.Add(match.Groups["type"].Value, ((int)index, match.Groups["value"].Value));
                            }
                            else
                            {
                                dic.Add(Constants.DefType_Wav, ((int)index, value));
                            }
                        }
                    }
                    else if (type is DefType.Bmp)
                    {
                        foreach (var (index, value) in list)
                        {
                            var match = Regex_Bga.Match(value);
                            if (match.Success)
                            {
                                dic.Add(match.Groups["type"].Value, ((int)index, match.Groups["value"].Value));
                            }
                            else
                            {
                                dic.Add(Constants.DefType_Bmp, ((int)index, value));
                            }
                        }
                    }
                    else
                    {
                        List<(int, string)> values = [];
                        var key = type.ToString().ToUpper();
                        foreach (var (index, value) in list)
                        {
                            values.Add((index, value));
                        }
                        dic.Add(key, values);
                    }
                }
            }
            return dic;
        }

        public const string DefFormat = "#{0}{1} {2}";

        internal void Dump(RawData.BmsTextWriter s, int radix, bool dumpEmpty)
        {
            foreach (var (type, list) in CreateDefDictionary())
            {
                foreach (var (index, value) in list)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        s.Dump(DefFormat, type, BmsUtils.ToBased(index, radix), value);
                    }
                }
                if (dumpEmpty)
                {
                    s.DumpEmpty();
                }
            }
        }

        [GeneratedRegex(@$"^\s*<(?<type>{Constants.DefType_ExWav})>\s+(?<value>.+)$")]
        public static partial Regex Regex_Wav { get; }

        [GeneratedRegex(@$"^\s*<(?<type>{Constants.DefType_Bga}|{Constants.DefType_AtBga}|{Constants.DefType_ExBmp})>\s+(?<value>.+)$")]
        public static partial Regex Regex_Bga { get; }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(Count);
            foreach (var (type, list) in this)
            {
                writer.Write((byte)type);
                list.Dump(writer);
            }
        }

        public static DefListCollection Load(BinaryReader reader)
        {
            DefListCollection result = [];
            result.ProcessLoad(reader);
            return result;
        }

        public void ProcessLoad(BinaryReader reader)
        {
            Clear();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var type = (DefType)reader.ReadByte();
                var list = DefList.Load(reader);
                Add(type, list);
            }
        }

        public DefListCollection Clone()
        {
            DefListCollection result = [];
            foreach (var kv in this)
            {
                result.Add(kv.Key, kv.Value.Clone());
            }
            return result;
        }

        public DefListCollection GetForChild()
        {
            if (Parent is not null)
            {
                var list = Parent.GetForChild();
                list.Merge(this);
                return list;
            }
            return Clone();
        }

        public void WriteJson(Utf8JsonWriter writer, int radix)
        {
            writer.WriteStartObject();
            foreach (var (type, list) in this)
            {
                writer.WritePropertyName(type.ToString());
                list.WriteJson(writer, radix);
            }
            writer.WriteEndObject();
        }
    }
}
