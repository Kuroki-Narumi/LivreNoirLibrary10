
namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe sealed partial class Mpeg1EncodeOptions : CodecOptionsBase, ICodecOptions
    {
        public const int DefaultSlices = 1;
        public const Mpeg1BitrateMode DefaultBitrateMode = Mpeg1BitrateMode.vbr;

        public int Slices { get; set => SetValue(ref field, value); } = DefaultSlices;
        public Mpeg1BitrateMode BitrateMode { get; set => SetValue(ref field, value); } = DefaultBitrateMode;

        public override AVCodecID Codec => AVCodecID.AV_CODEC_ID_MPEG1VIDEO;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            dic["slices"] = Slices.ToString();
            if (BitrateMode is not 0)
            {
                dic["rc"] = BitrateMode.ToString();
            }
            if (MaxBitrate is > 0)
            {
                dic["rc_max_rate"] = MaxBitrate.ToString();
            }
            if (VbvBufferSize is > 0)
            {
                dic["rc_buffer_size"] = VbvBufferSize.ToString();
            }
        }
    }
}
