using System;
using LivreNoirLibrary.Numerics;
using static LivreNoirLibrary.Media.FFmpeg.FFmpegUtils;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class FFmpegMediaExtensions
    {
        /// <summary>
        /// Decode the input stream and write to buffer.
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        /// <param name="offset">The sample offset in <paramref name="buffer"/> at which to begin writing data from the decoder.</param>
        /// <param name="count">The maximum number of samples to read.</param>
        /// <returns>
        /// Total samples actually read(samples * channels).
        /// </returns>
        public static int Read<T>(this T decoder, Span<float> buffer, int offset, int count)
            where T : IAudioDecoder
        {
            CheckIndexRange(buffer, offset, count);
            return decoder.Read(buffer.Slice(offset, count));
        }

        /// <summary>
        /// Decode the input stream and write to buffer.
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        /// <param name="offset">The byte offset in <paramref name="buffer"/> at which to begin writing data from the decoder.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns>
        /// Total bytes actually read.
        /// </returns>
        public static int Read<T>(this T decoder, Span<byte> buffer, int offset, int count)
            where T : IAudioDecoder
        {
            CheckIndexRange(buffer, offset, count);
            fixed (byte* bufferPtr = buffer)
            {
                var floatSpan = new Span<float>(bufferPtr + offset, count / sizeof(float));
                var read = decoder.Read(floatSpan);
                return read * sizeof(float);
            }
        }

        /// <summary>
        /// Decode the rest of the input stream. The stream position may be set to the end.
        /// </summary>
        /// <returns>
        /// Decoded samples.
        /// </returns>
        public static float[] ReadAllSamples<T>(this T decoder)
            where T : IAudioDecoder
        {
            var length = decoder.SampleLength - decoder.SamplePosition;
            if (length is <= 0)
            {
                return [];
            }
            var buffer = new float[length * decoder.OutputChannels];
            decoder.Read(buffer);
            return buffer;
        }

        /// <inheritdoc cref="IMediaDecoder.Seek(Rational)"/>
        public static void SeekCore<T>(this T decoder, Rational position)
            where T : IAudioDecoder
        {
            decoder.SampleSeek(position.Floor(decoder.OutputSampleRate));
        }

        /// <summary>
        /// <inheritdoc cref="IAudioEncoder.Write(ReadOnlySpan{float})"/>
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        /// <param name="offset">The sample offset in <paramref name="buffer"/> at which to begin writing data to the encoder.</param>
        /// <param name="count">Number of samples to write.</param>
        public static void Write<T>(this T encoder, ReadOnlySpan<float> buffer, int offset, int count)
            where T : IAudioEncoder
        {
            CheckIndexRange(buffer, offset, count);
            encoder.Write(buffer.Slice(offset, count));
        }

        /// <summary>
        /// <inheritdoc cref="IAudioEncoder.Write(ReadOnlySpan{float})"/>
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        public static void Write<T>(this T encoder, ReadOnlySpan<byte> buffer)
            where T : IAudioEncoder
        {
            fixed (byte* bufferPtr = buffer)
            {
                var floatSpan = new Span<float>(bufferPtr, buffer.Length / sizeof(float));
                encoder.Write(floatSpan);
            }
        }

        /// <summary>
        /// <inheritdoc cref="IAudioEncoder.Write(ReadOnlySpan{float})"/>
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        /// <param name="offset">The byte offset in <paramref name="buffer"/> at which to begin writing data to the encoder.</param>
        /// <param name="count">Number of bytes to write.</param>
        public static void Write<T>(this T encoder, ReadOnlySpan<byte> buffer, int offset, int count)
            where T : IAudioEncoder
        {
            CheckIndexRange(buffer, offset, count);
            Write(encoder, buffer.Slice(offset, count));
        }

        /// <summary>
        /// Decode one frame and write it to <paramref name="buffer"/>.<br/>
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="pts">The timestamp(in seconds) of the decoded frame.</param>
        /// <returns>
        /// <see cref="bool">true</see> if written to <paramref name="buffer"/>; otherwise <see cref="bool">false</see>(basically it means the end of stream).
        /// </returns>
        public static bool ReadOneFrame<T>(this T decoder, Span<byte> buffer, out Rational pts, out Rational duration)
            where T : IVideoDecoder
        {
            while (true)
            {
                if (decoder.GetFrame(buffer, out pts, out duration))
                {
                    return true;
                }
                if (!decoder.DecodeFrame(default))
                {
                    return false;
                }
            }
        }
    }
}
