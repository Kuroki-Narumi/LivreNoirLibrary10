using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class PendulumInfo
    {
        [JsonPropertyName(JsonPropertyNames.Scale)]
        public int Scale { get; set; }

        [JsonPropertyName(JsonPropertyNames.Text)]
        public string? Text { get; set; }
    }
}
