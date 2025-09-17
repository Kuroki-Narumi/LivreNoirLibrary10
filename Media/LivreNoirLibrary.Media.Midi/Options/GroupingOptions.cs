using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class GroupingOptions : ObservableObjectBase
    {
        public const int MinimumGroupCount = 1;
        public const int MaximumGroupCount = 7;
        public const int MinimumTupleCount = 2;
        public const int MaximumTupleCount = 7;

        public bool Selection { get; set => SetValue(ref field, value); }
        public EnchordType Type { get; set => SetValue(ref field, value, [nameof(Type_None), nameof(Type_All), nameof(Type_Glide), nameof(Type_Group), nameof(Type_Tuple)]); }
        public bool Downward { get; set => SetValue(ref field, value); }
        public int GroupCount { get; set => SetValue(ref field, Math.Clamp(value, MinimumGroupCount, MaximumGroupCount)); } = MinimumGroupCount;
        public int TupleCount { get; set => SetValue(ref field, Math.Clamp(value, MinimumTupleCount, MaximumTupleCount)); } = MinimumTupleCount;
        public bool PreDechord { get; set => SetValue(ref field, value); }

        [JsonIgnore]
        public bool Type_None { get => Type is EnchordType.None; set => SetType(EnchordType.None, value); }
        [JsonIgnore]
        public bool Type_All { get => Type is EnchordType.All; set => SetType(EnchordType.All, value); }
        [JsonIgnore]
        public bool Type_Glide { get => Type is EnchordType.Glide; set => SetType(EnchordType.Glide, value); }
        [JsonIgnore]
        public bool Type_Group { get => Type is EnchordType.Group; set => SetType(EnchordType.Group, value); }
        [JsonIgnore]
        public bool Type_Tuple { get => Type is EnchordType.Tuple; set => SetType(EnchordType.Tuple, value); }

        private void SetType(EnchordType type, bool value)
        {
            if (value)
            {
                Type = type;
            }
        }
    }
}
