using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IVideoDecoder : IVideoContext, IMediaDecoder
    {
        /// <summary>
        /// Approximate output frame count.
        /// </summary>
        long TotalFrame { get; }

        /// <summary>
        /// Decode one frame to codec context.<br/>
        /// Decoder may skip frames that are not a keyframe and whose dts(decompression timestamp) is less than <paramref name="requiredTime"/>.<br/>
        /// If hardware decoding is enabled, the decoder ignores <paramref name="requiredTime"/>.
        /// </summary>
        /// <param name="requiredTime">Minimum timestamp required (in seconds)<br/>
        /// If hardware decoding is enabled, the decoder ignores this.</param>
        /// <returns>
        /// <see cref="bool">true</see> if a frame decoded; otherwise <see cref="bool">false</see>(basically it means the end of stream).
        /// </returns>
        bool DecodeFrame(Rational requiredTime);

        /// <summary>
        /// Get a decoded frame from the decoder and write it to <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="pts">The frame timestamp(in seconds).</param>
        /// <param name="duration">The frame duration(in seconds).</param>
        /// <returns>
        /// <see cref="bool">true</see> if written to <paramref name="buffer"/>; otherwise <see cref="bool">false</see>(meaning the decoder has no decoded frames).
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"/>
        /// <exception cref="OutOfMemoryException"/>
        /// <exception cref="InvalidOperationException"/>
        bool GetFrame(Span<byte> buffer, out Rational pts, out Rational duration);
    }
}
