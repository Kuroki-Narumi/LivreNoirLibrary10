using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        public const string DefFormat = "{0}{1} {2}";

        extension(IDefListCollection obj)
        {
            public int MaxIndex
            {
                get
                {
                    var c = 0;
                    foreach (var (_, list) in obj.EnumerateList())
                    {
                        c = Math.Max(c, list.MaxIndex);
                    }
                    return c;
                }
            }

            public bool HasValue
            {
                get
                {
                    foreach (var (_, list) in obj.EnumerateList())
                    {
                        if (list.Count is > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public bool ContainsKey(DefType type, int key) => obj.TryGetList(type, out var list) && list.ContainsKey((short)key);

            public bool TryGetValue(DefType type, int key, [MaybeNullWhen(false)] out string value)
            {
                if (obj.TryGetList(type, out var list) && list.TryGetValue((short)key, out value))
                {
                    return true;
                }
                value = null;
                return false;
            }

            public bool TryGetKey(DefType type, string value, out int key)
            {
                if (obj.TryGetList(type, out var list) && list.TryGetKey(value, out var k))
                {
                    key = k;
                    return true;
                }
                key = -1;
                return false;
            }

            public bool Set(DefType type, int key, string? value)
            {
                if (value is null)
                {
                    return obj.Remove(type, key);
                }
                // compatible
                switch (type)
                {
                    case DefType.ExWav:
                        value = $"{Tags.ExWav} {value}";
                        type = DefType.Wav;
                        break;
                    case DefType.Bga:
                        value = $"{Tags.Bga} {value}";
                        type = DefType.Bmp;
                        break;
                    case DefType.ExBmp:
                        value = $"{Tags.ExBmp} {value}";
                        type = DefType.Bmp;
                        break;
                    case DefType.AtBga:
                        value = $"{Tags.AtBga} {value}";
                        type = DefType.Bmp;
                        break;
                }
                var list = obj.GetOrAddList(type);
                var i = (short)key;
                if (!list.TryGetValue(i, out var current) || current != value)
                {
                    list.Set(i, value);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool Remove(DefType type, int key)
            {
                // compatible
                type = type switch
                {
                    DefType.ExWav => DefType.Wav,
                    DefType.Bga or DefType.ExBmp or DefType.AtBga => DefType.Bga,
                    _ => type,
                };
                if (obj.TryGetList(type, out var list))
                {
                    if (list.Remove((short)key))
                    {
                        if (list.Count is 0)
                        {
                            obj.RemoveList(type);
                        }
                        return true;
                    }
                }
                return false;
            }

            public void Merge(IDefListCollection source)
            {
                foreach (var (type, list) in source.EnumerateList())
                {
                    obj.Merge(type, list);
                }
            }

            public void Merge(DefType type, IDefList source)
            {
                obj.GetOrAddList(type).Merge(source);
            }

            public int FindFreeDefIndex(DefType type, int start = 1)
            {
                for (; obj.ContainsKey(type, start); start++) ;
                return start;
            }

            public bool RemoveUnused(IDictionary<DefType, DefIndexMap> maps, DefIndexCollection used)
            {
                var modified = false;
                var remove = ObjectPool.Rent<List<DefType>>();
                try
                {
                    foreach (var (type, list) in obj.EnumerateList())
                    {
                        var map = maps.GetOrAdd(type);
                        if (used.TryGetValue(type, out var set))
                        {
                            if (list.RemoveUnused(set, map) is > 0)
                            {
                                modified = true;
                            }
                        }
                        else if (list.ClearWithoutZero(map))
                        {
                            modified = true;
                        }
                        if (list.Count is 0)
                        {
                            remove.Add(type);
                        }
                    }
                    foreach (var type in remove.AsSpan())
                    {
                        obj.RemoveList(type);
                    }
                    return modified;
                }
                finally
                {
                    ObjectPool.Return(remove);
                }
            }

            public void TryEncode(Encoding encoding)
            {
                foreach (var (_, list) in obj.EnumerateList())
                {
                    foreach (var (_, value) in list)
                    {
                        encoding.GetByteCount(value);
                    }
                }
            }

            public void Dump(BmsTextWriter writer, int radix, bool isRoot)
            {
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
                    if (obj.TryGetList(type, out var list) && list.Count is > 0)
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
                        if (isRoot)
                        {
                            writer.WriteLine();
                        }
                    }
                }
            }

            public void Dump(BinaryWriter writer)
            {
                writer.Write(obj.Count);
                foreach (var (type, list) in obj.EnumerateList())
                {
                    writer.Write((byte)type);
                    list.Dump(writer);
                }
            }

            public void ProcessLoad(BinaryReader reader)
            {
                var loaded = ObjectPool.Rent<List<DefType>>();
                try
                {
                    loaded.AddRange(obj.EnumerateList().Select(kv => kv.Item1));
                    var count = reader.ReadInt32();
                    for (var i = 0; i < count; i++)
                    {
                        var type = (DefType)reader.ReadByte();
                        obj.GetOrAddList(type).ProcessLoad(reader);
                        loaded.Remove(type);
                    }
                    foreach (var type in loaded)
                    {
                        obj.RemoveList(type);
                    }
                }
                finally
                {
                    ObjectPool.Return(loaded);
                }
            }
        }
    }
}