using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class CardPackItem
    {
        [JsonPropertyName(JsonPropertyNames.Id)]
        public int CardId { get; set; }

        [JsonPropertyName(JsonPropertyNames.Number)]
        public string Number { get; set; } = "";
    }
}
