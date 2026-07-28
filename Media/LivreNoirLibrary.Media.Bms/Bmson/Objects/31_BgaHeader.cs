using LivreNoirLibrary.ObjectModel;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record BgaHeader : IId
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string FileName { get; set; } = "";
    }
}
