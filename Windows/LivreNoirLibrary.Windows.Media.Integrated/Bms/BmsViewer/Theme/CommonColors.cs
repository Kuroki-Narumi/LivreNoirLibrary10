using System;
using System.Collections.Generic;
using System.Windows.Media;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class CommonColors : GenericPropertyBase<Color>
    {
        protected override void InitializeDefaultValues(Dictionary<string, Color> defaultValues)
        {
            defaultValues[nameof(HeaderText)] = Colors.HeaderText;
            defaultValues[nameof(Bar)] = Colors.BarLine;
            defaultValues[nameof(Beat)] = Colors.BeatLine;
            defaultValues[nameof(SubBeat)] = Colors.SubBeatLine;
            defaultValues[nameof(LaneBorder)] = Colors.LaneBorder;
            defaultValues[nameof(Mine)] = Colors.Note_Mine;
            defaultValues[nameof(LongEnd)] = Colors.Note_LongEnd;
            defaultValues[nameof(Selected)] = Colors.Selected;
            defaultValues[nameof(SelectedLong)] = Colors.SelectedLong;
            defaultValues[nameof(WaveForm)] = Colors.WaveForm;
        }

        public Color HeaderText { get => GetValue(); set => SetValue(value); }
        public Color Bar { get => GetValue(); set => SetValue(value); }
        public Color Beat { get => GetValue(); set => SetValue(value); }
        public Color SubBeat { get => GetValue(); set => SetValue(value); }
        public Color LaneBorder { get => GetValue(); set => SetValue(value); }
        public Color Mine { get => GetValue(); set => SetValue(value); }
        public Color LongEnd { get => GetValue(); set => SetValue(value); }
        public Color Selected { get => GetValue(); set => SetValue(value); }
        public Color SelectedLong { get => GetValue(); set => SetValue(value); }
        public Color WaveForm { get => GetValue(); set => SetValue(value); }

        public Dictionary<string, string> ToSerializable()
        {
            Dictionary<string, string> dic = [];
            foreach (var (key, value) in EnumerateValues())
            {
                dic[key] = value.GetColorCode();
            }
            return dic;
        }

        public void Load(CommonColors source)
        {
            foreach (var (key, value) in source.EnumerateValues())
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
