using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Wave
{
    public class SliceOptions : ObservableObjectBase, IOptions<SliceOptions>
    {
        public bool Adjust { get; set => SetValue(ref field, value); } = true;
        public double AdjustThreshold { get; set => SetValue(ref field, value); } = -80;
        public long Offset { get; set => SetValue(ref field, value); } = -100;
        public string Basename { get; set => SetValue(ref field, value); } = PackUtils.DefaultFormat_Slice;
        public double CutoffLeft { get; set => SetValue(ref field, value); } = -80;
        public long MarginLeft { get; set => SetValue(ref field, value); }
        public long FadeIn { get; set => SetValue(ref field, value); }
        public decimal FadeInFactor { get; set => SetValue(ref field, value); } = 1;
        public double CutoffRight { get; set => SetValue(ref field, value); } = -60;
        public long MarginRight { get; set => SetValue(ref field, value); } = 22;
        public long FadeOut { get; set => SetValue(ref field, value); } = 22;
        public decimal FadeOutFactor { get; set => SetValue(ref field, value); } = 1;
        public long CrossFade { get; set => SetValue(ref field, value); } = 200;
        public SampleFormat Format { get; set => SetValue(ref field, value); } = SampleFormat.Invalid;

        [JsonIgnore]
        public string BasenameWithDefault => string.IsNullOrEmpty(Basename) ? PackUtils.DefaultFormat_Filename : Basename;

        public void Load(SliceOptions source)
        {
            Adjust = source.Adjust;
            AdjustThreshold = source.AdjustThreshold;
            Offset = source.Offset;
            Basename = source.Basename;
            CutoffLeft = source.CutoffLeft;
            MarginLeft = source.MarginLeft;
            FadeIn = source.FadeIn;
            FadeInFactor = source.FadeInFactor;
            CutoffRight = source.CutoffRight;
            MarginRight = source.MarginRight;
            FadeOut = source.FadeOut;
            FadeOutFactor = source.FadeOutFactor;
            CrossFade = source.CrossFade;
            Format = source.Format;
        }
    }
}
