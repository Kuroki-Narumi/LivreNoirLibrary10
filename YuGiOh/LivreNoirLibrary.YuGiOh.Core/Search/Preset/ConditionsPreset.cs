using LivreNoirLibrary.ObjectModel;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public abstract class ConditionPreset : ObservableObjectBase
    {
        public static void CopyHashSet<T>(HashSet<T> from, HashSet<T> to)
        {
            to.Clear();
            to.UnionWith(from);
        }
    }

    public abstract class ConditionsPreset<T> : ConditionPreset
        where T : new()
    {
        [JsonPropertyName(JsonPropertyNames.Name)]
        public string Name { get; set => SetValue(ref field, value); } = "";

        [JsonIgnore]
        public bool IsDefault { get; set => SetValue(ref field, value); }
        [JsonPropertyName(JsonPropertyNames.Preset_Default)]
        public bool? IsDefault_Nullable { get => IsDefault ? true : null; set => IsDefault = value is true; }

        [JsonPropertyName(JsonPropertyNames.Preset_Conditions)]
        public T Conditions { get; set; } = new();

        public abstract void Copy(T from, T to);
    }
}
