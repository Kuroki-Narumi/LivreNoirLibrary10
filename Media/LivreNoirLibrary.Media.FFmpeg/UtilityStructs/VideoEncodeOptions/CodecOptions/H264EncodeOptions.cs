
namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe sealed partial class H264EncodeOptions : Mpeg4EncodeOptions, ICodecOptions
    {
        public H264Profile Profile { get; set => SetValue(ref field, value); } = H264Profile.Main;
        public H264Level Level { get; set => SetValue(ref field, value); } = H264Level.L5_1;
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

        private readonly (H264Level Level, int MaxFs, long MaxMbps, int MaxBr)[] _levels = 
        [
            (H264Level.L1,      99,     1485,      64),
            (H264Level.L1_1,   396,     3000,     192),
            (H264Level.L1_2,   396,     6000,     384),
            (H264Level.L1_3,   396,    11880,     768),
            (H264Level.L2,     396,    11880,    2000),
            (H264Level.L2_1,   792,    19800,    4000),
            (H264Level.L2_2,  1620,    20250,    4000),
            (H264Level.L3,    1620,    40500,   10000),
            (H264Level.L3_1,  3600,   108000,   14000),
            (H264Level.L3_2,  5120,   216000,   20000),
            (H264Level.L4,    8192,   245760,   20000),
            (H264Level.L4_1,  8192,   245760,   50000),
            (H264Level.L4_2,  8704,   522240,   50000),
            (H264Level.L5,   22080,   589824,  135000),
            (H264Level.L5_1, 36864,   983040,  240000),
        ];

        public override bool EnsureLevel(int width, int height, double fps, int kbps)
        {
            if (width is <= 0 || height is <= 0 || fps is <= 0)
            {
                return false;
            }
            var macroblocks = (((long)width + 15) / 16) * (((long)height + 15) / 16);
            var requiredMBPS = (long)Math.Ceiling(macroblocks * fps);
            foreach (var (level, maxFs, maxMbps, maxBr) in _levels)
            {
                if (macroblocks <= maxFs && 
                    requiredMBPS <= maxMbps && 
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
