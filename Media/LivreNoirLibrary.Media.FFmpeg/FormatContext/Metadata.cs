using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe partial class FormatContext : IMetaTag
    {
        AVDictionary** IMetaTag.GetDictPointer()
        {
            if (_format_context is not null)
            {
                return &_format_context->metadata;
            }
            FFmpegUtils.ThrowInvalidOperationException("format context is not opened.");
            return null;
        }
    }
}
