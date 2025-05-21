using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    [Flags]
    public enum FormatFlags
    {
        None = 0,
        NoFile = ffmpeg.AVFMT_NOFILE,
        NeedNumber = ffmpeg.AVFMT_NEEDNUMBER,
        Experimental = ffmpeg.AVFMT_EXPERIMENTAL,
        ShowIds = ffmpeg.AVFMT_SHOW_IDS,
        GlobalHeader = ffmpeg.AVFMT_GLOBALHEADER,
        NoTimeStamps = ffmpeg.AVFMT_NOTIMESTAMPS,
        GenericIndex = ffmpeg.AVFMT_GENERIC_INDEX,
        TS_Discont = ffmpeg.AVFMT_TS_DISCONT,
        VariableFps = ffmpeg.AVFMT_VARIABLE_FPS,
        NoDimensions = ffmpeg.AVFMT_NODIMENSIONS,
        NoStreams = ffmpeg.AVFMT_NOSTREAMS,
        NoBinSearch = ffmpeg.AVFMT_NOBINSEARCH,
        NoGenSearch = ffmpeg.AVFMT_NOGENSEARCH,
        NoByteSeek = ffmpeg.AVFMT_NO_BYTE_SEEK,
        AllowFlush = ffmpeg.AVFMT_ALLOW_FLUSH,
        TS_NonStrict = ffmpeg.AVFMT_TS_NONSTRICT,
        TS_Negative = ffmpeg.AVFMT_TS_NEGATIVE,
        SeekToPts = ffmpeg.AVFMT_SEEK_TO_PTS,
    }
}
