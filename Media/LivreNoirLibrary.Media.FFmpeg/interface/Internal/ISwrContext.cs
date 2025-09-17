using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    internal unsafe interface ISwrContext
    {
        SwrContext* SwrContext { get; set; }
        Span<float> GetConvertBuffer(int samplesPerChannel);
    }
}
