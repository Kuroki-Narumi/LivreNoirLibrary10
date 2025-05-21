using System;
using System.Threading;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public abstract unsafe class CodecContext(FormatContext baseContext) : DisposableBase, IMetaTag
    {
        internal FormatContext _base_context = baseContext;
        internal AVStream* _stream;
        internal AVCodecContext* _codec_context;
        private AVFrame* _frame;
        private AVFrame* _hw_frame;
        protected Lock _lock = new();

        public FormatContext BaseContext => _base_context;
        public bool IsReady => FFmpegUtils.IsReady(_codec_context);
        public AVCodecID Codec => _codec_context->codec_id;

        /// <summary>
        /// Average bitrate of the codec.
        /// </summary>
        public long Bitrate => _codec_context->bit_rate;
        public bool IsHardwareAccelerated => _codec_context->hw_device_ctx is not null;

        protected virtual bool NeedsHardwareFrame => IsHardwareAccelerated;

        public override void VerifyAccess()
        {
            if (!IsReady)
            {
                FFmpegUtils.ThrowInvalidOperationException("codec context is not ready.");
            }
            base.VerifyAccess();
        }

        unsafe AVDictionary** IMetaTag.GetDictPointer()
        {
            if (_stream is not null)
            {
                return &_stream->metadata;
            }
            return null;
        }

        internal static void GetFrame(ref AVFrame* frame)
        {
            if (frame is not null)
            {
                //ffmpeg.av_frame_unref(frame);
            }
            else
            {
                frame = ffmpeg.av_frame_alloc();
            }
        }

        protected AVFrame* GetFrame()
        {
            GetFrame(ref _frame);
            return _frame;
        }

        protected AVFrame* GetHwFrame()
        {
            if (NeedsHardwareFrame)
            {
                GetFrame(ref _hw_frame);
                return _hw_frame;
            }
            return null;
        }

        internal void DisposeBuffer()
        {
            if (_frame is not null)
            {
                var frame = _frame;
                ffmpeg.av_frame_free(&frame);
                _frame = null;
            }
            if (_hw_frame is not null)
            {
                var frame = _hw_frame;
                ffmpeg.av_frame_free(&frame);
                _hw_frame = null;
            }
        }

        internal void DisposeContext()
        {
            if (_codec_context is not null)
            {
                var codecContext = _codec_context;
                ffmpeg.avcodec_free_context(&codecContext);
                _codec_context = null;
            }
        }

        protected override void DisposeUnmanaged()
        {
            DisposeBuffer();
            DisposeContext();
            base.DisposeUnmanaged();
        }
    }
}
