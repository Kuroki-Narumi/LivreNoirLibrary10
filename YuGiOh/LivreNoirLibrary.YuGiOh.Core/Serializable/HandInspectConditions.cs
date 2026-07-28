using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class HandInspectConditions<T>
    {
        [JsonPropertyName(JsonPropertyNames.Name)]
        public string? Name { get; set; }

        [JsonPropertyName(JsonPropertyNames.GroupId)]
        public int? GroupId { get; set; }

        public int? Group { get; set; }

        [JsonPropertyName(JsonPropertyNames.Value1)]
        public double Value1 { get; set; }

        [JsonPropertyName(JsonPropertyNames.Value2)]
        public double Value2 { get; set; }

        [JsonPropertyName(JsonPropertyNames.Items)]
        public List<List<T>>? Items { get; set; }
    }
}
