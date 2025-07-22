using System;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.BM3
{
    public partial class ScoreOptions : ObservableObjectBase
    {
        [ObservableProperty]
        private int _resolution = RawData.DefaultResolution;
        [ObservableProperty]
        private int _format = RawData.DefaultFormat;
        [ObservableProperty]
        private string _exportDirectory = "Exported";
        [ObservableProperty]
        private bool _setupBar = false;
        [ObservableProperty]
        private BmsConvertOptions _bmsConvertOptions = new();
        [ObservableProperty]
        private PackOptions _defaultPackOptions = new();
        [ObservableProperty]
        private SliceOptions _defaultSliceOptions = new();

        public void Load(ScoreOptions source)
        {
            Resolution = source._resolution;
            Format = source._format;
            SetupBar = source._setupBar;
            ExportDirectory = source._exportDirectory;
            _bmsConvertOptions.Load(source._bmsConvertOptions);
            _defaultPackOptions.Load(source._defaultPackOptions);
            _defaultSliceOptions.Load(source._defaultSliceOptions);
        }
    }
}
