
namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe sealed partial class HevcEncodeOptions : Mpeg4EncodeOptions, ICodecOptions
    {
        public HevcProfile Profile { get; set => SetValue(ref field, value); } = HevcProfile.Main;
        public HevcLevel Level { get; set => SetValue(ref field, value); } = HevcLevel.L6_1;
        public HevcTune Tune { get; set => SetValue(ref field, value); } = HevcTune.None;
        public int[]? Pools { get; set => SetValue(ref field, value); }

        public override AVCodecID Codec => AVCodecID.AV_CODEC_ID_HEVC;

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
            if (Pools is not null)
            {
                dic["pools"] = string.Join(',', Pools);
            }
        }
    }
}
