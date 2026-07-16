using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class Deck
    {
        [JsonPropertyName(JsonPropertyNames.MainDeck)]
        public List<int>? MainDeck { get; set; }

        [JsonPropertyName(JsonPropertyNames.ExtraDeck)]
        public List<int>? ExtraDeck { get; set; }

        [JsonPropertyName(JsonPropertyNames.SideDeck)]
        public List<int>? SideDeck { get; set; }

        public bool IsEmpty() => IsEmpty(MainDeck) && IsEmpty(ExtraDeck) && IsEmpty(SideDeck);
        private static bool IsEmpty(List<int>? list) => list is null || list.Count is 0;
    }
}
