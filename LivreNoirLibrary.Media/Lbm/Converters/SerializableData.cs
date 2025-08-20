using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media.Lbm
{
    public class SerializableData
    {
    }

    public class SerializableConductor
    {
        [JsonPropertyName(JsonPropertyNames.Position)]
        public BarPosition Position { get; set; }
        [JsonPropertyName(JsonPropertyNames.Tempo)]
        public double Tempo { get; set; }
        [JsonPropertyName(JsonPropertyNames.Stop)]
        public Rational Stop { get; set; }
        [JsonPropertyName(JsonPropertyNames.Scroll)]
        public double Scroll { get; set; }
    }

    public class SerializableNote
    {
        [JsonPropertyName(JsonPropertyNames.Position)]
        public BarPosition Position { get; set; }
        [JsonPropertyName(JsonPropertyNames.Lane)]
        public int Lane { get; set; }
        [JsonPropertyName(JsonPropertyNames.Id)]
        public int Id { get; set; }
        [JsonPropertyName(JsonPropertyNames.Type)]
        public NoteType Type { get; set; }
        [JsonPropertyName(JsonPropertyNames.NoteLnType)]
        public Bms.LongNoteMode LnType { get; set; }
        [JsonPropertyName(JsonPropertyNames.NoteOffset)]
        public string? Offset { get; set; }
        [JsonPropertyName(JsonPropertyNames.NoteLength)]
        public string? Length { get; set; }
    }

    public class SerializableMetaNote
    {
        [JsonPropertyName(JsonPropertyNames.Position)]
        public BarPosition Position { get; set; }
        [JsonPropertyName(JsonPropertyNames.Lane)]
        public int Lane { get; set; }
        [JsonPropertyName(JsonPropertyNames.Id)]
        public int Id { get; set; }
        [JsonPropertyName(JsonPropertyNames.Value)]
        public string? Value { get; set; }
        [JsonPropertyName(JsonPropertyNames.DestinationX)]
        public int? DestinationX { get; set; }
        [JsonPropertyName(JsonPropertyNames.DestinationY)]
        public int? DestinationY { get; set; }
        [JsonPropertyName(JsonPropertyNames.CenterX)]
        public int? CenterX { get; set; }
        [JsonPropertyName(JsonPropertyNames.CenterY)]
        public int? CenterY { get; set; }
        [JsonPropertyName(JsonPropertyNames.Angle)]
        public double Angle { get; set; }
        [JsonPropertyName(JsonPropertyNames.AngleX)]
        public int? AngleX { get; set; }
        [JsonPropertyName(JsonPropertyNames.AngleY)]
        public int? AngleY { get; set; }
    }
}
