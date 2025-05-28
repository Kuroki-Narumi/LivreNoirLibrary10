using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class PackInfo
    {
        [JsonPropertyName(JsonPropertyNames.ProductId)]
        public string? ProductId { get; set; }

        [JsonPropertyName(JsonPropertyNames.Number)]
        public string? Number { get; set; }
    }
}
