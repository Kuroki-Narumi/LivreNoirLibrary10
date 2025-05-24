using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static LivreNoirLibrary.Media.Bms.KeyIndexes;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed partial class BmsData : BaseData
    {
        private readonly List<FlowContainer> _flows = [];
        private readonly List<FlowData> _flow_data = [];
        private readonly SortedSet<int> _flow_free_index = [];
        private int _max_def = Constants.Base_Default * Constants.Base_Default;

        public ChartType ChartType { get; set; } = ChartType.Beat;
        public bool UseExtendedConductor { get; internal set; }
        public bool UseExtendedBga { get; internal set; }

        public override BmsData Root => this;
        public ReadOnlySpan<FlowContainer> Flows => CollectionsMarshal.AsSpan(_flows);
        public bool ContainsFlow => _flows.Count is > 0;

        public override int Base
        {
            get => Headers.GetNumber(HeaderType.Base, Constants.Base_Default);
            set
            {
                if (value is <= 0)
                {
                    value = Constants.Base_Default;
                }
                else
                {
                    value = Math.Clamp(value, BasedIndex.MinimumRadix, BasedIndex.MaximumRadix);
                }
                Headers.Set(HeaderType.Base, value);
                _max_def = value * value;
            }
        }

        public override int MaxDefIndex => _max_def;

        public override void Clear()
        {
            base.Clear();
            ClearFlow();
        }

        public void CheckIfGeneric()
        {
            foreach (var (_, note) in Timeline)
            {
                if (note.IsKey())
                {
                    if (note.Lane is (> Beat_1P_7 and < Beat_2P_1) or (> Beat_2P_7))
                    {
                        ChartType = ChartType.Generic;
                        return;
                    }
                }
            }
            ChartType = ChartType.Beat;
        }

        public HashSet<int> GetUsedKeyLanes()
        {
            HashSet<int> result = [];
            foreach (var (_, note) in Timeline)
            {
                if (note.IsKey())
                {
                    result.Add(note.Lane);
                }
            }
            return result;
        }

        public KeyType GetKeyType() => ChartType switch
        {
            ChartType.Beat => GetKeyType_Bms(),
            ChartType.Popn => GetKeyType_Pms(),
            _ => GetKeyType_Generic(),
        };

        public KeyType GetKeyType_Bms()
        {
            var like_7 = false;
            var like_10 = false;
            foreach (var (_, note) in Timeline)
            {
                if (note.IsKey())
                {
                    switch (note.Lane)
                    {
                        case >= Beat_2P_6:
                            return new(ChartType.Beat, 14);
                        case >= Beat_2P_1:
                            like_10 = true;
                            break;
                        case Beat_1P_6 or Beat_1P_7:
                            like_7 = true;
                            break;
                    }
                }
            }
            return new(ChartType.Beat, like_7 ? (like_10 ? 14 : 7) : (like_10 ? 10 : 5));
        }

        public KeyType GetKeyType_Pms()
        {
            var like_3 = false;
            var like_5 = false;
            var like_9 = false;
            foreach (var (_, note) in Timeline)
            {
                if (note.IsKey())
                {
                    switch (note.Lane)
                    {
                        case (>= Pop_1P_8 and <= Pop_1P_6) or >= Pop_2P_8:
                            return new(ChartType.Popn, 18);
                        case >= Pop_6:
                            like_9 = true;
                            break;
                        case >= Pop_4:
                            like_5 = true;
                            break;
                        case >= Pop_1 and <= Pop_3:
                            like_3 = true;
                            break;
                    }
                }
            }
            return new(ChartType.Popn, like_3 ? (like_5 ? 9 : 3) : (like_9 ? 9 : 5));
        }

        public KeyType GetKeyType_Generic()
        {
            foreach (var (_, note) in Timeline)
            {
                if (note.IsKey() && note.Lane is >= 25)
                {
                    return new(ChartType.Generic, 48);
                }
            }
            return new(ChartType.Generic, 24);
        }
    }
}
