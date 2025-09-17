using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class Card
    {
        [JsonPropertyName(JsonPropertyNames.Id)]
        public int Id { get; set; }

        [JsonPropertyName(JsonPropertyNames.Name)]
        public string? Name { get; set; }

        [JsonPropertyName(JsonPropertyNames.Ruby)]
        public string? Ruby { get; set; }

        [JsonPropertyName(JsonPropertyNames.EnName)]
        public string? EnName { get; set; }

        [JsonPropertyName(JsonPropertyNames.Type)]
        public CardType CardType { get; set; }

        [JsonPropertyName(JsonPropertyNames.Text)]
        public string? Text { get; set; }

        [JsonPropertyName(JsonPropertyNames.Unusable)]
        public bool? Unusable { get; set; }

        [JsonPropertyName(JsonPropertyNames.MonsterInfo)]
        public MonsterInfo? MonsterInfo { get; set; }

        [JsonPropertyName(JsonPropertyNames.PendulumInfo)]
        public PendulumInfo? PendulumInfo { get; set; }

        [JsonPropertyName(JsonPropertyNames.PackInfo)]
        public List<PackInfo>? PackInfo { get; set; }
    }
}
