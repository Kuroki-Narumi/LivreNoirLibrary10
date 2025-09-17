using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public record SoundChannel
    {
        [JsonPropertyName("name")]
        public string FileName { get; set; } = "";

        [JsonPropertyName("notes")]
        public List<Note> NoteList { get; set; } = [];
    }
}
