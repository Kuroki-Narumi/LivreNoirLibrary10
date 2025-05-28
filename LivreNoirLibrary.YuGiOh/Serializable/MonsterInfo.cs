using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Serializable
{
    public class MonsterInfo
    {
        [JsonPropertyName(JsonPropertyNames.Attribute)]
        public Attribute Attribute { get; set; }

        [JsonPropertyName(JsonPropertyNames.Type)]
        public MonsterType Type { get; set; }

        [JsonPropertyName(JsonPropertyNames.Effect)]
        public bool? Effect { get; set; }

        [JsonPropertyName(JsonPropertyNames.Ability)]
        public Ability? Ability { get; set; }

        [JsonPropertyName(JsonPropertyNames.Level)]
        public int Level { get; set; }

        [JsonPropertyName(JsonPropertyNames.Atk)]
        [JsonConverter(typeof(Converters.StatusJsonConverter))]
        public int Atk { get; set; }

        [JsonPropertyName(JsonPropertyNames.Def)]
        [JsonConverter(typeof(Converters.StatusJsonConverter))]
        public int Def { get; set; }
    }
}
