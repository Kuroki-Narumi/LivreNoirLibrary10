using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class KeySortOptions : ObservableObjectBase
    {
        [JsonIgnore]
        [ObservableProperty]
        private SortedDictionary<int, int> _groupList = [];
        [ObservableProperty]
        private int _minimumMemberCount = 2;
        [ObservableProperty]
        private ConvertTarget _target = new();
        [ObservableProperty]
        private int _startLane = 1;
        [ObservableProperty]
        private bool _removeMeta = true;
    }
}
