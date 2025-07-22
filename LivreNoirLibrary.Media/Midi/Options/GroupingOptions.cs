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

        [ObservableProperty]
        private bool _selection;
        [ObservableProperty(Related = [nameof(Type_None), nameof(Type_All), nameof(Type_Glide), nameof(Type_Group), nameof(Type_Tuple)])]
        private EnchordType _type = EnchordType.Glide;
        [ObservableProperty]
        private bool _downward;
        [ObservableProperty]
        private int _groupCount = MinimumGroupCount;
        [ObservableProperty]
        private int _tupleCount = MinimumTupleCount;
        [ObservableProperty]
        private bool _preDechord;

        private static int CoerceGroupCount(int value) => Math.Clamp(value, MinimumGroupCount, MaximumGroupCount);
        private static int CoerceTupleCount(int value) => Math.Clamp(value, MinimumTupleCount, MaximumTupleCount);

        [JsonIgnore]
        public bool Type_None { get => _type is EnchordType.None; set => SetType(EnchordType.None, value); }
        [JsonIgnore]
        public bool Type_All { get => _type is EnchordType.All; set => SetType(EnchordType.All, value); }
        [JsonIgnore]
        public bool Type_Glide { get => _type is EnchordType.Glide; set => SetType(EnchordType.Glide, value); }
        [JsonIgnore]
        public bool Type_Group { get => _type is EnchordType.Group; set => SetType(EnchordType.Group, value); }
        [JsonIgnore]
        public bool Type_Tuple { get => _type is EnchordType.Tuple; set => SetType(EnchordType.Tuple, value); }

        private void SetType(EnchordType type, bool value)
        {
            if (value)
            {
                Type = type;
            }
        }

        public void RotateType(int delta)
        {
            var v = (int)_type;
            Type = (EnchordType)((delta is > 0 ? (v + 4) : (v + 1)) % 5);
        }
    }
}
