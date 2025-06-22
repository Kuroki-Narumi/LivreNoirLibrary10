using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class LaneInfoBundle : ObservableObjectBase
    {
        [JsonPropertyName("name")]
        [ObservableProperty]
        private string? _name = "";
        [JsonPropertyName("lanes")]
        [ObservableProperty]
        private ObservableList<LaneInfo> _lanes = [];

        public LaneInfoBundle() { }

        public LaneInfoBundle(string name, IEnumerable<LaneInfo> lanes)
        {
            _name = name;
            _lanes = [.. lanes];
        }

        public LaneInfoBundle(LaneInfoBundle source) => Load(source);

        public virtual void Load(LaneInfoBundle source)
        {
            _lanes.Clear();
            Name = source._name;
            foreach (var item in source._lanes)
            {
                _lanes.Add(item.Clone());
            }
        }

        public static LaneInfoBundle Conductor { get; } = new("Basic",
        [
            LaneInfo.GetStatic("Bpm", Channel.Bpm_Base, "Bpm", LaneInfo.DefaultMetaLaneWidth),
            LaneInfo.GetStatic("Stop", Channel.Stop, "Stop", LaneInfo.DefaultMetaLaneWidth),
        ]);

        public static LaneInfoBundle Conductor_Ex { get; } = new("Extended",
        [
            LaneInfo.GetStatic("Bpm", Channel.Bpm_Base, "Bpm", LaneInfo.DefaultMetaLaneWidth),
            LaneInfo.GetStatic("Stop", Channel.Stop, "Stop", LaneInfo.DefaultMetaLaneWidth),
            LaneInfo.GetStatic("Scroll", Channel.Scroll, "Scroll", LaneInfo.DefaultMetaLaneWidth),
            LaneInfo.GetStatic("Speed", Channel.Speed, "Speed", LaneInfo.DefaultMetaLaneWidth),
        ]);

        public static LaneInfoBundle Bga_Standard { get; } = new("BGA",
        [
            LaneInfo.GetStatic("Bga", Channel.Bga_Base, "Bga"),
            LaneInfo.GetStatic("Layer", Channel.Bga_Layer1, "Bga"),
            LaneInfo.GetStatic("Poor", Channel.Bga_Poor, "Bga"),
        ]);

        public static LaneInfoBundle Bga_Extended { get; } = new ("EX BGA",
        [
            LaneInfo.GetStatic("Base", Channel.Bga_Base, "Bga"),
            LaneInfo.GetStatic("L1", Channel.Bga_Layer1, "Bga"),
            LaneInfo.GetStatic("L2", Channel.Bga_Layer2, "Bga"),
            LaneInfo.GetStatic("Poor", Channel.Bga_Poor, "Bga"),
            LaneInfo.GetStatic("Key", Channel.SwBga, "Bga"),
        ]);
    }
}
