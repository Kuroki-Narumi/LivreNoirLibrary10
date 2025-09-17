using LivreNoirLibrary.ObjectModel;
using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Integrated
{
    public class BmsConvertOptions : ObservableObjectBase, IOptions<BmsConvertOptions>
    {
        public string Filename { get => field; set => SetValue(ref field, value); } = PackUtils.ExportFormat_Filename;
        public string Genre { get => field; set => SetValue(ref field, value); } = "";
        public string Title { get => field; set => SetValue(ref field, value); } = PackUtils.ExportFormat_Title;
        public string Artist { get => field; set => SetValue(ref field, value); } = PackUtils.ExportFormat_Copyright;
        public int LaneStart { get => field; set => SetValue(ref field, value); }
        public int DefStart { get => field; set => SetValue(ref field, value); } = 1;
        public int DefInterval { get => field; set => SetValue(ref field, value); }

        [JsonIgnore]
        public string FilenameWithDefault => string.IsNullOrEmpty(Filename) ? PackUtils.ExportFormat_Filename : Filename;

        public void Load(BmsConvertOptions source)
        {
            Filename = source.Filename;
            Genre = source.Genre;
            Title = source.Title;
            Artist = source.Artist;
            LaneStart = source.LaneStart;
            DefStart = source.DefStart;
            DefInterval = source.DefInterval;
        }
    }
}
