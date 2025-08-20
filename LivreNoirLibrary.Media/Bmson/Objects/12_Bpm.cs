using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record Bpm : Object
    {
        [JsonPropertyName("bpm")]
        public double Tempo { get; set; }
    }
}
