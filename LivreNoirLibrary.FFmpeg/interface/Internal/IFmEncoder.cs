using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.FFmpeg
{
    internal unsafe interface IFmEncoder
    {
        AVFormatContext* FormatContext { get; }

        bool GetEncodeOptions([MaybeNullWhen(false)] out Dictionary<string, string?> options)
        {
            options = null;
            return false;
        }

        void SetupEncoder(AVCodec* codec, AVCodecContext* codecContext);
    }
}
