using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class LaneInfoBundle : ObservableObjectBase, IEnumerable<LaneInfo>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set => SetValue(ref field, value); }
        [JsonPropertyName("lanes")]
        public ObservableList<LaneInfo> Lanes { get; set => SetValue(ref field, value); } = [];

        public LaneInfoBundle() { }
        public LaneInfoBundle(string name, ReadOnlySpan<LaneInfo> lanes)
        {
            Name = name;
            Lanes.AddRange(lanes);
        }

        public void CopyFrom(LaneInfoBundle source)
        {
            Name = source.Name;
            Lanes.Clear();
            foreach (var item in source.Lanes)
            {
                Lanes.Add(item.Clone());
            }
        }

        public List<LaneInfo>.Enumerator GetEnumerator() => Lanes.GetEnumerator();
        IEnumerator<LaneInfo> IEnumerable<LaneInfo>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static LaneInfoBundle Conductor { get; } = new("Basic",
        [
            LaneInfo.CreateLane("Bpm", Channel.Bpm_Base, null, LaneInfo.DefaultMetaLaneWidth),
            LaneInfo.CreateLane("Stop", Channel.Stop, null, LaneInfo.DefaultMetaLaneWidth),
        ]);

        public static LaneInfoBundle Conductor_Ex { get; } = new("Extended",
        [
            LaneInfo.CreateLane("Bpm", Channel.Bpm_Base, null, LaneInfo.DefaultMetaLaneWidth),
            LaneInfo.CreateLane("Stop", Channel.Stop, null, LaneInfo.DefaultMetaLaneWidth),
            LaneInfo.CreateLane("Scroll", Channel.Scroll, null, LaneInfo.DefaultMetaLaneWidth),
            LaneInfo.CreateLane("Speed", Channel.Speed, null, LaneInfo.DefaultMetaLaneWidth),
        ]);

        public static LaneInfoBundle Bga_Standard { get; } = new("BGA",
        [
            LaneInfo.CreateLane("Bga", Channel.Bga_Base, "Bga"),
            LaneInfo.CreateLane("Layer", Channel.Bga_Layer1, "Bga"),
            LaneInfo.CreateLane("Poor", Channel.Bga_Poor, "Bga"),
        ]);

        public static LaneInfoBundle Bga_Extended { get; } = new("EX BGA",
        [
            LaneInfo.CreateLane("Base", Channel.Bga_Base, "Bga"),
            LaneInfo.CreateLane("L1", Channel.Bga_Layer1, "Bga"),
            LaneInfo.CreateLane("L2", Channel.Bga_Layer2, "Bga"),
            LaneInfo.CreateLane("Poor", Channel.Bga_Poor, "Bga"),
            LaneInfo.CreateLane("Key", Channel.SwBga, "Bga"),
        ]);
    }
}
