using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.BM3
{
    public partial class BmsConvertOptions : ObservableObjectBase
    {
        [ObservableProperty]
        private string _filename = PackUtils.ExportFormat_Filename;
        [ObservableProperty]
        private string _genre = "";
        [ObservableProperty]
        private string _title = PackUtils.ExportFormat_Title;
        [ObservableProperty]
        private string _artist = PackUtils.ExportFormat_Copyright;
        [ObservableProperty]
        private int _laneStart = 0;
        [ObservableProperty]
        private int _defStart = 1;
        [ObservableProperty]
        private int _defInterval = 0;

        public void Load(BmsConvertOptions source)
        {
            Filename = source._filename;
            Genre = source._genre;
            Title = source._title;
            Artist = source._artist;
            LaneStart = source._laneStart;
            DefStart = source._defStart;
            DefInterval = source._defInterval;
        }
    }
}
