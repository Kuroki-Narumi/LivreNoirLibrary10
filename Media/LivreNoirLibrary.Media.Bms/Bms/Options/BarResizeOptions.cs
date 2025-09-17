using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BarResizeOptions : ObservableObjectBase
    {
        internal readonly SortedSet<int> _numbers = [];

        public Rational Length { get; set => SetValue(ref field, value); }
        public bool RatioMode { get; set => SetValue(ref field, value); }
        public BarResizeMode Mode
        {
            get;
            set => SetValue(ref field, value, [nameof(Mode_None), nameof(Mode_Trim), nameof(Mode_Overlap), nameof(Mode_Stretch), nameof(Mode_Slide)]);
        } = BarResizeMode.Trim;
        public bool StretchWithTempo { get; set => SetValue(ref field, value); } = true;

        [JsonIgnore]
        public IEnumerable<int> Numbers
        {
            get => _numbers;
            set
            {
                _numbers.Clear();
                _numbers.UnionWith(value);
                SendPropertyChanged();
            }
        }

        [JsonIgnore]
        public bool Mode_None { get => Mode is BarResizeMode.None; set => SetMode(BarResizeMode.None, value); }
        [JsonIgnore]
        public bool Mode_Trim { get => Mode is BarResizeMode.Trim; set => SetMode(BarResizeMode.Trim, value); }
        [JsonIgnore]
        public bool Mode_Overlap { get => Mode is BarResizeMode.Overlap; set => SetMode(BarResizeMode.Overlap, value); }
        [JsonIgnore]
        public bool Mode_Stretch { get => Mode is BarResizeMode.Stretch; set => SetMode(BarResizeMode.Stretch, value); }
        [JsonIgnore]
        public bool Mode_Slide { get => Mode is BarResizeMode.Slide; set => SetMode(BarResizeMode.Slide, value); }

        private void SetMode(BarResizeMode mode, bool value)
        {
            if (value)
            {
                Mode = mode;
            }
        }
    }

    public enum BarResizeMode
    {
        None,
        Trim,
        Overlap,
        Stretch,
        Slide,
    }
}