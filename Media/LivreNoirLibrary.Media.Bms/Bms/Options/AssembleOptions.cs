using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class AssembleOptions : ObservableObjectBase
    {
        public AssembleMode Mode { get; set => SetValue(ref field, value, [nameof(Mode_Entire), nameof(Mode_Selection), nameof(Mode_Preview)]); }
        public ConvertTarget Target { get; set => SetValue(ref field, value); } = new();
        public bool IsFlowEnabled { get; set => SetValue(ref field, value); }
        public RandomProvideMode RandomMode
        {
            get;
            set => SetValue(ref field, value, [nameof(RandomMode_Auto), nameof(RandomMode_Seed), nameof(RandomMode_Manual), nameof(RandomMode_Ignore)]);
        } = RandomProvideMode.Manual;
        public int RandomSeed { get; set => SetValue(ref field, value); }

        public bool Adjust { get; set => SetValue(ref field, value); } = true;

        public AssembleReplaceMode ReplaceMode { get; set => SetValue(ref field, value, [nameof(ReplaceMode_None), nameof(ReplaceMode_Selection), nameof(ReplaceMode_All)]); }
        public int ReplaceMargin { get; set => SetValue(ref field, value); }

        [JsonIgnore]
        public BarPosition PreviewStart { get; set => SetValue(ref field, value); }
        public double PreviewFadeIn { get; set => SetValue(ref field, value); } = 1;
        public double PreviewBody { get; set => SetValue(ref field, value); } = 19;
        public double PreviewFadeOut { get; set => SetValue(ref field, value); } = 2;
        public bool SetPreview { get; set => SetValue(ref field, value); }

        public double Gain { get; set => SetValue(ref field, value); }
        public NormalizeMode NormalizeMode { get; set => SetValue(ref field, value, [nameof(NormalizeMode_None), nameof(NormalizeMode_Peak), nameof(NormalizeMode_Rms), nameof(NormalizeMode_Lufs)]); }
        public bool PlayLongEnd { get; set => SetValue(ref field, value); }
        public bool Overlap { get; set => SetValue(ref field, value); }
        public bool Marker { get; set => SetValue(ref field, value); } = true;

        public SampleFormat Format { get; set => SetValue(ref field, value); }
        public bool OpenWave { get; set => SetValue(ref field, value); }

        [JsonIgnore]
        public bool Mode_Entire { get => Mode is AssembleMode.Entire; set => SetMode(AssembleMode.Entire, value); }
        [JsonIgnore]
        public bool Mode_Selection { get => Mode is AssembleMode.Selection; set => SetMode(AssembleMode.Selection, value); }
        [JsonIgnore]
        public bool Mode_Preview { get => Mode is AssembleMode.Preview; set => SetMode(AssembleMode.Preview, value); }

        public bool RandomMode_Auto { get => RandomMode is RandomProvideMode.Auto; set => SetRandomMode(RandomProvideMode.Auto, value); }
        [JsonIgnore]
        public bool RandomMode_Seed { get => RandomMode is RandomProvideMode.Seed; set => SetRandomMode(RandomProvideMode.Seed, value); }
        [JsonIgnore]
        public bool RandomMode_Manual { get => RandomMode is RandomProvideMode.Manual; set => SetRandomMode(RandomProvideMode.Manual, value); }
        [JsonIgnore]
        public bool RandomMode_Ignore { get => RandomMode is RandomProvideMode.Ignore; set => SetRandomMode(RandomProvideMode.Ignore, value); }

        [JsonIgnore]
        public bool ReplaceMode_None { get => ReplaceMode is AssembleReplaceMode.None; set => SetReplaceMode(AssembleReplaceMode.None, value); }
        [JsonIgnore]
        public bool ReplaceMode_Selection { get => ReplaceMode is AssembleReplaceMode.Selection; set => SetReplaceMode(AssembleReplaceMode.Selection, value); }
        [JsonIgnore]
        public bool ReplaceMode_All { get => ReplaceMode is AssembleReplaceMode.All; set => SetReplaceMode(AssembleReplaceMode.All, value); }

        [JsonIgnore]
        public bool NormalizeMode_None { get => NormalizeMode is NormalizeMode.None; set => SetNormalizeMode(NormalizeMode.None, value); }
        [JsonIgnore]
        public bool NormalizeMode_Peak { get => NormalizeMode is NormalizeMode.Peak; set => SetNormalizeMode(NormalizeMode.Peak, value); }
        [JsonIgnore]
        public bool NormalizeMode_Rms { get => NormalizeMode is NormalizeMode.Rms; set => SetNormalizeMode(NormalizeMode.Rms, value); }
        [JsonIgnore]
        public bool NormalizeMode_Lufs { get => NormalizeMode is NormalizeMode.Lufs; set => SetNormalizeMode(NormalizeMode.Lufs, value); }

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
            Target.IsSelectionEnabled = isSelectionEnabled;
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

        private void SetNormalizeMode(NormalizeMode mode, bool value)
        {
            if (value)
            {
                NormalizeMode = mode;
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

    public enum NormalizeMode
    {
        None,
        Peak,
        Rms,
        Lufs,
    }
}