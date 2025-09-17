using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe readonly struct StreamInfo : IMetaTag
    {
        internal readonly AVStream* _stream;

        public int Index => _stream->index;
        public AVMediaType MediaType => _stream->codecpar->codec_type;

        internal StreamInfo(AVStream* stream)
        {
            _stream = stream;
        }

        AVDictionary** IMetaTag.GetDictPointer()
        {
            if (_stream is not null)
            {
                return &_stream->metadata;
            }
            return null;
        }
    }
}
