using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.BM3
{
    public partial class SliceOptions : ObservableObjectBase
    {
        [ObservableProperty]
        private bool _adjust = true;
        [ObservableProperty]
        private long _offset = -100;
        [ObservableProperty]
        private bool _moveToMinimum = false;
        [ObservableProperty]
        private string _basename = PackUtils.DefaultFormat_Slice;
        [ObservableProperty]
        private double _cutoffLeft = -180;
        [ObservableProperty]
        private long _marginLeft = 0;
        [ObservableProperty]
        private long _fadeIn = 0;
        [ObservableProperty]
        private double _cutoffRight = -60;
        [ObservableProperty]
        private long _marginRight = 22;
        [ObservableProperty]
        private long _fadeOut = 22;
        [ObservableProperty]
        private long _crossFade = 40;
        [ObservableProperty]
        private SampleFormat _format = SampleFormat.Invalid;

        [JsonIgnore]
        public string BasenameWithDefault => string.IsNullOrEmpty(_basename) ? PackUtils.DefaultFormat_Filename : _basename;

        public void Load(SliceOptions source)
        {
            Adjust = source._adjust;
            Offset = source._offset;
            MoveToMinimum = source._moveToMinimum;
            Basename = source._basename;
            CutoffLeft = source._cutoffLeft;
            MarginLeft = source._marginLeft;
            FadeIn = source._fadeIn;
            CutoffRight = source._cutoffRight;
            MarginRight = source._marginRight;
            FadeOut = source._fadeOut;
            CrossFade = source._crossFade;
            Format = source._format;
        }
    }
}
