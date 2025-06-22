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

        [ObservableProperty(Related = [nameof(HeaderWithLeader), nameof(MenuHeader), nameof(MenuHeaderWithLeader)])]
        private string _header = "";
        private string? _description;
        [ObservableProperty(Related = [nameof(HeaderWithLeader), nameof(MenuHeader), nameof(MenuHeaderWithLeader)])]
        private string? _keyTip;

        public string? Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (!string.IsNullOrEmpty(_description))
                    {
                        _description = null;
                        SendPropertyChanged(nameof(Description));
                    }
                }
                else
                {
                    SetProperty(ref _description, value);
                }
            }
        }

        public string HeaderWithLeader => $"{_header}{Leader}";
        public string MenuHeader => string.IsNullOrEmpty(_keyTip) ? _header : $"{_header}(_{_keyTip})";
        public string MenuHeaderWithLeader => string.IsNullOrEmpty(_keyTip) ? HeaderWithLeader : $"{HeaderWithLeader}(_{_keyTip})";

        public static implicit operator string(VocabData value) => value._header;
        public static implicit operator VocabData(string value) => new() { _header = value };
        public static implicit operator VocabData((string, string) tuple) => new() { _header = tuple.Item1, Description = tuple.Item2 };
        public static implicit operator VocabData((string, string, string) tuple) => new() { _header = tuple.Item1, Description = tuple.Item2, _keyTip = tuple.Item3 };

        public void Update(VocabData source)
        {
            SetProperty(ref _header, source._header, nameof(Header));
            SetProperty(ref _description, source._description, nameof(Description));
            if (!string.IsNullOrEmpty(source._keyTip))
            {
                SetProperty(ref _keyTip, source._keyTip, nameof(KeyTip));
            }
            SendPropertyChanged(nameof(HeaderWithLeader));
            SendPropertyChanged(nameof(MenuHeader));
            SendPropertyChanged(nameof(MenuHeaderWithLeader));
        }

        public VocabData Clone() => new() { _header = _header, _description = _description, _keyTip = _keyTip };

        public bool Equals(VocabData? other) => other is not null && _header == other._header && _description == other._description && _keyTip == other._keyTip;
        public override bool Equals(object? obj) => obj is VocabData data && Equals(data);
        public override int GetHashCode() => HashCode.Combine(_header, _description, _keyTip);

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options) => VocabDataJsonConverter.WriteStatic(this, writer, options);
    }
}
