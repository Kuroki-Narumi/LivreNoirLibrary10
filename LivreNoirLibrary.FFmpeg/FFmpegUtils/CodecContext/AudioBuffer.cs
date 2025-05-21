using System;
using System.IO;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public static partial class FFmpegUtils
    {
        /// <inheritdoc cref="IInternalAudioDecoder.Setup(Stream, bool, int, int, int)"/>
        /// <param name="decoder">Decoder to setup.</param>
        /// <param name="path">Fullpath of the file to decode.</param>
        internal static bool Setup<T>(this T decoder, string path, int streamIndex, int outSampleRate, int outChannels)
            where T : IAudioBufferDecoder
        {
            return decoder.Setup(File.OpenRead(path), false, streamIndex, outSampleRate, outChannels);
        }

        /// <inheritdoc cref="IAudioBufferDecoder.UnsafeSampleSeek(long)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SampleSeekCore<T>(this T decoder, long position)
            where T : IAudioBufferDecoder, IAudioDecoder
        {
            var len = decoder.SampleLength;
            if (position is < 0)
            {
                position += len;
            }
            var index = (int)(position - decoder.BufferHead);
            if ((uint)index < (uint)decoder.BufferLength)
            {
                decoder.BufferIndex = index * decoder.OutputChannels;
                return;
            }
            decoder.UnsafeSampleSeek(Math.Max(position, 0));
        }

        /// <inheritdoc cref="IAudioDecoder.Read"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ReadCore<T>(this T decoder, Span<float> buffer)
            where T : IAudioBufferDecoder
        {
            var channels = decoder.OutputChannels;
            var dstIndex = 0;
            var dstSize = buffer.Length;
            var bufferRead = decoder.BufferIndex;
            while (dstSize is > 0)
            {
                // バッファから読み出せる残りの量
                var remain = decoder.BufferLength * channels - bufferRead;
                if (remain is <= 0)
                {
                    bufferRead = 0;
                    if (decoder.UpdateBuffer())
                    {
                        break;
                    }
                    remain = decoder.BufferLength * channels;
                }
                if (dstSize <= remain)
                {
                    remain = dstSize;
                }
                decoder.Buffer.Slice(bufferRead, remain).CopyTo(buffer[dstIndex..]);
                dstIndex += remain;   // 書き込み位置(読み出されたサンプル数)
                dstSize -= remain;    // 要求読み出しサンプル数
                bufferRead += remain; // バッファの読み出し位置
            }
            decoder.BufferIndex = bufferRead;
            return dstIndex;
        }

        /// <inheritdoc cref="IAudioEncoder.Write"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void WriteCore<T>(this T encoder, ReadOnlySpan<float> buffer)
            where T : IAudioBufferEncoder
        {
            var channels = encoder.InputChannels;
            var srcIndex = 0;
            var srcSize = buffer.Length;
            while (srcSize is > 0)
            {
                var bufferWrote = encoder.BufferIndex;
                // バッファに書き込める残りの量
                var remain = encoder.BufferLength * channels - bufferWrote;
                if (remain is <= 0)
                {
                    encoder.EncodeBuffer();
                    bufferWrote = 0;
                    remain = encoder.BufferLength * channels;
                }
                if (srcSize <= remain)
                {
                    remain = srcSize;
                }
                buffer.Slice(srcIndex, remain).CopyTo(encoder.Buffer[bufferWrote..]);
                srcIndex += remain; // 読み込み位置(書き込まれたサンプル数)
                srcSize -= remain;  // 要求書き込みサンプル数
                encoder.BufferIndex = bufferWrote + remain; // バッファの書き込み位置
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FlushCore<T>(this T encoder)
            where T : IAudioBufferEncoder
        {
            while (encoder.EncodeBuffer()) ;
        }
    }
}
