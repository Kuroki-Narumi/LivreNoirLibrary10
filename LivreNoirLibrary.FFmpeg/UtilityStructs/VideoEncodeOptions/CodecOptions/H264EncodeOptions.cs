using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe sealed partial class H264EncodeOptions : Mpeg4EncodeOptions, ICodecOptions
    {
        [ObservableProperty]
        private H264Profile _profile = H264Profile.Main;
        [ObservableProperty]
        private H264Level _level;
        [ObservableProperty]
        private H264Tune _tune;

        public override AVCodecID Codec => AVCodecID.AV_CODEC_ID_H264;

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
        }
    }
}
