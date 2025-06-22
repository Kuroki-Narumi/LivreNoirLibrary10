using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Media;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class CommonColors : GenericPropertyBase<Color>
    {
        private static readonly Dictionary<string, Color> _default_values = new()
        {
            { nameof(HeaderText), Colors.HeaderText },
            { nameof(Bar), Colors.BarLine },
            { nameof(Beat), Colors.BeatLine },
            { nameof(SubBeat), Colors.SubBeatLine },
            { nameof(LaneBorder), Colors.LaneBorder },
            { nameof(Mine), Colors.Note_Mine },
            { nameof(LongEnd), Colors.Note_LongEnd },
            { nameof(Selected), Colors.Selected },
            { nameof(SelectedLong), Colors.SelectedLong },
        };

        [JsonIgnore]
        public override Dictionary<string, Color> DefaultValues => _default_values;

        public Color HeaderText { get => GetValue(); set => SetValue(value); }
        public Color Bar { get => GetValue(); set => SetValue(value); }
        public Color Beat { get => GetValue(); set => SetValue(value); }
        public Color SubBeat { get => GetValue(); set => SetValue(value); }
        public Color LaneBorder { get => GetValue(); set => SetValue(value); }
        public Color Mine { get => GetValue(); set => SetValue(value); }
        public Color LongEnd { get => GetValue(); set => SetValue(value); }
        public Color Selected { get => GetValue(); set => SetValue(value); }
        public Color SelectedLong { get => GetValue(); set => SetValue(value); }

        public void SetDefault() => Load(_default_values);

        public Dictionary<string, string> ToSerializable()
        {
            Dictionary<string, string> dic = [];
            foreach (var (key, value) in _values)
            {
                dic[key] = value.GetColorCode();
            }
            return dic;
        }

        public void Load(CommonColors source)
        {
            foreach (var (key, value) in source._values)
            {
                SetValue(value, key);
            }
        }

        public void Load(Dictionary<string, string>? source)
        {
            if (source is not null)
            {
                foreach (var (key, value) in source)
                {
                    if (value.TryParseToColor(out var color))
                    {
                        SetValue(color, key);
                    }
                }
            }
        }
    }
}
