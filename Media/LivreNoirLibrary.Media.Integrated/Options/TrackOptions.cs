using System;
using System.Collections.Generic;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Media.Wave;

namespace LivreNoirLibrary.Media.Integrated
{
    public class TrackOptions : ObservableObjectBase, IOptions<TrackOptions>
    {
        public bool IsSystemTrack { get => field; set => SetValue(ref field, value); }
        public bool ApplyToBms { get => field; set => SetValue(ref field, value); }
        public SortedSet<int> SideChainSources
        {
            get => field;
            set
            {
                field.Clear();
                field.UnionWith(value);
                SendPropertyChanged();
            }
        } = [];
        public PackOptions? PackOptions { get => field; set => SetValue(ref field, value); }
        public SliceOptions? SliceOptions { get => field; set => SetValue(ref field, value); }

        public void Load(TrackOptions source)
        {
            IsSystemTrack = source.IsSystemTrack;
            ApplyToBms = source.ApplyToBms;
            SideChainSources = source.SideChainSources;
            LoadPackOptions(source.PackOptions);
            LoadSliceOptions(source.SliceOptions);
        }

        public bool LoadPackOptions(PackOptions? source)
        {
            bool flag;
            if (source is not null)
            {
                if (PackOptions is null)
                {
                    PackOptions = new();
                    PackOptions.Load(source);
                    flag = true;
                }
                else if (Json.Equals(PackOptions, source))
                {
                    flag = false;
                }
                else
                {
                    PackOptions.Load(source);
                    flag = true;
                }
            }
            else
            {
                flag = PackOptions is not null;
                PackOptions = null;
            }
            return flag;
        }

        public bool LoadSliceOptions(SliceOptions? source)
        {
            bool flag;
            if (source is not null)
            {
                if (SliceOptions is null)
                {
                    SliceOptions = new();
                    SliceOptions.Load(source);
                    flag = true;
                }
                else if (Json.Equals(SliceOptions, source))
                {
                    flag = false;
                }
                else
                {
                    SliceOptions.Load(source);
                    flag = true;
                }
            }
            else
            {
                flag = SliceOptions is not null;
                SliceOptions = null;
            }
            return flag;
        }

        public void SwapSideChain(int index1, int index2)
        {
            var sc = SideChainSources;
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
            foreach (var item in SideChainSources)
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
