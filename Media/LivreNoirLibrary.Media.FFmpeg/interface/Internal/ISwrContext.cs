using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    internal unsafe interface ISwrContext
    {
        SwrContext* SwrContext { get; set; }
        AVSampleFormat InputSampleFormat { get; }
        AVSampleFormat OutputSampleFormat { get; }
        float* GetConvertBuffer(int samplesPerChannel);
    }
}
