using System;

namespace LivreNoirLibrary.Media
{
    internal interface IAudioBuffer : IAudioContext
    {
        Span<float> Buffer { get; }
        /// <summary>
        /// Current buffer size(samples per channel).
        /// </summary>
        int BufferLength { get; }
        /// <summary>
        /// Number of total samples read from/wrote to the buffer.
        /// </summary>
        int BufferIndex { get; set; }
    }
}
