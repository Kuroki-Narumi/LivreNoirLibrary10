using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static FFmpeg.AutoGen.ffmpeg;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public enum HevcProfile
    {
        Main = AV_PROFILE_HEVC_MAIN,
        Main_10 = AV_PROFILE_HEVC_MAIN_10,
        Main_Still_Picture = AV_PROFILE_HEVC_MAIN_STILL_PICTURE,
        Rext = AV_PROFILE_HEVC_REXT,
        Scc = AV_PROFILE_HEVC_SCC,
    }

    public static partial class FFmpegUtils
    {
        private static readonly Dictionary<HevcProfile, string> _hevc_profiles = new()
        {
            { HevcProfile.Main, "main" },
            { HevcProfile.Main_10, "main10" },
            { HevcProfile.Main_Still_Picture, "main-still-picture" },
            { HevcProfile.Rext, "rext" },
            { HevcProfile.Scc, "scc" },
        };

        public static bool TryToString(this HevcProfile value, [MaybeNullWhen(false)]out string name) => _hevc_profiles.TryGetValue(value, out name);
    }
}
