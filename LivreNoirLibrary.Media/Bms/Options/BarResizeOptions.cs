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
        [ObservableProperty]
        private Rational _length;
        [ObservableProperty]
        private bool _ratio;
        [ObservableProperty(Related = [nameof(Mode_None), nameof(Mode_Trim), nameof(Mode_Overlap), nameof(Mode_Stretch), nameof(Mode_Slide)])]
        private BarResizeMode _mode = BarResizeMode.Trim;
        [ObservableProperty]
        private bool _stretchWithTempo = true;

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
        public bool Mode_None { get => _mode is BarResizeMode.None; set => SetMode(BarResizeMode.None, value); }
        [JsonIgnore]
        public bool Mode_Trim { get => _mode is BarResizeMode.Trim; set => SetMode(BarResizeMode.Trim, value); }
        [JsonIgnore]
        public bool Mode_Overlap { get => _mode is BarResizeMode.Overlap; set => SetMode(BarResizeMode.Overlap, value); }
        [JsonIgnore]
        public bool Mode_Stretch { get => _mode is BarResizeMode.Stretch; set => SetMode(BarResizeMode.Stretch, value); }
        [JsonIgnore]
        public bool Mode_Slide { get => _mode is BarResizeMode.Slide; set => SetMode(BarResizeMode.Slide, value); }

        private void SetMode(BarResizeMode mode, bool value)
        {
            if (value)
            {
                Mode = mode;
            }
        }
    }
}
