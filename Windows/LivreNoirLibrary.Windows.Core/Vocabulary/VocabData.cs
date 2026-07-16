using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows
{
    [JsonConverter(typeof(VocabDataJsonConverter))]
    public class VocabData : ObservableObjectBase, IVocabData, IWriteJson, IEquatable<VocabData>
    {
        public const string Leader = "...";

        public string? Value
        {
            get;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    SetValue(ref field, value, [nameof(WithLeader), nameof(MenuHeader), nameof(MenuHeaderWithLeader)]);
                }
            }
        }

        public string? KeyTip
        {
            get;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    SetValue(ref field, value, [nameof(WithLeader), nameof(MenuHeader), nameof(MenuHeaderWithLeader)]);
                }
            }
        }

        public string WithLeader => $"{Value}{Leader}";
        public string MenuHeader => string.IsNullOrEmpty(KeyTip) ? Value ?? "" : $"{Value}(_{KeyTip})";
        public string MenuHeaderWithLeader => string.IsNullOrEmpty(KeyTip) ? WithLeader : $"{WithLeader}(_{KeyTip})";

        public override string ToString() => Value ?? "";

        public static implicit operator string(VocabData value) => value.Value ?? "";
        public static implicit operator VocabData(string value) => new() { Value = value };
        public static implicit operator VocabData((string, string) tuple) => new() { Value = tuple.Item1, KeyTip = tuple.Item2 };

        public void Update(VocabData source) => Update(source.Value, source.KeyTip);

        public void Update(string? value, string? keyTip)
        {
            Value = value;
            KeyTip = keyTip;
        }

        public bool Equals(string? value, string? keyTip) => (string.IsNullOrEmpty(value) || Value == value) && (string.IsNullOrEmpty(keyTip) || KeyTip == keyTip);
        public bool Equals(VocabData? other) => other is not null && Equals(other.Value, other.KeyTip);
        public override bool Equals(object? obj) => obj is VocabData data && Equals(data);
        public override int GetHashCode() => HashCode.Combine(Value, KeyTip);

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options) => VocabDataJsonConverter.WriteStatic(this, writer, options);
    }
}
