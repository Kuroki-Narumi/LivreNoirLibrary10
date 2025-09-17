using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Files;

namespace LivreNoirLibrary.Media.Bms
{
    partial class IBmsDataExtensions
    {
        public static bool TryGetDef(this IBmsData data, DefType type, int index, [MaybeNullWhen(false)] out string value)
        {
            if (data.DefLists.TryGetValue(type, index, out value))
            {
                return true;
            }
            if (data.Parent is { } parent)
            {
                return TryGetDef(parent, type, index, out value);
            }
            value = default;
            return false;
        }

        public static void SetDef(this IBmsData data, DefType type, int index, string? value)
        {
            if (string.IsNullOrEmpty(value) || (data.Parent is { } parent && parent.TryGetDef(type, index, out var current) && current == value))
            {
                data.DefLists.Remove(type, index);
            }
            else
            {
                data.DefLists.Set(type, index, value);
            }
        }

        public static bool TryGetWavePath(this IBmsData data, int index, string root, [MaybeNullWhen(false)] out string name, [MaybeNullWhen(false)] out string path)
        {
            if (TryGetDef(data, DefType.Wav, index, out name) && FileUtils.TryGetAudioFileName(Path.GetFullPath(name, root), out path))
            {
                return true;
            }
            path = null;
            return false;
        }

        public static bool TryGetImagePath(this IBmsData data, int index, string root, [MaybeNullWhen(false)] out string name, [MaybeNullWhen(false)] out string path)
        {
            if (TryGetDef(data, DefType.Bmp, index, out name) && FileUtils.TryGetImageFileName(Path.GetFullPath(name, root), out path))
            {
                return true;
            }
            path = null;
            return false;
        }

        public static bool TryGetVideoPath(this IBmsData data, int index, string root, [MaybeNullWhen(false)] out string name, [MaybeNullWhen(false)] out string path)
        {
            if (TryGetDef(data, DefType.Bmp, index, out name) && FileUtils.TryGetVideoFileName(Path.GetFullPath(name, root), out path))
            {
                return true;
            }
            path = null;
            return false;
        }

        public static int FindFreeDefIndex(this IBmsData data, DefType type, int start = 1)
        {
            while (TryGetDef(data, type, start, out _))
            {
                start++;
            }
            return start;
        }

        public static bool TryGetDefIndex(this IBmsData data, DefType type, string value, out int key)
        {
            if (data.DefLists.TryGetKey(type, value, out key))
            {
                return true;
            }
            else if (data.Parent is { } parent)
            {
                return TryGetDefIndex(parent, type, value, out key);
            }
            key = -1;
            return false;
        }

        public static bool DefMoveDown(this IBmsData data, DefType type, List<int> indexes)
        {
            if (indexes.Count is 0)
            {
                return false;
            }
            else if (indexes.Count is 1)
            {
                var i = indexes[0];
                if ((uint)i < Constants.DefMax_Extended)
                {
                    DefSwap(data, type, i, i + 1);
                    return true;
                }
            }
            else
            {
                indexes.Sort();
                DefIndexMap map = new();
                var limit = (uint)Constants.DefMax_Extended;
                foreach (var index in CollectionsMarshal.AsSpan(indexes))
                {
                    var next = index + 1;
                    if ((uint)next >= limit) { continue; }
                    var current = map[index];
                    if (current != index)
                    {
                        map.Set(next, current);
                        map.Set(index, next);
                    }
                    else
                    {
                        map.Set(index, next);
                        map.Set(next, index);
                    }
                }
                if (map.IsEffective())
                {
                    DefMap(data, type, map);
                    return true;
                }
            }
            return false;
        }

        public static bool DefMoveUp(this IBmsData data, DefType type, List<int> indexes)
        {
            if (indexes.Count is 0)
            {
                return false;
            }
            if (indexes.Count is 1)
            {
                var i = indexes[0];
                if (i is > 1)
                {
                    DefSwap(data, type, i, i - 1);
                    return true;
                }
            }
            else
            {
                indexes.Sort();
                DefIndexMap map = new();
                for (var i = indexes.Count - 1; i is >= 0; i--)
                {
                    var index = indexes[i];
                    var next = index - 1;
                    if (next is < 1) { continue; }
                    var current = map[index];
                    if (current != index)
                    {
                        map.Set(next, current);
                        map.Set(index, next);
                    }
                    else
                    {
                        map.Set(index, next);
                        map.Set(next, index);
                    }
                }
                if (map.IsEffective())
                {
                    DefMap(data, type, map);
                    return true;
                }
            }
            return false;
        }

