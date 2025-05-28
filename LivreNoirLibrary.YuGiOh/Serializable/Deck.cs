using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class Deck
    {
        [JsonPropertyName(JsonPropertyNames.MainDeck)]
        public List<string>? MainDeck { get; set; }

        [JsonPropertyName(JsonPropertyNames.ExtraDeck)]
        public List<string>? ExtraDeck { get; set; }

        [JsonPropertyName(JsonPropertyNames.SideDeck)]
        public List<string>? SideDeck { get; set; }
    }
}
