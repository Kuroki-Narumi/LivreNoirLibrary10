using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class DefListCollection : SortedList<DefType, DefList>, IDefListCollection, IDumpable, ILoadable<DefListCollection>
    {
        public bool Contains(DefType type) => ContainsKey(type);

        public bool TryGetList(DefType type, [MaybeNullWhen(false)] out IDefList list)
        {
            if (TryGetValue(type, out var defList))
            {
                list = defList;
                return true;
            }
            list = null;
            return false;
        }

        public IDefList GetOrAddList(DefType type) => this.GetOrAdd(type);

        public bool RemoveList(DefType type) => Remove(type);

        public IEnumerable<(DefType, IDefList)> EnumerateList()
        {
            foreach (var (key, value) in this)
            {
                yield return (key, value);
            }
        }

        public void Merge(DefList src, DefType type)
        {
            this.GetOrAdd(type).Merge(src);
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

        public const string DefFormat = "{0}{1} {2}";

        public void Dump(BmsTextWriter writer)
        {
            var radix = writer.Radix;
            void Write(string type, short key, string value)
            {
                writer.WriteLine(DefFormat, type, BmsUtils.ToBased(key, radix), value);
            }
            bool CheckSpecial(string tag, short key, string value)
            {
                var reqLength = tag.Length + 1;
                if (value.Length >= reqLength && value.StartsWith(tag, StringComparison.InvariantCultureIgnoreCase))
                {
                    Write(tag, key, value[reqLength..]);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            foreach (var type in Enum.GetValues<DefType>())
            {
                if (TryGetValue(type, out var list) && list.Count is > 0)
                {
                    if (type is DefType.Wav)
                    {
                        foreach (var (key, value) in list)
                        {
                            if (CheckSpecial(Tags.ExWav, key, value))
                            {
                                Write(Tags.Wav, key, value);
                            }
                        }
                    }
                    else if (type is DefType.Bmp)
                    {
                        foreach (var (key, value) in list)
                        {
                            if (CheckSpecial(Tags.Bga, key, value) &&
                                CheckSpecial(Tags.ExBmp, key, value) &&
                                CheckSpecial(Tags.AtBga, key, value))
                            {
                                Write(Tags.Bmp, key, value);
                            }
                        }
                    }
                    else
                    {
                        var t = type.ToString().ToUpper();
                        foreach (var (key, value) in list)
                        {
                            Write($"#{t}", key, value);
                        }
                    }
                    writer.WriteEmpty();
                }
            }
        }

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
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var type = (DefType)reader.ReadByte();
                var list = DefList.Load(reader);
                Add(type, list);
            }
        }

        public DefListCollection Clone()
        {
            DefListCollection result = [];
            foreach (var (type, list) in this)
            {
                result.Add(type, list.Clone());
            }
            return result;
        }

        public void WriteJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            foreach (var (type, list) in this)
            {
                writer.WritePropertyName(type.ToString());
                list.WriteJson(writer);
            }
            writer.WriteEndObject();
        }
    }
}
