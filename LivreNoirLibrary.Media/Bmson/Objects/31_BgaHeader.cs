using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record BgaHeader
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string FileName { get; set; } = "";
    }
}
