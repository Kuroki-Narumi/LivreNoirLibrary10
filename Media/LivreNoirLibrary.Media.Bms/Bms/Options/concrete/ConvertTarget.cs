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
