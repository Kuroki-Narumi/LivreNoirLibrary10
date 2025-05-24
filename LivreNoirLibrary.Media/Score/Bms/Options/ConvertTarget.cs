using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class ConvertTarget() : ObservableObjectBase
    {
        [ObservableProperty(Related = [nameof(Type_All), nameof(Type_Key), nameof(Type_Bgm), nameof(Type_Selected), nameof(Type_BgmAndSelected), nameof(Type_Lane), nameof(Type_Id)])]
        protected ConvertTargetType _type;
        [JsonIgnore]
        [ObservableProperty]
        protected bool _isSelectionEnabled;
        private readonly SortedSet<int> _targets = [];

        [JsonIgnore]
        public bool Type_All { get => _type is ConvertTargetType.All; set => SetType(ConvertTargetType.All, value); }
        [JsonIgnore]
        public bool Type_Key { get => _type is ConvertTargetType.Key; set => SetType(ConvertTargetType.Key, value); }
        [JsonIgnore]
        public bool Type_Bgm { get => _type is ConvertTargetType.Bgm; set => SetType(ConvertTargetType.Bgm, value); }
        [JsonIgnore]
        public bool Type_Selected { get => _type is ConvertTargetType.Selected; set => SetType(ConvertTargetType.Selected, value); }
        [JsonIgnore]
        public bool Type_BgmAndSelected { get => _type is ConvertTargetType.BgmAndSelected; set => SetType(ConvertTargetType.BgmAndSelected, value); }
        [JsonIgnore]
        public bool Type_Lane { get => _type is ConvertTargetType.Lane; set => SetType(ConvertTargetType.Lane, value); }
        [JsonIgnore]
        public bool Type_Id { get => _type is ConvertTargetType.Id; set => SetType(ConvertTargetType.Id, value); }

        public ConvertTarget(ConvertTargetType type) : this()
        {
            _type = type;
        }

        private void OnIsSelectionEnabledChanged(bool value)
        {
            if (!value && _type is ConvertTargetType.Selected or ConvertTargetType.BgmAndSelected)
            {
                Type = ConvertTargetType.All;
            }
        }

        [JsonIgnore]
        public IEnumerable<int>? TargetList
        {
            [return: NotNull]
            get => _targets;
            set
            {
                _targets.Clear();
                if (value is not null)
                {
                    _targets.UnionWith(value);
                    SendPropertyChanged(nameof(TargetList));
                }
            }
        }

        private void SetType(ConvertTargetType type, bool value)
        {
            if (value)
            {
                Type = type;
            }
        }

        public Predicate<Note> GetSelector(HashSet<Note> selection, bool includesLongEnd)
        {
            return _type switch
            {
                ConvertTargetType.Key => n => n.IsVisibleKey(includesLongEnd),
                ConvertTargetType.Bgm => n => n.IsBgm(),
                ConvertTargetType.Selected => n => n.IsPlayableSound(includesLongEnd) && selection.Contains(n),
                ConvertTargetType.BgmAndSelected => n => n.IsBgm() || (n.IsPlayableSound(includesLongEnd) && selection.Contains(n)),
                ConvertTargetType.Lane => n => n.IsPlayableSound(includesLongEnd) && _targets.Contains(n.Lane),
                ConvertTargetType.Id => n => n.IsPlayableSound(includesLongEnd) && _targets.Contains(n.Id),
                _ => n => n.IsPlayableSound(includesLongEnd)
            };
        }
    }
}
