using System;
using System.Collections.Generic;
using LivreNoirLibrary.ObjectModel;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsViewModel
    {
        private static readonly DefType[] _types = [DefType.Wav, DefType.Bmp, DefType.ExRank, DefType.Text, DefType.Argb, DefType.SwBga, DefType.ChangeOption];

        private readonly Dictionary<DefType, DefListViewModel> _defLists = CreateDefLists();
        private static Dictionary<DefType, DefListViewModel> CreateDefLists()
        {
            Dictionary<DefType, DefListViewModel> items = [];
            foreach (var type in _types)
            {
                items.Add(type, new(type));
            }
            return items;
        }

        public DefListViewModel WavDefs => _defLists[DefType.Wav];
        public DefListViewModel BmpDefs => _defLists[DefType.Bmp];
        public DefListViewModel ExRankDefs => _defLists[DefType.ExRank];
        public DefListViewModel TextDefs => _defLists[DefType.Text];
        public DefListViewModel ArgbDefs => _defLists[DefType.Argb];
        public DefListViewModel SwBgaDefs => _defLists[DefType.SwBga];
        public DefListViewModel ChangeOptionDefs => _defLists[DefType.ChangeOption];

        private void RefreshDefList(BaseData source)
        {
            var radix = source.Base;
            var defs = source.DefLists;
            foreach (var (type, items) in _defLists)
            {
                items.Update(defs, type, radix);
            }
        }

        private void RefreshDefList(BaseData source, DefType type)
        {
            if (_defLists.TryGetValue(type, out var items))
            {
                items.Update(source.DefLists, type, source.Base);
            }
        }

        public bool SetDef(DefType type, int index, string? value)
        {
            if (_currentData.DefLists.Set(type, index, value))
            {
                _defLists[type].Update(index, value);
                this.OnEdit(true);
                return true;
            }
            return false;
        }

        public IEnumerable<DefListItem> SetDefs(DefType type, IEnumerable<(int, string?)> values)
        {
            var modified = false;
            var source = _currentData.DefLists;
            if (_defLists.TryGetValue(type, out var defs))
            {
                foreach (var (index, value) in values)
                {
                    if (source.Set(type, index, value))
                    {
                        modified = true;
                        var item = defs.Update(index, value);
                        yield return item;
                    }
                }
            }
            this.OnEdit(modified);
        }

        public bool ClearDefs(DefType type, List<int> indexes)
        {
            var modified = false;
            var target = _currentData.DefLists;
            if (_defLists.TryGetValue(type, out var defs))
            {
                foreach (var index in CollectionsMarshal.AsSpan(indexes))
                {
                    if (target.Remove(type, index))
                    {
                        defs.Update(index, null);
                        modified = true;
                    }
                }
            }
            this.OnEdit(modified);
            return modified;
        }

        public bool MoveDownDefs(DefType type, List<int> indexes)
        {
            if (_currentData!.DefMoveDown(type, indexes))
            {
                this.OnEdit(true);
                RefreshDefList(_currentData, type);
                return true;
            }
            return false;
        }

        public bool MoveUpDefs(DefType type, List<int> indexes)
        {
            if (_currentData!.DefMoveUp(type, indexes))
            {
                this.OnEdit(true);
                RefreshDefList(_currentData, type);
                return true;
            }
            return false;
        }

        public DefSortResult DefSort(DefSortOptions options)
        {
            var result = _root.DefSort(options);
            this.OnEdit(result.Count is > 0);
            OnCurrentDataChanged(_currentData!);
            return result;
        }
    }
}
