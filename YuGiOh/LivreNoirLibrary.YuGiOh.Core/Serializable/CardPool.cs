using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class CardPool
    {
        [JsonPropertyName(JsonPropertyNames.Cards)]
        public List<Card> Cards { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Packs)]
        public List<CardPack> Packs { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.LastUpdate)]
        [JsonConverter(typeof(Converters.DateTimeJsonConverter))]
        public DateTime LastUpdate { get; set; }
    }
}
