using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record Stop : Object
    {
        [JsonPropertyName("duration")]
        public long Duration { get; set; }
    }
}
