using System;
using System.Collections.Generic;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.BM3
{
    public partial class TrackOptions : ObservableObjectBase
    {
        [ObservableProperty]
        private bool _isSystemTrack = false;
        [ObservableProperty]
        private bool _applyToBms = true;
        [ObservableProperty]
        private SortedSet<int> _sideChainSources = [];
        [ObservableProperty]
        private PackOptions? _packOptions;
        [ObservableProperty]
        private SliceOptions? _sliceOptions;

        public void Load(TrackOptions source)
        {
            IsSystemTrack = source._isSystemTrack;
            ApplyToBms = source._applyToBms;
            LoadPackOptions(source._packOptions);
            SideChainSources = [.. source._sideChainSources];
            LoadSliceOptions(source._sliceOptions);
        }

        public bool LoadPackOptions(PackOptions? source)
        {
            bool flag;
            if (source is not null)
            {
                if (_packOptions is null)
                {
                    PackOptions = new();
                    _packOptions!.Load(source);
                    flag = true;
                }
                else if (Json.Equals(_packOptions, source))
                {
                    flag = false;
                }
                else
                {
                    _packOptions.Load(source);
                    flag = true;
                }
            }
            else
            {
                flag = _packOptions is not null;
                PackOptions = null;
            }
            return flag;
        }

        public bool LoadSliceOptions(SliceOptions? source)
        {
            bool flag;
            if (source is not null)
            {
                if (_sliceOptions is null)
                {
                    SliceOptions = new();
                    _sliceOptions!.Load(source);
                    flag = true;
                }
                else if (Json.Equals(_sliceOptions, source))
                {
                    flag = false;
                }
                else
                {
                    _sliceOptions.Load(source);
                    flag = true;
                }
            }
            else
            {
                flag = _sliceOptions is not null;
                SliceOptions = null;
            }
            return flag;
        }

        public void SwapSideChain(int index1, int index2)
        {
            var sc = _sideChainSources;
            var flag1 = sc.Remove(index1);
            var flag2 = sc.Remove(index2);
            if (flag1)
            {
                sc.Add(index2);
            }
            if (flag2)
            {
                sc.Add(index1);
            }
        }

        public void RemoveSideChain(int index)
        {
            SortedSet<int> sc = [];
            foreach (var item in _sideChainSources)
            {
                if (item < index)
                {
                    sc.Add(item);
                }
                else if (item > index)
                {
                    sc.Add(item - 1);
                }
            }
            SideChainSources = sc;
        }
    }
}
