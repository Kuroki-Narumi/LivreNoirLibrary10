using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public readonly struct VideoEncodeOptions
    {
        public const long DefaultBitRate = 5_000_000;

        public readonly int InputWidth;
        public readonly int InputHeight;
        public readonly int OutputWidth;
        public readonly int OutputHeight;
        public readonly Rational FrameRate;
        public readonly long BitRate;
        public readonly ICodecOptions CodecOptions;
        public readonly IHardwareEncodeOptions? HardwareOptions;

        public void Validate()
        {
            FFmpegUtils.ValidateSize(nameof(InputWidth), InputWidth);
            FFmpegUtils.ValidateSize(nameof(InputHeight), InputHeight);
            FFmpegUtils.ValidateSize(nameof(OutputWidth), OutputWidth);
            FFmpegUtils.ValidateSize(nameof(OutputHeight), OutputHeight);
            FFmpegUtils.ValidateFrameRate(nameof(FrameRate), FrameRate);
        }

        public VideoEncodeOptions(int width, int height, Rational frameRate, long bitrate, ICodecOptions codecOptions, IHardwareEncodeOptions? hardwareOptions = null)
        {
            InputWidth = OutputWidth = width;
            InputHeight = OutputHeight = height;
            FrameRate = frameRate;
            BitRate = bitrate is > 0 ? bitrate : DefaultBitRate;
            CodecOptions = codecOptions;
            HardwareOptions = hardwareOptions;
            Validate();
        }

        public VideoEncodeOptions(int inWidth, int inHeight, int outWidth, int outHeight, Rational frameRate, long bitrate, ICodecOptions codecOptions, IHardwareEncodeOptions? hardwareOptions = null)
        {
            InputWidth = inWidth;
            InputHeight = inHeight;
            OutputWidth = outWidth;
            OutputHeight = outHeight;
            FrameRate = frameRate;
            BitRate = bitrate is > 0 ? bitrate : DefaultBitRate;
            CodecOptions = codecOptions;
            HardwareOptions = hardwareOptions;
            Validate();
        }
    }
}
