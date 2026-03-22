using System;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IAudioBuffer
    {
        /// <summary>
        /// Gets the total number of samples in the buffer.
        /// </summary>
        /// <remarks>
        /// To get the number of samples per channel, divide this value by <see cref="Channels"/>.
        /// </remarks>
        int TotalSample { get; }

        /// <summary>
        /// Gets the sample rate in the buffer.
        /// </summary>
        int SampleRate { get; }

        /// <summary>
        /// Gets the number of audio channels in the buffer.
        /// </summary>
        int Channels { get; }
    }
}
