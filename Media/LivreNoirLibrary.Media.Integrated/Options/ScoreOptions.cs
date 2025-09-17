using System;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Integrated
{
    public class ScoreOptions : ObservableObjectBase, IOptions<ScoreOptions>
    {
        public int Resolution { get => field; set => SetValue(ref field, value); } = RawData.DefaultResolution;
        public int Format { get => field; set => SetValue(ref field, value); } = RawData.DefaultFormat;
        public bool SetupBarEnabled { get => field; set => SetValue(ref field, value); }
        public string ExportDirectory { get => field; set => SetValue(ref field, value); } = "Exported";
        public BmsConvertOptions BmsConvertOptions { get => field; set => SetValue(ref field, value); } = new();
        public PackOptions DefaultPackOptions { get => field; set => SetValue(ref field, value); } = new();
        public SliceOptions DefaultSliceOptions { get => field; set => SetValue(ref field, value); } = new();

        public void Load(ScoreOptions source)
        {
            Resolution = source.Resolution;
            Format = source.Format;
            SetupBarEnabled = source.SetupBarEnabled;
            ExportDirectory = source.ExportDirectory;
            BmsConvertOptions.Load(source.BmsConvertOptions);
            DefaultPackOptions.Load(source.DefaultPackOptions);
            DefaultSliceOptions.Load(source.DefaultSliceOptions);
        }
    }
}
