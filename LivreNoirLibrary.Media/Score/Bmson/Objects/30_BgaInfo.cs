using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record BgaInfo
    {
        [JsonPropertyName("bga_header")]
        public List<BgaHeader> Headers { get; set; } = [];

        [JsonPropertyName("bga_events")]
        public List<Bga> BaseList { get; set; } = [];

        [JsonPropertyName("layer_events")]
        public List<Bga> LayerList { get; set; } = [];

        [JsonPropertyName("poor_events")]
        public List<Bga> PoorList { get; set; } = [];
    }
}
