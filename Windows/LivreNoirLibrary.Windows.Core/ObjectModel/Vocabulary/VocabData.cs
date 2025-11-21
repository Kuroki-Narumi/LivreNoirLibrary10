using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows
{
    [JsonConverter(typeof(VocabDataJsonConverter))]
    public partial class VocabData : ObservableObjectBase, IJsonWriter, IEquatable<VocabData>
    {
        public const string Leader = "...";

        public string Header { get; set => SetValue(ref field, value, [nameof(HeaderWithLeader), nameof(MenuHeader), nameof(MenuHeaderWithLeader)]); } = "";

        public string? Description
        {
            get;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    SetValue(ref field, value);
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
                    SetValue(ref field, value, [nameof(HeaderWithLeader), nameof(MenuHeader), nameof(MenuHeaderWithLeader)]);
                }
            }
        }

        public string HeaderWithLeader => $"{Header}{Leader}";
        public string MenuHeader => string.IsNullOrEmpty(KeyTip) ? Header : $"{Header}(_{KeyTip})";
        public string MenuHeaderWithLeader => string.IsNullOrEmpty(KeyTip) ? HeaderWithLeader : $"{HeaderWithLeader}(_{KeyTip})";

        public override string ToString() => Header;

        public static implicit operator string(VocabData value) => value.Header;
        public static implicit operator VocabData(string value) => new() { Header = value };
        public static implicit operator VocabData((string, string) tuple) => new() { Header = tuple.Item1, Description = tuple.Item2 };
        public static implicit operator VocabData((string, string, string) tuple) => new() { Header = tuple.Item1, Description = tuple.Item2, KeyTip = tuple.Item3 };

        public void Update(VocabData source)
        {
            Header = source.Header;
            Description = source.Description;
            KeyTip = source.KeyTip;
        }

        public bool Equals(VocabData? other) => other is not null && Header == other.Header && Description == other.Description && KeyTip == other.KeyTip;
        public override bool Equals(object? obj) => obj is VocabData data && Equals(data);
        public override int GetHashCode() => HashCode.Combine(Header, Description, KeyTip);

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options) => VocabDataJsonConverter.WriteStatic(this, writer, options);
    }
}
