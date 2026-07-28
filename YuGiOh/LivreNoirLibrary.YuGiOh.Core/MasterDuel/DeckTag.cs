using LivreNoirLibrary.ObjectModel;
using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public  class DeckTag : ObservableObjectBase, IComparable<DeckTag>, INamedObject
    {
        [JsonPropertyName(JsonPropertyNames.Name)]
        public string Name { get; set => SetValue(ref field, value); } = "";

        [JsonPropertyName(JsonPropertyNames.Hint)]
        public string SearchHint { get; set => SetValue(ref field, value); } = "";

        public bool IsMatch(ReadOnlySpan<char> text) => Name.Contains(text, StringComparison.OrdinalIgnoreCase) || SearchHint.Contains(text, StringComparison.OrdinalIgnoreCase);

        public DeckTag Clone() => new() { Name = Name, SearchHint = SearchHint };

        public int CompareTo(DeckTag? other) => other is { } o ? Name.CompareTo(other.Name, StringComparison.Ordinal) : -1;
    }
}
