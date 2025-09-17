using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record Bga : Object
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
