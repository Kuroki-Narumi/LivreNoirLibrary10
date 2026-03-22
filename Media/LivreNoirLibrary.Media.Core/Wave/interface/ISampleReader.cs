using System;

namespace LivreNoirLibrary.Media.Wave
{
    public interface ISampleReader : IAudioBuffer
    {
        /// <summary>
        /// Gets the current position of the sample within the data stream.
        /// </summary>
        int SamplePosition { get; }

        /// <summary>
        /// Seeks to the specified sample position within the audio stream.
        /// </summary>
        /// <param name="samplePosition">
        /// The zero-based index of the sample position to seek to. 
        /// Must be a non-negative integer within the bounds of the audio data.
        /// </param>
        /// <returns>true if the seek operation was successful; otherwise, false.</returns>
        bool SampleSeek(int samplePosition);

        /// <summary>
        /// Reads a sequence of audio samples into the specified span.
        /// </summary>
        /// <remarks>
        /// This method may return fewer samples than requested if the end of the data source is reached before the span is filled.
        /// </remarks>
        /// <param name="span">The span that receives the audio samples. The span must have sufficient capacity to hold the samples being read.</param>
        /// <returns>
        /// The number of samples successfully read into the span. 
        /// This value may be less than the length of the span if the end of the data source is reached.
        /// </returns>
        int ReadSamples(Span<float> span);
    }
}
