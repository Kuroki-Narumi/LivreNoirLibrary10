
namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe sealed partial class H264EncodeOptions : Mpeg4EncodeOptions, ICodecOptions
    {
        public H264Profile Profile { get; set => SetValue(ref field, value); } = H264Profile.Main;
        public H264Level Level { get; set => SetValue(ref field, value); } = H264Level.L4_1;
        public H264Tune Tune { get; set => SetValue(ref field, value); } = H264Tune.None;

        public override AVCodecID Codec => AVCodecID.AV_CODEC_ID_H264;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (Profile.TryToString(out var name))
            {
                dic["profile"] = name;
            }
            if (Level.TryToString(out name))
            {
                dic["level"] = name;
            }
            if (Tune is not 0)
            {
                dic["tune"] = Tune.ToString();
            }
        }
    }
}
