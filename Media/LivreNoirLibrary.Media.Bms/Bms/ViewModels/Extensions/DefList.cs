using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsExtensions
    {
        private delegate bool TryGetFileName(string fileName, out string actualPath);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetPathCore(IBmsViewModel vm, DefType type, int key, string root, TryGetFileName func, [MaybeNullWhen(false)] out string name, [MaybeNullWhen(false)] out string path)
        {
            if (vm.GetDefValue(type, key) is { } n && func(Path.GetFullPath(n, root), out path))
            {
                name = n;
                return true;
            }
            name = path = null;
            return false;
        }

        extension (IBmsViewModel vm)
        {
            public bool IsDefined(DefType type, int index)
            {
                if (vm.CurrentData.DefLists.ContainsKey(type, index))
                {
                    return true;
                }
                foreach (var data in vm.EnumerateParents())
                {
                    if (data.DefLists.ContainsKey(type, index))
                    {
                        return true;
                    }
                }
                return false;
            }

            public string? GetDefValue(DefType type, int key, bool containsCurrent = true)
                => vm.GetInheritedValue<string>((data, out value) => data.DefLists.TryGetValue(type, key, out value!), null, containsCurrent);

            public bool TryGetDefKey(DefType type, string value, out int key)
                => vm.TryGetInheritedValue((data, out key) => data.DefLists.TryGetKey(type, value, out key), out key);

            public void SetDefValue(DefType type, int key, string? value)
            {
                if (vm.CurrentData.DefLists.Set(type, key, value))
                {
                    vm.OnModified();
                }
            }

            public bool TryGetWavePath(int index, string root, [MaybeNullWhen(false)] out string defValue, [MaybeNullWhen(false)] out string path) 
                => TryGetPathCore(vm, DefType.Wav, index, root, FileUtils.TryGetAudioFileName!, out defValue, out path);

            public bool TryGetImagePath(int index, string root, [MaybeNullWhen(false)] out string defValue, [MaybeNullWhen(false)] out string path) 
                => TryGetPathCore(vm, DefType.Bmp, index, root, FileUtils.TryGetImageFileName!, out defValue, out path);

            public bool TryGetVideoPath(int index, string root, [MaybeNullWhen(false)] out string defValue, [MaybeNullWhen(false)] out string path) 
                => TryGetPathCore(vm, DefType.Bmp, index, root, FileUtils.TryGetVideoFileName!, out defValue, out path);

            public bool TryGetMediaPath(int index, string root, [MaybeNullWhen(false)] out string defValue, [MaybeNullWhen(false)] out string path) 
                => TryGetPathCore(vm, DefType.Bmp, index, root, FileUtils.TryGetMediaFileName!, out defValue, out path);

            public int FindFreeDefIndex(DefType type, int start = 1)
            {
                for (; IsDefined(vm, type, start); start++) ;
                return start;
            }

            public void DefMoveDown(DefType type, List<int> indexes, DefIndexMap? mapCache = null)
            {
                switch (indexes.Count)
                {
                    case 0:
                        return;
                    case 1:
                        var i = indexes[0];
                        if ((uint)i < BmsConstants.DefMax_Extended)
                        {
                            DefSwap(vm, type, i, i + 1);
                        }
                        return;
                    default:
                        indexes.Sort();
                        var map = mapCache ?? new();
                        foreach (var index in indexes.AsSpan())
                        {
                            var next = index + 1;
                            if ((uint)next >= BmsConstants.DefMax_Extended) { continue; }
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
                        DefMap(vm, type, map);
                        return;
                }
            }

            public void DefMoveUp(DefType type, List<int> indexes, DefIndexMap? mapCache = null)
            {
                switch (indexes.Count)
                {
                    case 0:
                        return;
                    case 1:
                        var index = indexes[0];
                        if ((uint)index < BmsConstants.DefMax_Extended)
                        {
                            DefSwap(vm, type, index, index - 1);
                        }
                        return;
                    default:
                        indexes.Sort();
                        var map = mapCache ?? new();
                        for (var i = indexes.Count - 1; i is >= 0; i--)
                        {
                            index = indexes[i];
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
                        DefMap(vm, type, map);
                        return;
                }
            }

            public void DefSwap(DefType type, int index1, int index2)
            {
                if (index1 == index2)
                {
                    return;
                }
                foreach (var (_, data) in vm.Root.EnumerateAllData())
                {
                    DefSwapCore(data, type, index1, index2);
                    switch (type)
                    {
                        case DefType.Wav:
                            DefSwap_Key(data, index1, index2);
                            break;
                        case DefType.Bmp:
                            DefSwap_Meta(data, index1, index2, Channel.Bga_Base, Channel.Bga_Layer1, Channel.Bga_Layer2, Channel.Bga_Poor);
                            break;
                        case DefType.ExRank:
                            DefSwap_Meta(data, index1, index2, Channel.ExRank);
                            break;
                        case DefType.Text:
                            DefSwap_Meta(data, index1, index2, Channel.Text);
                            break;
                        case DefType.Argb:
                            DefSwap_Meta(data, index1, index2, Channel.Argb_Base, Channel.Argb_Layer1, Channel.Argb_Layer2, Channel.Argb_Poor);
                            break;
                        case DefType.SwBga:
                            DefSwap_Meta(data, index1, index2, Channel.SwBga);
                            break;
                        case DefType.ChangeOption:
                            DefSwap_Meta(data, index1, index2, Channel.ChangeOption);
                            break;
                    }
                }
                if (vm.LnObj == index1)
                {
                    vm.LnObj = index2;
                }
                else if (vm.LnObj == index2)
                {
                    vm.LnObj = index1;
                }
                vm.OnModified();
            }

            public void DefMap(DefType type, DefIndexMap map)
            {
                if (!map.IsEffective)
                {
                    return;
                }
                foreach (var (_, data) in vm.Root.EnumerateAllData())
                {
                    DefMapCore(data, type, map);
                    switch (type)
                    {
                        case DefType.Wav:
                            DefMap_Key(data, map);
                            break;
                        case DefType.Bmp:
                            DefMap_Meta(data, map, Channel.Bga_Base, Channel.Bga_Layer1, Channel.Bga_Layer2, Channel.Bga_Poor);
                            break;
                        case DefType.ExRank:
                            DefMap_Meta(data, map, Channel.ExRank);
                            break;
                        case DefType.Text:
                            DefMap_Meta(data, map, Channel.Text);
                            break;
                        case DefType.Argb:
                            DefMap_Meta(data, map, Channel.Argb_Base, Channel.Argb_Layer1, Channel.Argb_Layer2, Channel.Argb_Poor);
                            break;
                        case DefType.SwBga:
                            DefMap_Meta(data, map, Channel.SwBga);
                            break;
                        case DefType.ChangeOption:
                            DefMap_Meta(data, map, Channel.ChangeOption);
                            break;
                    }
                }
                vm.LnObj = map[vm.LnObj];
                vm.OnModified();
            }
        }


        private static void DefSwapCore(IBmsDataUnit data, DefType type, int index1, int index2)
        {
            if (data.DefLists.TryGetList(type, out var list))
            {
                list.Swap((short)index1, (short)index2);
            }
        }

        private static void DefSwap_Key(IBmsDataUnit data, int index1, int index2)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note.IsSound())
                {
                    var value = (int)note.Value;
                    if (value == index1)
                    {
                        note.Value = index2;
                    }
                    else if (value == index2)
                    {
                        note.Value = index1;
                    }
                }
            }
        }

        private static void DefSwap_Meta(IBmsDataUnit data, int index1, int index2, Channel channel)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note.Channel == channel)
                {
                    var value = (int)note.Value;
                    if (value == index1)
                    {
                        note.Value = index2;
                    }
                    else if (value == index2)
                    {
                        note.Value = index1;
                    }
                }
            }
        }

        private static void DefSwap_Meta(IBmsDataUnit data, int index1, int index2, params ReadOnlySpan<Channel> channels)
        {
            var span = MemoryMarshal.Cast<Channel, short>(channels);
            foreach (var (_, note) in data.Timeline)
            {
                if (span.Contains((short)note.Channel))
                {
                    var value = (int)note.Value;
                    if (value == index1)
                    {
                        note.Value = index2;
                    }
                    else if (value == index2)
                    {
                        note.Value = index1;
                    }
                }
            }
        }

        private static void DefMapCore(IBmsDataUnit data, DefType type, DefIndexMap map)
        {
            if (data.DefLists.TryGetList(type, out var list))
            {
                list.Map(map);
            }
        }

        private static void DefMap_Key(IBmsDataUnit data, DefIndexMap map)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note.IsSound())
                {
                    note.Value = map[(int)note.Value];
                }
            }
        }

        private static void DefMap_Meta(IBmsDataUnit data, DefIndexMap map, Channel channel)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note.Channel == channel)
                {
                    note.Value = map[(int)note.Value];
                }
            }
        }

        private static void DefMap_Meta(IBmsDataUnit data, DefIndexMap map, params ReadOnlySpan<Channel> channels)
        {
            var span = MemoryMarshal.Cast<Channel, short>(channels);
            foreach (var (_, note) in data.Timeline)
            {
                if (span.Contains((short)note.Channel))
                {
                    note.Value = map[(int)note.Value];
                }
            }
        }

        public static void RemoveDefWithBasename(this IBmsData data, DefType type, string basename)
        {
            DefIndexMap map = [];
            foreach (var (_, d) in data.EnumerateAllData())
            {
                if (d.DefLists.TryGetList(type, out var list))
                {
                    map.Clear();
                    list.RemoveWithBasename(basename, map);
                    d.Timeline.RemoveAll((_, n) => n.TryGetDefType(out var t) && t == type && map.IsRemoved((short)n.Value));
                }
            }
        }
    }
}
