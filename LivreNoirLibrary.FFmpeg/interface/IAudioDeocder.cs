using System;

namespace LivreNoirLibrary.Media
{
    public interface IAudioDecoder : IAudioContext, IMediaDecoder, IDisposable
    {
        /// <summary>
        /// Approximate output sample length(samples per channel).
        /// </summary>
        long SampleLength { get; }
        /// <summary>
        /// Approximate output total sample(samples * channels).
        /// </summary>
        long TotalSample => SampleLength * OutputChannels;
        /// <summary>
        /// Current output position(samples per channel).
        /// </summary>
        long SamplePosition { get; }
        /// <summary>
        /// Try to seek the base stream to the specified position(approx. output samples per channel).
        /// </summary>
        /// <param name="position">Required position(samples per channel)</param>
        void SampleSeek(long position);
        /// <summary>
        /// Decode the input stream and write to <paramref name="buffer"/>. The decoder attempts to fill the entire <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        /// <returns>
        /// Total samples actually read(samples * channels). <br/>
        /// If there's enough of the input stream remaining: the same as the <paramref name="buffer"/> length, otherwise: the total samples to the end of the stream.
        /// </returns>
        int Read(Span<float> buffer);
    }
}
