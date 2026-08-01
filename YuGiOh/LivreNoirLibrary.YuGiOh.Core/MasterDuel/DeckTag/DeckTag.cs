using LivreNoirLibrary.ObjectModel;
using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class DeckTag : ObservableObjectBase, IComparable<DeckTag>, INamedObject, IDeckTag
    {
        [JsonPropertyName(JsonPropertyNames.Name)]
        public string? Name { get; set => SetValue(ref field, value); }

        [JsonPropertyName(JsonPropertyNames.Hint)]
        public string? SearchHint { get; set => SetValue(ref field, value); }

        public int CompareTo(DeckTag? other) => IDeckTagExtensions.Compare(this, other);
    }
}
