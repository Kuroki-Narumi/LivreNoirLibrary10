using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BmsData
    {
        public DefSortResult DefSort(DefSortOptions options)
        {
            var lnobj = (short)LnObj;
            var radix = Base;
            var used = GetUsedDefList();
            var maps = ProcessRemoveUnused(used, options.RemoveUnusedDef, options.RemoveMultiDef);
            if (options.Sort)
            {
                var headroom = options.Headroom;
                if (headroom is <= 0)
                {
                    headroom = 1;
                }
                var sortByName = options.SortByName;
                var fixLnEnd = options.FixLnEnd;
                HashSet<short> empty = [];
                foreach (var (type, list) in DefLists)
                {
                    var map = list.GetSortedMap(used.TryGetValue(type, out var u) ? u : empty, type is DefType.Wav && fixLnEnd ? [lnobj] : empty, headroom, sortByName);
                    if (map.IsEffective())
                    {
                        DefMap(type, map);
                        maps.GetOrAdd(type).Product(map);
                    }
                }
            }
            var baseChanged = false;
            var maxUsedCount = Math.Max(DefLists.MaxIndex, lnobj);
            var newRadix = radix;
            switch (options.TargetRadix)
            {
                case TargetRadixType.FF:
                    if (radix is not Constants.Base_Legacy && maxUsedCount is < Constants.DefMax_Legacy)
                    {
                        Base = newRadix = Constants.Base_Legacy;
                        baseChanged = true;
                    }
                    break;
                case TargetRadixType.ZZ:
                    if (radix is not Constants.Base_Default && maxUsedCount is < Constants.DefMax_Default)
                    {
                        Base = newRadix = Constants.Base_Default;
                        baseChanged = true;
                    }
                    break;
                case TargetRadixType.zz:
                    if (radix is not Constants.Base_Extended)
                    {
                        Base = newRadix = Constants.Base_Extended;
                        baseChanged = true;
                    }
                    break;
            }
            if (baseChanged)
            {
                LnObj = lnobj;
            }
            return new(maps, radix, newRadix);
        }

        private DefIndexMapCollection ProcessRemoveUnused(DefIndexCollection used, bool removeUnused, bool removeDuplicated)
        {
            var lnobj = LnObj;
            var includeLnEnd = lnobj is 0;
            if (!includeLnEnd)
            {
                used.Add(DefType.Wav, lnobj);
            }
            HashSet<short> removed = [];
            DefIndexMapCollection maps = [];
            var wavMap = maps.GetOrAdd(DefType.Wav);
            Dictionary<string, short> duplicated = [];
            foreach (var data in EachData())
            {
                var defLists = data.DefLists;
                if (removeDuplicated && defLists.TryGetValue(DefType.Wav, out var wavDef))
                {
                    foreach (var (_, note) in data.Timeline)
                    {
                        if (note.IsWavObject(includeLnEnd))
                        {
                            var id = (short)note.Id;
                            if (wavDef.TryGetValue(id, out var value))
                            {
                                if (duplicated.TryGetValue(value, out var baseId))
                                {
                                    if (id != baseId)
                                    {
                                        note.Id = baseId;
                                        wavMap.Set(id, baseId);
                                        wavDef.Remove(id);
                                        removed.Add(id);
                                    }
                                }
                                else
                                {
                                    duplicated.Add(value, id);
                                }
                            }
                        }
                    }
                }
                if (removeUnused)
                {
                    defLists.RemoveUnused(maps, used);
                }
            }
            if (used.TryGetValue(DefType.Wav, out var s))
            {
                s.ExceptWith(removed);
            }
            return maps;
        }

        public void RenameDef(string directory)
        {
            var radix = Base;
            void ProcessRename(DefList list, Func<string?, string[]> func)
            {
                var keys = list.Keys.ToArray();
                foreach (var key in keys)
                {
                    var value = list[key];
                    var baseName = Path.GetFileNameWithoutExtension(value);
                    var newName = BasedIndex.ToBased(key, radix, 2);
                    list[key] = value.Replace(baseName, newName);
                    var paths = func(Path.GetFullPath(value, directory));
                    foreach (var path in paths)
                    {
                        var rel = Path.GetRelativePath(directory, path);
                        File.Move(path, Path.GetFullPath(rel.Replace(baseName, newName), directory));
                    }
                }
            }
            if (DefLists.TryGetValue(DefType.Wav, out var list))
            {
                ProcessRename(list, FileUtils.GetAllAudioFilenames);
            }
            if (DefLists.TryGetValue(DefType.Bmp, out list))
            {
                ProcessRename(list, FileUtils.GetAllMediaFilenames);
            }
        }
    }
}
