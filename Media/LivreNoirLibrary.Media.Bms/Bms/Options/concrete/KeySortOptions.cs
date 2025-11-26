using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class KeySortOptions : ObservableObjectBase
    {
        [JsonIgnore]
        public SortedDictionary<int, int> GroupList { get; set => SetValue(ref field, value); } = [];
        public int MinimumMemberCount { get; set => SetValue(ref field, value); } = 2;
        public ConvertTarget ConvertTarget { get; set => SetValue(ref field, value); } = new();
        public int StartLane { get; set => SetValue(ref field, value); } = 1;
        public bool RemoveMeta { get; set => SetValue(ref field, value); } = true;
    }
}
