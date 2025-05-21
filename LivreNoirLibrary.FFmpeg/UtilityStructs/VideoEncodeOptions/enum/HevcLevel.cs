using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public enum HevcLevel
    {
        None,
        L1,
        L2,
        L2_1,
        L3,
        L3_1,
        L4,
        L4_1,
        L5,
        L5_1,
        L5_2,
        L6,
        L6_1,
        L6_2,
    }

    public static partial class FFmpegUtils
    {
        private static readonly Dictionary<HevcLevel, string> _hevc_level = new()
        {
            { HevcLevel.L1, "1" },
            { HevcLevel.L2, "2" },
            { HevcLevel.L2_1, "2.1" },
            { HevcLevel.L3, "3" },
            { HevcLevel.L3_1, "3.1" },
            { HevcLevel.L4, "4" },
            { HevcLevel.L4_1, "4.1" },
            { HevcLevel.L5, "5" },
            { HevcLevel.L5_1, "5.1" },
            { HevcLevel.L6, "6" },
            { HevcLevel.L6_1, "6.1" },
        };

        public static bool TryToString(this HevcLevel value, [MaybeNullWhen(false)] out string name) => _hevc_level.TryGetValue(value, out name);
    }
}
