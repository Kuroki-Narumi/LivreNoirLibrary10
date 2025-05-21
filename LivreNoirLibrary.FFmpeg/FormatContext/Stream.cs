using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe partial class FormatContext
    {
        internal bool TryGetStream([MaybeNullWhen(false)]out AVStream* stream, AVMediaType type, int index = -1)
        {
            var fmt = _format_context;
            if (fmt is not null)
            {
                var count = fmt->nb_streams;
                if ((uint)index < count)
                {
                    var s = fmt->streams[index];
                    if (type == s->codecpar->codec_type)
                    {
                        stream = s;
                        return true;
                    }
                }
                else
                {
                    for (var i = 0u; i < count; i++)
                    {
                        var s = fmt->streams[i];
                        if (type == s->codecpar->codec_type)
                        {
                            stream = s;
                            return true;
                        }
                    }
                }
            }
            stream = null;
            return false;
        }

        public bool TryGetStreamInfo(int index, out StreamInfo info)
        {
            var fmt = _format_context;
            if (fmt is not null)
            {
                var count = fmt->nb_streams;
                if ((uint)index < count)
                {
                    info = new(fmt->streams[index]);
                    return true;
                }
            }
            info = default;
            return false;
        }

        public StreamInfo[] GetStreamInfo()
        {
            var fmt = _format_context;
            if (fmt is null)
            {
                return [];
            }
            var count = fmt->nb_streams;
            var result = new StreamInfo[count];
            for (var i = 0u; i < count; i++)
            {
                var s = fmt->streams[i];
                result[i] = new(s);
            }
            return result;
        }

        public StreamInfoEnumerable EnumStreamInfo(AVMediaType type = AVMediaType.AVMEDIA_TYPE_UNKNOWN) => new(_format_context, type);

        public static IEnumerable<StreamInfo> EnumStreamInfo(string path, AVMediaType type = AVMediaType.AVMEDIA_TYPE_UNKNOWN)
        {
            using var ctx = OpenRead(path);
            foreach (var info in ctx.EnumStreamInfo(type))
            {
                yield return info;
            }
        }
    }
}
