using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class DefSortOptions : ObservableObjectBase
    {
        [ObservableProperty]
        private int _headroom = 1;
        [ObservableProperty]
        private bool _removeUnusedDef = false;
        [ObservableProperty]
        private bool _sort = true;
        [ObservableProperty]
        private bool _sortByName = false;
        [ObservableProperty]
        private bool _fixLnEnd = true;
        [ObservableProperty]
        private bool _removeMultiDef = false;
        [ObservableProperty(Related = [nameof(TargetRadix_None), nameof(TargetRadix_FF), nameof(TargetRadix_ZZ), nameof(TargetRadix_zz)])]
        private TargetRadixType _targetRadix = TargetRadixType.None;

        [JsonIgnore]
        public bool TargetRadix_None { get => _targetRadix is TargetRadixType.None; set => SetTargetRadix(TargetRadixType.None, value); }
        [JsonIgnore]
        public bool TargetRadix_FF { get => _targetRadix is TargetRadixType.FF; set => SetTargetRadix(TargetRadixType.FF, value); }
        [JsonIgnore]
        public bool TargetRadix_ZZ { get => _targetRadix is TargetRadixType.ZZ; set => SetTargetRadix(TargetRadixType.ZZ, value); }
        [JsonIgnore]
        public bool TargetRadix_zz { get => _targetRadix is TargetRadixType.zz; set => SetTargetRadix(TargetRadixType.zz, value); }

        private void SetTargetRadix(TargetRadixType type, bool value)
        {
            if (value)
            {
                TargetRadix = type;
            }
        }
    }

    public enum TargetRadixType
    {
        None,
        FF,
        ZZ,
        zz,
    }
}
