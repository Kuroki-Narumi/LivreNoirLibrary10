using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public enum H264Level
    {
        None,
        L1,
        L1b,
        L1_1,
        L1_2,
        L1_3,
        L2,
        L2_1,
        L2_2,
        L3,
        L3_1,
        L3_2,
        L4,
        L4_1,
        L4_2,
        L5,
        L5_1,
    }

    public static partial class FFmpegUtils
    {
        private static readonly Dictionary<H264Level, string> _h264_level = new()
        {
            { H264Level.L1, "1" },
            { H264Level.L1b, "1b" },
            { H264Level.L1_1, "1.1" },
            { H264Level.L1_2, "1.2" },
            { H264Level.L1_3, "1.3" },
            { H264Level.L2, "2" },
            { H264Level.L2_1, "2.1" },
            { H264Level.L2_2, "2.2" },
            { H264Level.L3, "3" },
            { H264Level.L3_1, "3.1" },
            { H264Level.L3_2, "3.2" },
            { H264Level.L4, "4" },
            { H264Level.L4_1, "4.1" },
            { H264Level.L4_2, "4.1" },
            { H264Level.L5, "5" },
            { H264Level.L5_1, "5.1" },
        };

        public static bool TryToString(this H264Level value, [MaybeNullWhen(false)] out string name) => _h264_level.TryGetValue(value, out name);
    }
}
