using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class SerializableTheme
    {
        [JsonPropertyName(ThemeJsonConverter.Prop_CommonColors)]
        public Dictionary<string, string>? CommonColors { get; set; }
        [JsonPropertyName(ThemeJsonConverter.Prop_Conductor)]
        public List<LaneInfoBundle>? ConductorLanes { get; set; }
        [JsonPropertyName(ThemeJsonConverter.Prop_Meta)]
        public List<LaneInfoBundle>? MetaLanes { get; set; }
        [JsonPropertyName(ThemeJsonConverter.Prop_Key)]
        public List<KeyLaneInfoBundle>? KeyLanes { get; set; }
        [JsonPropertyName(ThemeJsonConverter.Prop_Bgm)]
        public LaneInfo? BgmLane { get; set; }
        [JsonPropertyName(ThemeJsonConverter.Prop_SeparatorWidth)]
        public int SeparatorWidth { get; set; }
    }
}
