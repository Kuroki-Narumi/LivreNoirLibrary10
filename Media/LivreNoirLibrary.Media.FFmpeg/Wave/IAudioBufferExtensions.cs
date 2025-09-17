using System;

namespace LivreNoirLibrary.Media.Wave
{
    public static partial class IAudioBufferExtensions
    {
        extension(IAudioBuffer buffer)
        {
            public int SampleLength => buffer.TotalSample / buffer.Channels;
            public double TotalSeconds => (double)buffer.SampleLength / buffer.SampleRate;
            public TimeSpan TotalTime => TimeSpan.FromSeconds(buffer.TotalSeconds);
        }
    }
}
