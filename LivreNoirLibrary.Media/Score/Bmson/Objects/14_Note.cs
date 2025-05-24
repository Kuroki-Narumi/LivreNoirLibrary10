using System.Text.Json.Serialization;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Media.Bmson
{
    public record Note : Object
    {
        [JsonIgnore]
        public int X { get; set; }
        [JsonPropertyName("x")]
        public int? X_Nullable { get => X is 0 ? null : X; set => X = value ?? 0; }

        [JsonPropertyName("l")]
        public long Length { get; set; }

        [JsonPropertyName("c")]
        public bool Continue { get; set; }

        [JsonIgnore]
        public LongNoteMode Type { get; set; }
        [JsonPropertyName("t")]
        public LongNoteMode? Type_Nullable { get => Type is LongNoteMode.Auto ? null : Type; set => Type = value ?? LongNoteMode.Auto; }

        [JsonIgnore]
        public bool Up { get; set; }
        [JsonPropertyName("up")]
        public bool? Up_Nullable { get => Up ? Up : null; set => Up = value is true; }
    }
}
