
namespace LivreNoirLibrary.Media.FFmpeg
{
    public abstract unsafe partial class Mpeg4EncodeOptions : CodecOptionsBase, ICodecOptions
    {
        public const int DefaultMaxBFrames = 3;
        public const int MaxBFrames_Min = 0;
        public const int MaxBFrames_Max = 16;
        public const Mpeg4Preset DefaultPreset = Mpeg4Preset.slow;

        public const int DefaultCrf = 26;
        public const int Crf_Min = -1;
        public const int Crf_Max = 51;

        public int MaxBFrames { get; set => SetValue(ref field, Math.Clamp(value, MaxBFrames_Min, MaxBFrames_Max)); } = DefaultMaxBFrames;
        public Mpeg4Preset Preset { get; set => SetValue(ref field, value); } = DefaultPreset;
        public int Crf { get; set => SetValue(ref field, Math.Clamp(value, Crf_Min, Crf_Max)); } = DefaultCrf;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (Preset is > 0)
            {
                dic["preset"] = Preset.ToString();
            }
            if (Crf is >= 0)
            {
                dic["crf"] = Crf.ToString();
            }
            if (MaxBitrate is > 0)
            {
                dic["vbv-maxrate"] = MaxBitrate.ToString();
            }
            if (VbvBufferSize is > 0)
            {
                dic["vbv-bufsize"] = VbvBufferSize.ToString();
            }
        }

        public abstract bool EnsureLevel(int width, int height, double fps, int kbps);
    }
}
