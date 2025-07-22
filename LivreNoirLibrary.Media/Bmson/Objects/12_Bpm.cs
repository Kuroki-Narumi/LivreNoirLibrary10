using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record Bpm : Object
    {
        [JsonPropertyName("bpm")]
        public decimal Tempo { get; set; }
    }
}
