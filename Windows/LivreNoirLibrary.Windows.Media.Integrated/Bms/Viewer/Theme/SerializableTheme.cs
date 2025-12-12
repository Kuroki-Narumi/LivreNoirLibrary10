using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public class SerializableTheme
    {
        [JsonPropertyName("common_colors")]
        public Dictionary<string, string>? CommonColors { get; set; }
        [JsonPropertyName("conductor")]
        public List<LaneInfoBundle>? ConductorLanes { get; set; }
        [JsonPropertyName("meta")]
        public List<LaneInfoBundle>? MetaLanes { get; set; }
        [JsonPropertyName("key")]
        public List<KeyLaneInfoBundle>? KeyLanes { get; set; }
        [JsonPropertyName("bgm")]
        public LaneInfo? BgmLane { get; set; }
        [JsonPropertyName("separator_width")]
        public int SeparatorWidth { get; set; }

        public SerializableTheme() { }
        public SerializableTheme(Theme source)
        {
            CommonColors = source.CommonColors.ToSerializable();
            ConductorLanes = [.. source.ConductorLanes];
            MetaLanes = [.. source.MetaLanes];
            KeyLanes = [.. source.KeyLanes];
            BgmLane = source.BgmLane;
            SeparatorWidth = source.SeparatorWidth;
        }
    }
}
