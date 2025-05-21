using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public enum AVDictMode
    {
        None = 0,
        DontOverwrite = ffmpeg.AV_DICT_DONT_OVERWRITE,
        Append = ffmpeg.AV_DICT_APPEND,
        MultiKey = ffmpeg.AV_DICT_MULTIKEY,
    }
}
