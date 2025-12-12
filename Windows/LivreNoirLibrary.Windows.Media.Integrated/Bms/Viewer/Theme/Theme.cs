using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public class Theme : ObservableObjectBase
    {
        public const int DefaultSeparatorWidth = 6;

        private static readonly LaneInfoBundle[] _default_conductor = [LaneInfoBundle.Conductor_Ex, LaneInfoBundle.Conductor];
        private static readonly LaneInfoBundle[] _default_meta = [LaneInfoBundle.Bga_Standard, LaneInfoBundle.Bga_Extended];
        private static readonly KeyLaneInfoBundle[] _default_key = [
                KeyLaneInfoBundle.Beat_7k,
                KeyLaneInfoBundle.Beat_14k,
                KeyLaneInfoBundle.Beat_5k,
                KeyLaneInfoBundle.Beat_10k,
                KeyLaneInfoBundle.Popn_9k,
                KeyLaneInfoBundle.Popn_18k,
                KeyLaneInfoBundle.Generic_24k,
                KeyLaneInfoBundle.Generic_48k,
            ];

        public CommonColors CommonColors { get; } = new();
        public ObservableList<LaneInfoBundle> ConductorLanes { get; } = [];
        public ObservableList<LaneInfoBundle> MetaLanes { get; } = [];
        public ObservableList<KeyLaneInfoBundle> KeyLanes { get; } = [];
        public LaneInfo BgmLane { get; set => SetValue(ref field, value); }
        public int SeparatorWidth { get; set => SetValue(ref field, Math.Max(value, 0)); }

        public Theme()
        {
            BgmLane = LaneInfo.Bgm_Default.Clone();
        }

        public Theme(SerializableTheme source)
        {
            CommonColors.Load(source.CommonColors);
            if (source.ConductorLanes is { } list1)
            {
                ConductorLanes.AddRange(list1);
            }
            if (source.MetaLanes is { } list2)
            {
                MetaLanes.AddRange(list2);
            }
            if (source.KeyLanes is { } list3)
            {
                KeyLanes.AddRange(list3);
            }
            BgmLane = source.BgmLane ?? LaneInfo.Bgm_Default.Clone();
            SeparatorWidth = source.SeparatorWidth;
        }

        public void LoadDefault()
        {
            CommonColors.ClearValues();
            ConductorLanes.ClearWithoutNotify();
            ConductorLanes.AddRange(_default_conductor);
            MetaLanes.ClearWithoutNotify();
            MetaLanes.AddRange(_default_meta);
            KeyLanes.ClearWithoutNotify();
            KeyLanes.AddRange(_default_key);
            BgmLane = LaneInfo.Bgm_Default.Clone();
            SeparatorWidth = DefaultSeparatorWidth;
        }

        public void CopyFrom(Theme source)
        {
            CommonColors.Load(source.CommonColors);
            ConductorLanes.ClearWithoutNotify();
            ConductorLanes.AddRange(source.ConductorLanes);
            MetaLanes.ClearWithoutNotify();
            MetaLanes.AddRange(source.MetaLanes);
            KeyLanes.ClearWithoutNotify();
            KeyLanes.AddRange(source.KeyLanes);
            BgmLane = source.BgmLane.Clone();
            SeparatorWidth = source.SeparatorWidth;
        }

        public SerializableTheme ToSerializable() => new(this);

        public string GetJsonText(bool pretty = true) => ToSerializable().GetJsonText(pretty);

        public static bool TryLoadJson(string jsonText, [MaybeNullWhen(false)] out Theme theme)
        {
            if (Json.TryParse<SerializableTheme>(jsonText, out var data))
            {
                theme = new(data);
                return true;
            }
            theme = null;
            return false;
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
        public Binding GetBinding_WaveForm() => GetBindingCore(nameof(CommonColors.WaveForm));
    }
}
