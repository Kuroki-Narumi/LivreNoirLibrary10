
namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe sealed partial class HevcEncodeOptions : Mpeg4EncodeOptions, ICodecOptions
    {
        public HevcProfile Profile { get; set => SetValue(ref field, value); } = HevcProfile.Main;
        public HevcLevel Level { get; set => SetValue(ref field, value); } = HevcLevel.L5_1;
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

        private readonly (HevcLevel Level, long MaxPicSize, long MaxLumaSr, int MaxBr)[] _levels =
        [
            (HevcLevel.L1,     36864,      552960,     128),
            (HevcLevel.L2,     73728,     3686400,    1500),
            (HevcLevel.L2_1,  122880,     7372800,    3000),
            (HevcLevel.L3,    245760,    16588800,    6000),
            (HevcLevel.L3_1,  552960,    33177600,   10000),
            (HevcLevel.L4,    983040,    66846720,   12000),
            (HevcLevel.L4_1,  983040,   133693440,   20000),
            (HevcLevel.L5,   2228224,   267386880,   25000),
            (HevcLevel.L5_1, 2228224,   534773760,   40000),
            (HevcLevel.L5_2, 2228224,  1069547520,   60000),
            (HevcLevel.L6,   8912896,  1069547520,   60000),
            (HevcLevel.L6_1, 8912896,  2139095040,  120000),
            (HevcLevel.L6_2, 8912896,  4278190080,  240000),
        ];

        public override bool EnsureLevel(int width, int height, double fps, int kbps)
        {
            if (width is <= 0 || height is <= 0 || fps is <= 0)
            {
                return false;
            }
            var lumaSamples = (long)width * height;
            var requiredMBPS = (long)Math.Ceiling(lumaSamples * fps);
            foreach (var (level, maxPicSize, maxLumaSr, maxBr) in _levels)
            {
                if (lumaSamples <= maxPicSize &&
                    requiredMBPS <= maxLumaSr &&
                    kbps <= maxBr)
                {
                    Level = level;
                    return true;
                }
            }
            return false;
        }
    }
}
