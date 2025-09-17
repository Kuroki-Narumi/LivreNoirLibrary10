using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class Regulation
    {
        [JsonPropertyName(JsonPropertyNames.Forbidden)]
        public List<string>? Forbidden { get; set; }

        [JsonPropertyName(JsonPropertyNames.Limit1)]
        public List<string>? Limit1 { get; set; }

        [JsonPropertyName(JsonPropertyNames.Limit2)]
        public List<string>? Limit2 { get; set; }

        [JsonPropertyName(JsonPropertyNames.Specified)]
        public List<string>? Specified { get; set; }
    }
}
