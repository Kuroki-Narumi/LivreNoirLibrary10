using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe sealed partial class HevcEncodeOptions : Mpeg4EncodeOptions, ICodecOptions
    {
        [ObservableProperty]
        private HevcProfile _profile = HevcProfile.Main;
        [ObservableProperty]
        private HevcLevel _level;
        [ObservableProperty]
        private HevcTune _tune;
        [ObservableProperty]
        private int[]? _pools;

        public override AVCodecID Codec => AVCodecID.AV_CODEC_ID_HEVC;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (_profile.TryToString(out var name))
            {
                dic["profile"] = name;
            }
            if (_level.TryToString(out name))
            {
                dic["level"] = name;
            }
            if (_tune is not 0)
            {
                dic["tune"] = _tune.ToString();
            }
            if (_pools is not null)
            {
                dic["pools"] = string.Join(',', _pools);
            }
        }
    }
}
