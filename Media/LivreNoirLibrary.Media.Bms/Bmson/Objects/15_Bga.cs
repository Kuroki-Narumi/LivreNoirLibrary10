using LivreNoirLibrary.ObjectModel;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record Bga : Object, IId
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
