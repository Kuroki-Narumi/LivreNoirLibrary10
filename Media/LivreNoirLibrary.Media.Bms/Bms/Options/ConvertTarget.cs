using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class ConvertTarget : ObservableObjectBase
    {
        private readonly SortedSet<int> _targets = [];

        public ConvertTargetType Type { get; set => SetValue(ref field, value, [nameof(Type_All), nameof(Type_Key), nameof(Type_Bgm), nameof(Type_Selected), nameof(Type_BgmAndSelected), nameof(Type_Lane), nameof(Type_Id)]); }
        [JsonIgnore]
        public bool IsSelectionEnabled
        {
            get;
            set
            {
                if (SetValue(ref field, value))
                {
                    if (!value && Type is ConvertTargetType.Selected or ConvertTargetType.BgmAndSelected)
                    {
                        Type = ConvertTargetType.All;
                    }
                }
            }
        }

        [JsonIgnore]
        public bool Type_All { get => Type is ConvertTargetType.All; set => SetType(ConvertTargetType.All, value); }
        [JsonIgnore]
        public bool Type_Key { get => Type is ConvertTargetType.Key; set => SetType(ConvertTargetType.Key, value); }
        [JsonIgnore]
        public bool Type_Bgm { get => Type is ConvertTargetType.Bgm; set => SetType(ConvertTargetType.Bgm, value); }
        [JsonIgnore]
        public bool Type_Selected { get => Type is ConvertTargetType.Selected; set => SetType(ConvertTargetType.Selected, value); }
        [JsonIgnore]
        public bool Type_BgmAndSelected { get => Type is ConvertTargetType.BgmAndSelected; set => SetType(ConvertTargetType.BgmAndSelected, value); }
        [JsonIgnore]
        public bool Type_Lane { get => Type is ConvertTargetType.Lane; set => SetType(ConvertTargetType.Lane, value); }
        [JsonIgnore]
        public bool Type_Id { get => Type is ConvertTargetType.Id; set => SetType(ConvertTargetType.Id, value); }

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

        public Predicate<INote> GetSelector(HashSet<INote> selection, bool includeLongEnd)
        {
            return Type switch
            {
                ConvertTargetType.Key => n => n.IsVisibleKey(includeLongEnd, out _),
                ConvertTargetType.Bgm => n => n.IsBgm(out _),
                ConvertTargetType.Selected => n => n.IsPlayableSound(includeLongEnd, out _) && selection.Contains(n),
                ConvertTargetType.BgmAndSelected => n => n.IsSound(n => n.IsBgm() || (n.IsNormal(includeLongEnd) && selection.Contains(n))),
                ConvertTargetType.Lane => n => n.IsSound(n => n.IsPlayableSound(includeLongEnd) && _targets.Contains(n.Lane)),
                ConvertTargetType.Id => n => n.IsSound(n => n.IsPlayableSound(includeLongEnd) && _targets.Contains(n.Value)),
                _ => n => n.IsPlayableSound(includeLongEnd, out _)
            };
        }
    }

    public enum ConvertTargetType
    {
        All,
        Key,
        Bgm,
        Selected,
        BgmAndSelected,
        Lane,
        Id,
    }
}
