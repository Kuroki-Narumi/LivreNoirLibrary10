using System;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public static unsafe partial class FFmpegUtils
    {
        internal static void DisposeSwrContext<T>(this T obj)
            where T : ISwrContext
        {
            var ctx = obj.SwrContext;
            if (ctx is not null)
            {
                ffmpeg.swr_close(ctx);
                ffmpeg.swr_free(&ctx);
                obj.SwrContext = null;
            }
        }

        internal static void AllocSwrContext<T>(this T obj,
            AVChannelLayout dstChannel, AVSampleFormat dstFormat, int dstSampleRate, 
            AVChannelLayout srcChannel, AVSampleFormat srcFormat, int srcSampleRate)
            where T : ISwrContext
        {
            DisposeSwrContext(obj);
            SwrContext* swrContext;
            ffmpeg.swr_alloc_set_opts2(&swrContext,
                &dstChannel, dstFormat, dstSampleRate,
                &srcChannel, srcFormat, srcSampleRate,
                0, null).CheckError(ThrowInvalidOperationException);
            ffmpeg.swr_init(swrContext).CheckError(ThrowInvalidOperationException);
            obj.SwrContext = swrContext;
        }

        /// <inheritdoc cref="ffmpeg.swr_convert"/>
        /// <param name="srcBuffer">input buffers, only the first one need to be set in case of packed audio</param>
        /// <param name="srcSamples">number of input samples available in one channel</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int SwrConvertToRead<T>(this T obj, byte** srcBuffer, int srcSamples)
            where T : ISwrContext
        {
            var swr = obj.SwrContext;
            var dstSamples = ffmpeg.swr_get_out_samples(swr, srcSamples);
            if (dstSamples is <= 0)
            {
                return 0;
            }
            var dstBuffer = obj.GetConvertBuffer(dstSamples);
            fixed (float* bufferPtr = dstBuffer)
            {
                var bytePtr = (byte*)bufferPtr;
                var outSamples = ffmpeg.swr_convert(swr, &bytePtr, dstSamples, srcBuffer, srcSamples).CheckError(ThrowInvalidDataException);
                return outSamples;
            }
        }

        /// <inheritdoc cref="ffmpeg.swr_convert"/>
        /// <typeparam name="T"></typeparam>
        /// 
        /// <param name="srcSamples">number of input samples available in one channel</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int SwrConvertToWrite<T>(this T obj, Span<float> src, int srcSamples, byte** dstBuffer, int dstCapacity)
            where T : ISwrContext
        {
            var swr = obj.SwrContext;
            fixed(float* bufferPtr = src)
            {
                var bytePtr = (byte*)bufferPtr;
                var bytePtrPtr = srcSamples is 0 ? null : &bytePtr;
                var outSamples = ffmpeg.swr_convert(swr, dstBuffer, dstCapacity, bytePtrPtr, srcSamples).CheckError(ThrowInvalidDataException);
                return outSamples;
            }
        }
    }
}
