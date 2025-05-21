using System;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public abstract unsafe class VideoContext(FormatContext baseContext) : CodecContext(baseContext), IVideoContext
    {
        public const AVPixelFormat InternalPixelFormat = AVPixelFormat.AV_PIX_FMT_BGRA;

        private SwsContext* _sws_context;
        protected int _in_width;
        protected int _in_height;
        private AVPixelFormat _in_format;
        protected int _out_width;
        protected int _out_height;
        private AVPixelFormat _out_format;

        private readonly byte*[] _frame_slice = new byte*[8];
        private readonly int[] _frame_stride = new int[8];
        private readonly byte*[] _buffer_slice = new byte*[4];
        private readonly int[] _buffer_stride = new int[4];

        protected Rational _frame_rate;

        public int InputWidth => _in_width;
        public int InputHeight => _in_height;
        public int OutputWidth => _out_width;
        public int OutputHeight => _out_height;
        public Rational FrameRate => _frame_rate;

        protected SwsContext* EnsureSwsContext(int inWidth, int inHeight, AVPixelFormat inFormat, int outWidth, int outHeight, AVPixelFormat outFormat)
        {
            if (_sws_context is null ||
                _in_width !=  inWidth || _in_height != inHeight || _in_format != inFormat ||
                _out_width !=  outWidth || _out_height !=  outHeight || _out_format != outFormat)
            {
                _sws_context = ffmpeg.sws_getCachedContext(_sws_context, inWidth, inHeight, inFormat, outWidth, outHeight, outFormat, ffmpeg.SWS_BICUBIC, null, null, null);
                if (_sws_context is null)
                {
                    FFmpegUtils.ThrowOutOfMemoryException("Failed to allocate sws context.");
                }
                _in_width = inWidth;
                _in_height = inHeight;
                _in_format = inFormat;
                _out_width = outWidth;
                _out_height = outHeight;
                _out_format = outFormat;
            }
            return _sws_context;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected (byte*[] FrameSlices, int[] FrameStrides, byte*[] BufferSlices, int[] BufferStrides) GetSlices(AVFrame* frame, byte* ptr, int width)
        {
            var esl = _frame_slice;
            var est = _frame_stride;
            for (var i = 0u; i < 8u; i++)
            {
                esl[i] = frame->data[i];
                est[i] = frame->linesize[i];
            }
            var bsl = _buffer_slice;
            var bst = _buffer_stride;
            bsl[0] = ptr;
            bst[0] = width * 4;
            return (esl, est, bsl, bst);
        }

        protected override void DisposeUnmanaged()
        {
            if (_sws_context is not null)
            {
                ffmpeg.sws_freeContext(_sws_context);
                _sws_context = null;
            }
            base.DisposeUnmanaged();
        }
    }
}
