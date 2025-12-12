using System;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public static unsafe partial class FFmpegUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AVChannelLayout CreateChannelLayout(int channels)
        {
            AVChannelLayout layout = default;
            ffmpeg.av_channel_layout_default(&layout, channels);
            return layout;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SwrContext* CreateSwrContext(
            int dstChannel, AVSampleFormat dstFormat, int dstSampleRate,
            int srcChannel, AVSampleFormat srcFormat, int srcSampleRate)
            => CreateSwrContext(CreateChannelLayout(dstChannel), dstFormat, dstSampleRate, CreateChannelLayout(srcChannel), srcFormat, srcSampleRate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SwrContext* CreateSwrContext(
            AVChannelLayout dstChannel, AVSampleFormat dstFormat, int dstSampleRate,
            AVChannelLayout srcChannel, AVSampleFormat srcFormat, int srcSampleRate)
        {
            SwrContext* ctx;
            ffmpeg.swr_alloc_set_opts2(&ctx,
                &dstChannel, dstFormat, dstSampleRate,
                &srcChannel, srcFormat, srcSampleRate,
                0, null).CheckError(ThrowInvalidOperationException);
            ffmpeg.swr_init(ctx).CheckError(ThrowInvalidOperationException);
            return ctx;
        }

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

        internal static SwrContext* EnsureSwrContext<T>(this T obj)
            where T : ISwrContext, IAudioContext
        {
            var ctx = obj.SwrContext;
            if (ctx is null)
            {
                obj.SwrContext = ctx = CreateSwrContext(
                    obj.OutputChannels, obj.OutputSampleFormat, obj.OutputSampleRate, 
                    obj.InputChannels, obj.InputSampleFormat, obj.InputSampleRate);
            }
            return ctx;
        }

        /// <inheritdoc cref="ffmpeg.swr_convert"/>
        /// <param name="srcBuffer">input buffers, only the first one need to be set in case of packed audio</param>
        /// <param name="srcSamples">number of input samples available in one channel</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int SwrConvertToRead<T>(this T obj, byte** srcBuffer, int srcSamples)
            where T : ISwrContext, IAudioContext
        {
            var swr = obj.EnsureSwrContext();
            var dstSamples = ffmpeg.swr_get_out_samples(swr, srcSamples);
            if (dstSamples is <= 0)
            {
                return 0;
            }
            var dstBuffer = (byte*)obj.GetConvertBuffer(dstSamples);
            var outSamples = ffmpeg.swr_convert(swr, &dstBuffer, dstSamples, srcBuffer, srcSamples).CheckError(ThrowInvalidDataException);
            return outSamples;
        }

        /// <inheritdoc cref="ffmpeg.swr_convert"/>
        /// <typeparam name="T"></typeparam>
        /// 
        /// <param name="srcSamples">number of input samples available in one channel</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int SwrConvertToWrite<T>(this T obj, float* bufferPtr, int srcSamples, byte** dstBuffer, int dstCapacity)
            where T : ISwrContext, IAudioContext
        {
            var swr = obj.EnsureSwrContext();
            var bytePtr = (byte*)bufferPtr;
            var bytePtrPtr = srcSamples is 0 ? null : &bytePtr;
            var outSamples = ffmpeg.swr_convert(swr, dstBuffer, dstCapacity, bytePtrPtr, srcSamples).CheckError(ThrowInvalidDataException);
            return outSamples;
        }
    }
}
