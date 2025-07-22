using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record RateEvent : Object
    {
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; } = Constants.DefaultScrollRate;
    }
}
