using System;
using System.Collections.Generic;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe sealed partial class Mpeg1EncodeOptions : CodecOptionsBase, ICodecOptions
    {
        public const int DefaultSlices = 1;
        public const Mpeg1BitrateMode DefaultBitrateMode = Mpeg1BitrateMode.vbr;

        [ObservableProperty]
        private int _slices = DefaultSlices;
        [ObservableProperty]
        private Mpeg1BitrateMode _bitrateMode = DefaultBitrateMode;

        public override AVCodecID Codec => AVCodecID.AV_CODEC_ID_MPEG1VIDEO;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            dic["slices"] = _slices.ToString();
            if (_bitrateMode is not 0)
            {
                dic["rc"] = _bitrateMode.ToString();
            }
            if (_maxBitrate is > 0)
            {
                dic["rc_max_rate"] = _maxBitrate.ToString();
            }
            if (_vbvBufferSize is > 0)
            {
                dic["rc_buffer_size"] = _vbvBufferSize.ToString();
            }
        }
    }
}
