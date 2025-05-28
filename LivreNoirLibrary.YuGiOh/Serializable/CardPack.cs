using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class CardPack
    {
        [JsonPropertyName(JsonPropertyNames.ProductId)]
        public string ProductId { get; set; } = "";

        [JsonPropertyName(JsonPropertyNames.Name)]
        public string Name { get; set; } = "";

        [JsonPropertyName(JsonPropertyNames.Date)]
        [JsonConverter(typeof(Converters.DateOnlyJsonConverter))]
        public DateTime Date { get; set; }
    }
}
