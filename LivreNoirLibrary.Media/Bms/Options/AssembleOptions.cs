using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class AssembleOptions : ObservableObjectBase
    {
        [ObservableProperty(Related = [nameof(Mode_Entire), nameof(Mode_Selection), nameof(Mode_Preview)])]
        private AssembleMode _mode = AssembleMode.Entire;
        [ObservableProperty]
        private ConvertTarget _target = new();
        [ObservableProperty]
        private bool _isFlowEnabled = false;
        [ObservableProperty(Related = [nameof(RandomMode_Auto), nameof(RandomMode_Seed), nameof(RandomMode_Manual), nameof(RandomMode_Ignore)])]
        private RandomProvideMode _randomMode = RandomProvideMode.Manual;
        [ObservableProperty]
        private int _randomSeed = 0;

        [ObservableProperty]
        private bool _adjust = true;

        [ObservableProperty(Related = [nameof(ReplaceMode_None), nameof(ReplaceMode_Selection), nameof(ReplaceMode_All)])]
        private AssembleReplaceMode _replaceMode = AssembleReplaceMode.None;
        [ObservableProperty]
        private int _replaceMargin = 0;

        [JsonIgnore]
        [ObservableProperty]
        private BarPosition _previewStart;
        [ObservableProperty]
        private decimal _previewFadeIn = 1;
        [ObservableProperty]
        private decimal _previewBody = 19;
        [ObservableProperty]
        private decimal _previewFadeOut = 2;
        [ObservableProperty]
        private bool _setPreview;

        [ObservableProperty]
        private double _gain = 0;
        [ObservableProperty]
        private bool _normalize = false;
        [ObservableProperty]
        private bool _playLongEnd = false;
        [ObservableProperty]
        private bool _overlap = false;
        [ObservableProperty]
        private bool _marker = true;

        [ObservableProperty]
        private SampleFormat _format;
        [ObservableProperty]
        private bool _openWave = false;

        [JsonIgnore]
        public bool Mode_Entire { get => _mode is AssembleMode.Entire; set => SetMode(AssembleMode.Entire, value); }
        [JsonIgnore]
        public bool Mode_Selection { get => _mode is AssembleMode.Selection; set => SetMode(AssembleMode.Selection, value); }
        [JsonIgnore]
        public bool Mode_Preview { get => _mode is AssembleMode.Preview; set => SetMode(AssembleMode.Preview, value); }

        public bool RandomMode_Auto { get => _randomMode is RandomProvideMode.Auto; set => SetRandomMode(RandomProvideMode.Auto, value); }
        [JsonIgnore]
        public bool RandomMode_Seed { get => _randomMode is RandomProvideMode.Seed; set => SetRandomMode(RandomProvideMode.Seed, value); }
        [JsonIgnore]
        public bool RandomMode_Manual { get => _randomMode is RandomProvideMode.Manual; set => SetRandomMode(RandomProvideMode.Manual, value); }
        [JsonIgnore]
        public bool RandomMode_Ignore { get => _randomMode is RandomProvideMode.Ignore; set => SetRandomMode(RandomProvideMode.Ignore, value); }

        [JsonIgnore]
        public bool ReplaceMode_None { get => _replaceMode is AssembleReplaceMode.None; set => SetReplaceMode(AssembleReplaceMode.None, value); }
        [JsonIgnore]
        public bool ReplaceMode_Selection { get => _replaceMode is AssembleReplaceMode.Selection; set => SetReplaceMode(AssembleReplaceMode.Selection, value); }
        [JsonIgnore]
        public bool ReplaceMode_All { get => _replaceMode is AssembleReplaceMode.All; set => SetReplaceMode(AssembleReplaceMode.All, value); }

        public void EnsureMode(bool isSelectionEnabled)
        {
            if (isSelectionEnabled)
            {
                Mode = AssembleMode.Selection;
            }
            else if (Mode_Selection && !isSelectionEnabled)
            {
                Mode = AssembleMode.Entire;
            }
            _target.IsSelectionEnabled = isSelectionEnabled;
        }

        private void SetMode(AssembleMode mode, bool value)
        {
            if (value)
            {
                Mode = mode;
            }
        }

        private void SetReplaceMode(AssembleReplaceMode mode, bool value)
        {
            if (value)
            {
                ReplaceMode = mode;
            }
        }

        private void SetRandomMode(RandomProvideMode mode, bool value)
        {
            if (value)
            {
                RandomMode = mode;
            }
        }
    }

    public enum AssembleMode
    {
        Entire,
        Selection,
        Preview,
    }

    public enum RandomProvideMode
    {
        Auto,
        Seed,
        Manual,
        Ignore,
    }

    public enum AssembleReplaceMode
    {
        None,
        Selection,
        All,
    }
}
