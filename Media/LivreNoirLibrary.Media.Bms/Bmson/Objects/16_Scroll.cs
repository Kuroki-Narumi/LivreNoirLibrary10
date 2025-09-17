using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record RateEvent : Object
    {
        [JsonPropertyName("rate")]
        public double Rate { get; set; } = Constants.DefaultRate;
    }
}
