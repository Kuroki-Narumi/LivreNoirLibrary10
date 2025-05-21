using System;
using System.Collections.Generic;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public abstract unsafe partial class Mpeg4EncodeOptions : CodecOptionsBase, ICodecOptions
    {
        public const int DefaultMaxBFrames = 3;
        public const int MaxBFrames_Min = 0;
        public const int MaxBFrames_Max = 16;
        public const Mpeg4Preset DefaultPreset = Mpeg4Preset.medium;

        public const int DefaultCrf = 26;
        public const int Crf_Min = -1;
        public const int Crf_Max = 51;

        [ObservableProperty]
        protected int _maxBFrames = DefaultMaxBFrames;
        [ObservableProperty]
        protected Mpeg4Preset _preset = DefaultPreset;
        [ObservableProperty]
        protected int _crf = DefaultCrf;

        private static int CoerceCrf(int value) => Math.Clamp(value, Crf_Min, Crf_Max);

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (_preset is > 0)
            {
                dic["preset"] = _preset.ToString();
            }
            if (_crf is >= 0)
            {
                dic["crf"] = _crf.ToString();
            }
            if (_maxBitrate is > 0)
            {
                dic["vbv-maxrate"] = _maxBitrate.ToString();
            }
            if (_vbvBufferSize is > 0)
            {
                dic["vbv-bufsize"] = _vbvBufferSize.ToString();
            }
        }
    }
}