        public static void DefSwap(this IBmsData data, DefType type, int index1, int index2)
        {
            foreach (var d in data.EachData())
            {
                DefSwapCore(d, type, index1, index2);
                switch (type)
                {
                    case DefType.Wav:
                        DefSwap_Key(d, index1, index2);
                        break;
                    case DefType.Bmp:
                        DefSwap_Meta(d, index1, index2, Channel.Bga_Base, Channel.Bga_Layer1, Channel.Bga_Layer2, Channel.Bga_Poor);
                        break;
                    case DefType.ExRank:
                        DefSwap_Meta(d, index1, index2, Channel.ExRank);
                        break;
                    case DefType.Text:
                        DefSwap_Meta(d, index1, index2, Channel.Text);
                        break;
                    case DefType.Argb:
                        DefSwap_Meta(d, index1, index2, Channel.Argb_Base, Channel.Argb_Layer1, Channel.Argb_Layer2, Channel.Argb_Poor);
                        break;
                    case DefType.SwBga:
                        DefSwap_Meta(d, index1, index2, Channel.SwBga);
                        break;
                    case DefType.ChangeOption:
                        DefSwap_Meta(d, index1, index2, Channel.ChangeOption);
                        break;
                }
            }
        }

        private static void DefSwapCore(IBmsData data, DefType type, int index1, int index2)
        {
            if (data.DefLists.TryGetList(type, out var list))
            {
                list.Swap((short)index1, (short)index2);
            }
        }

        private static void DefSwap_Key(IBmsData data, int index1, int index2)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note is ISoundNote s)
                {
                    var value = s.Value;
                    if (value == index1)
                    {
                        s.Value = index2;
                    }
                    else if (value == index2)
                    {
                        s.Value = index1;
                    }
                }
            }
        }

        private static void DefSwap_Meta(IBmsData data, int index1, int index2, Channel channel)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note is IMetaNote s && s.Channel == channel)
                {
                    var value = s.Value;
                    if (value == index1)
                    {
                        s.Value = index2;
                    }
                    else if (value == index2)
                    {
                        s.Value = index1;
                    }
                }
            }
        }

        private static void DefSwap_Meta(IBmsData data, int index1, int index2, params ReadOnlySpan<Channel> channels)
        {
            var span = MemoryMarshal.Cast<Channel, short>(channels);
            foreach (var (_, note) in data.Timeline)
            {
                if (note is IMetaNote s && span.Contains((short)s.Channel))
                {
                    var value = s.Value;
                    if (value == index1)
                    {
                        s.Value = index2;
                    }
                    else if (value == index2)
                    {
                        s.Value = index1;
                    }
                }
            }
        }

        public static void DefMap(this IBmsData data, DefType type, DefIndexMap map)
        {
            foreach (var d in data.EachData())
            {
                DefMapCore(d, type, map);
                switch (type)
                {
                    case DefType.Wav:
                        DefMap_Key(d, map);
                        break;
                    case DefType.Bmp:
                        DefMap_Meta(d, map, Channel.Bga_Base, Channel.Bga_Layer1, Channel.Bga_Layer2, Channel.Bga_Poor);
                        break;
                    case DefType.ExRank:
                        DefMap_Meta(d, map, Channel.ExRank);
                        break;
                    case DefType.Text:
                        DefMap_Meta(d, map, Channel.Text);
                        break;
                    case DefType.Argb:
                        DefMap_Meta(d, map, Channel.Argb_Base, Channel.Argb_Layer1, Channel.Argb_Layer2, Channel.Argb_Poor);
                        break;
                    case DefType.SwBga:
                        DefMap_Meta(d, map, Channel.SwBga);
                        break;
                    case DefType.ChangeOption:
                        DefMap_Meta(d, map, Channel.ChangeOption);
                        break;
                }
            }
        }

        private static void DefMapCore(IBmsData data, DefType type, DefIndexMap map)
        {
            if (data.DefLists.TryGetList(type, out var list))
            {
                list.Map(map);
            }
        }

        private static void DefMap_Key(IBmsData data, DefIndexMap map)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note is ISoundNote s)
                {
                    s.Value = map[s.Value];
                }
            }
        }

        private static void DefMap_Meta(IBmsData data, DefIndexMap map, Channel channel)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note is IMetaNote s && s.Channel == channel)
                {
                    s.Value = map[s.Value];
                }
            }
        }

        private static void DefMap_Meta(IBmsData data, DefIndexMap map, params ReadOnlySpan<Channel> channels)
        {
            var span = MemoryMarshal.Cast<Channel, short>(channels);
            foreach (var (_, note) in data.Timeline)
            {
                if (note is IMetaNote s && span.Contains((short)s.Channel))
                {
                    s.Value = map[s.Value];
                }
            }
        }

        public static void RemoveDefWithBasename(this IBmsData data, DefType type, string basename)
        {
            HashSet<short> removeIds = [];
            foreach (var d in data.EachData())
            {
                if (d.DefLists.TryGetList(type, out var list))
                {
                    removeIds.Clear();
                    list.RemoveWithBasename(basename, removeIds);
                    d.Timeline.RemoveAll((_, n) => n.IsDefType(type, out var nn) && removeIds.Contains((short)nn.Value));
                }
            }
        }

        public static DefIndexCollection GetUsedDefList(this IBmsData data, DefIndexCollection? used = null)
        {
            used ??= [];
            foreach (var d in data.EachData())
            {
                foreach (var (_, note) in d.Timeline)
                {
                    if (note.TryGetDefType(out var type, out var actual) && actual.Value is not 0)
                    {
                        used.Add(type, actual.Value);
                    }
                }
            }
            return used;
        }
    }
}
