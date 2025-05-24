using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Files;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BaseData
    {
        public bool TryGetWavePath(int index, string root, [MaybeNullWhen(false)]out string name, [MaybeNullWhen(false)]out string path)
        {
            name = DefLists.GetInherited(DefType.Wav, index);
            if (name is not null && FileUtils.TryGetAudioFilename(Path.GetFullPath(name, root), out path))
            {
                return true;
            }
            path = null;
            return false;
        }

        public bool TryGetVideoPath(int index, string root, [MaybeNullWhen(false)]out string name, [MaybeNullWhen(false)]out string path)
        {
            name = DefLists.GetInherited(DefType.Bmp, index);
            if (name is not null && FileUtils.TryGetVideoFilename(Path.GetFullPath(name, root), out path))
            {
                return true;
            }
            path = null;
            return false;
        }

        public bool TryGetImagePath(int index, string root, [MaybeNullWhen(false)]out string name, [MaybeNullWhen(false)]out string path)
        {
            name = DefLists.GetInherited(DefType.Bmp, index);
            if (name is not null && FileUtils.TryGetImageFilename(Path.GetFullPath(name, root), out path))
            {
                return true;
            }
            path = null;
            return false;
        }

        public void DefMoveDown(DefType type, List<int> indexes)
        {
            if (indexes.Count is 0)
            {
                return;
            }
            if (indexes.Count is 1)
            {
                var i = indexes[0];
                if ((uint)i < (uint)MaxDefIndex)
                {
                    DefSwap(type, i, i + 1);
                }
            }
            else
            {
                indexes.Sort(); 
                DefIndexMap map = new();
                var limit = (uint)MaxDefIndex;
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
                DefMap(type, map);
            }
        }

        public void DefMoveUp(DefType type, List<int> indexes)
        {
            if (indexes.Count is 0)
            {
                return;
            }
            if (indexes.Count is 1)
            {
                var i = indexes[0];
                if (i is > 1)
                {
                    DefSwap(type, i, i - 1);
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
                DefMap(type, map);
            }
        }

        public void DefSwap(DefType type, int index1, int index2)
        {
            foreach (var data in Root.EachData())
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
        }

        private static void DefSwapCore(BaseData data, DefType type, int index1, int index2)
        {
            if (data.DefLists.TryGetValue(type, out var list))
            {
                list.Swap(index1, index2);
            }
        }

        private static void DefSwap_Key(BaseData data, int index1, int index2)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note.IsWavObject())
                {
                    var id = note.Id;
                    if (id == index1)
                    {
                        note.Id = index2;
                    }
                    else if (id == index2)
                    {
                        note.Id = index1;
                    }
                }
            }
        }

        private static void DefSwap_Meta(BaseData data, int index1, int index2, Channel channel)
        {
            var lane = channel.GetLane();
            foreach (var (_, note) in data.Timeline)
            {
                if (note.Lane == lane)
                {
                    var id = note.Id;
                    if (id == index1)
                    {
                        note.Id = index2;
                    }
                    else if (id == index2)
                    {
                        note.Id = index1;
                    }
                }
            }
        }

        private static void DefSwap_Meta(BaseData data, int index1, int index2, params ReadOnlySpan<Channel> channels)
        {
            HashSet<int> targets = [];
            foreach (var c in channels)
            {
                targets.Add(c.GetLane());
            }
            foreach (var (_, note) in data.Timeline)
            {
                if (targets.Contains(note.Lane))
                {
                    var id = note.Id;
                    if (id == index1)
                    {
                        note.Id = index2;
                    }
                    else if (id == index2)
                    {
                        note.Id = index1;
                    }
                }
            }
        }

        public void DefMap(DefType type, DefIndexMap map)
        {
            foreach (var data in Root.EachData())
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
        }

        private static void DefMapCore(BaseData data, DefType type, DefIndexMap map)
        {
            if (data.DefLists.TryGetValue(type, out var list))
            {
                list.Map(map);
            }
        }

        private static void DefMap_Key(BaseData data, DefIndexMap map)
        {
            foreach (var (_, note) in data.Timeline)
            {
                if (note.IsWavObject())
                {
                    note.Id = map[note.Id];
                }
            }
        }

        private static void DefMap_Meta(BaseData data, DefIndexMap map, Channel channel)
        {
            var lane = BmsUtils.GetLane(channel);
            foreach (var (_, note) in data.Timeline)
            {
                if (note.Lane == lane)
                {
                    note.Id = map[note.Id];
                }
            }
        }

        private static void DefMap_Meta(BaseData data, DefIndexMap map, params ReadOnlySpan<Channel> channels)
        {
            HashSet<int> targets = [];
            foreach (var c in channels)
            {
                targets.Add(c.GetLane());
            }
            foreach (var (_, note) in data.Timeline)
            {
                if (targets.Contains(note.Lane))
                {
                    note.Id = map[note.Id];
                }
            }
        }

        internal void RemoveDefWithBasenameInternal(DefType type, string basename)
        {
            List<int> removeIds = [];
            var max = MaxDefIndex;
            for (var i = 1; i < max; i++)
            {
                if (DefLists.GetInherited(type, i) is string value && value.StartsWith(basename))
                {
                    removeIds.Add(i);
                    DefLists.Remove(type, i);
                }
            }
            Timeline.RemoveIf((_, n) => !n.IsMine() && BmsUtils.GetDefType(n.Lane) == type && removeIds.BinarySearch(n.Id) is >= 0);
        }
    }
}
