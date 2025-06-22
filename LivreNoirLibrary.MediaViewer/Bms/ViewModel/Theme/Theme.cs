using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Data;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    [JsonConverter(typeof(ThemeJsonConverter))]
    public partial class Theme : ObservableObjectBase
    {
        private static readonly LaneInfoBundle[] _default_conductor = [LaneInfoBundle.Conductor_Ex, LaneInfoBundle.Conductor];
        private static readonly LaneInfoBundle[] _default_meta = [LaneInfoBundle.Bga_Standard, LaneInfoBundle.Bga_Extended];
        private static readonly KeyLaneInfoBundle[] _default_key = [
                KeyLaneInfoBundle.Beat_7k,
                KeyLaneInfoBundle.Beat_14k,
                KeyLaneInfoBundle.Beat_5k,
                KeyLaneInfoBundle.Beat_10k,
                KeyLaneInfoBundle.Pop_9k,
                KeyLaneInfoBundle.Pop_18k,
                KeyLaneInfoBundle.Generic_24k,
                KeyLaneInfoBundle.Generic_48k,
            ];
        private static readonly LaneOrderType[] _default_lane_order = [LaneOrderType.Meta, LaneOrderType.Key, LaneOrderType.Bgm];
        public static ReadOnlySpan<LaneOrderType> DefaultLaneOrder => _default_lane_order;

        internal readonly CommonColors _commonColors = new();
        internal readonly ObservableList<LaneInfoBundle> _conductor = [];
        internal readonly ObservableList<LaneInfoBundle> _meta = [];
        internal readonly ObservableList<KeyLaneInfoBundle> _key = [];
        [ObservableProperty]
        internal LaneInfo _bgmLane = LaneInfo.Bgm.Clone();
        [ObservableProperty]
        internal int _separatorWidth = LaneInfo.SeparatorWidth;
        internal readonly ObservableList<LaneOrderType> _laneOrder = [.. _default_lane_order];

        public CommonColors CommonColors => _commonColors;
        public ObservableList<LaneInfoBundle> ConductorLanes => _conductor;
        public ObservableList<LaneInfoBundle> MetaLanes => _meta;
        public ObservableList<KeyLaneInfoBundle> KeyLanes => _key;
        public ObservableList<LaneOrderType> LaneOrder => _laneOrder;

        private static int CoerceSeparatorWidth(int value) => Math.Max(value, 0);

        public Theme() { }
        public Theme(SerializableTheme source)
        {
            _commonColors.Load(source.CommonColors);
            _conductor.Load(source.ConductorLanes);
            _meta.Load(source.MetaLanes);
            _key.Load(source.KeyLanes);
            _bgmLane.Load(source.BgmLane);
            SeparatorWidth = source.SeparatorWidth;
        }

        public void LoadDefault()
        {
            _commonColors.SetDefault();
            _conductor.Load(_default_conductor);
            _meta.Load(_default_meta);
            _key.Load(_default_key);
            _bgmLane.Load(LaneInfo.Bgm);
            SeparatorWidth = LaneInfo.SeparatorWidth;
            _laneOrder.Load(_default_lane_order);
        }

        public void Load(Theme source)
        {
            _commonColors.Load(source.CommonColors);
            _conductor.Load(source._conductor.Select(i => new LaneInfoBundle(i)));
            _meta.Load(source._meta.Select(i => new LaneInfoBundle(i)));
            _key.Load(source._key.Select(i => new KeyLaneInfoBundle(i)));
            BgmLane = source._bgmLane.Clone();
            _laneOrder.Load(source._laneOrder);
            SeparatorWidth = source._separatorWidth;
        }

        public List<LaneInfo> CreateBgmList(int min)
        {
            List<LaneInfo> list = [];
            for (int i = 0; i < min; i++)
            {
                var lane = BgmLane.Clone();
                lane.Name = $"{BgmLane.Name}{i + 1:D2}";
                lane.Lane = -i;
                list.Add(lane);
            }
            return list;
        }

        private Binding GetBindingCore(string propName) => new(propName) { Source = CommonColors };
        public Binding GetBinding_HeaderText() => GetBindingCore(nameof(CommonColors.HeaderText));
        public Binding GetBinding_BarLine() => GetBindingCore(nameof(CommonColors.Bar));
        public Binding GetBinding_BeatLine() => GetBindingCore(nameof(CommonColors.Beat));
        public Binding GetBinding_SubBeatLine() => GetBindingCore(nameof(CommonColors.SubBeat));
        public Binding GetBinding_LaneBorder() => GetBindingCore(nameof(CommonColors.LaneBorder));
        public Binding GetBinding_Mine() => GetBindingCore(nameof(CommonColors.Mine));
        public Binding GetBinding_LongEnd() => GetBindingCore(nameof(CommonColors.LongEnd));
        public Binding GetBinding_Selected() => GetBindingCore(nameof(CommonColors.Selected));
        public Binding GetBinding_SelectedLong() => GetBindingCore(nameof(CommonColors.SelectedLong));
    }
}
