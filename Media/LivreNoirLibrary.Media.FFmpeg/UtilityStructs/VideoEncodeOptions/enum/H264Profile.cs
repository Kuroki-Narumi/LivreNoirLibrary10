using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static FFmpeg.AutoGen.ffmpeg;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public enum H264Profile
    {
        Baseline = AV_PROFILE_H264_BASELINE,
        Main = AV_PROFILE_H264_MAIN,
        High = AV_PROFILE_H264_HIGH,
        Cavlc44 = AV_PROFILE_H264_CAVLC_444,
        Constrained = AV_PROFILE_H264_CONSTRAINED,
        Constrained_Baseline = AV_PROFILE_H264_CONSTRAINED_BASELINE,
        Extended = AV_PROFILE_H264_EXTENDED,
        High_10 = AV_PROFILE_H264_HIGH_10,
        High_10_Intra = AV_PROFILE_H264_HIGH_10_INTRA,
        High_422 = AV_PROFILE_H264_HIGH_422,
        High_422_Intra = AV_PROFILE_H264_HIGH_422_INTRA,
        High_444 = AV_PROFILE_H264_HIGH_444,
        High_444_Intra = AV_PROFILE_H264_HIGH_444_INTRA,
        High_444_Predictive = AV_PROFILE_H264_HIGH_444_PREDICTIVE,
        Intra = AV_PROFILE_H264_INTRA,
        MultiView_High = AV_PROFILE_H264_MULTIVIEW_HIGH,
        Stereo_High = AV_PROFILE_H264_STEREO_HIGH,
    }

    public static partial class FFmpegUtils
    {
        private static readonly Dictionary<H264Profile, string> _h264_profiles = new()
        {
            { H264Profile.Baseline, "baseline" },
            { H264Profile.Main, "main" },
            { H264Profile.High, "high" },
        };

        public static bool TryToString(this H264Profile value, [MaybeNullWhen(false)] out string name) => _h264_profiles.TryGetValue(value, out name);
    }
}
