using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public readonly struct AudioEncodeOptions
    {
        public const long DefaultBitrate = 224000;

        public readonly int InputSampleRate;
        public readonly int InputChannels;
        public readonly int OutputSampleRate;
        public readonly int OutputChannels;
        public readonly long Bitrate;

        public AudioEncodeOptions(int sampleRate, int channels, long bitRate = DefaultBitrate)
        {
            InputSampleRate = OutputSampleRate = sampleRate;
            InputChannels = OutputChannels = channels;
            Bitrate = bitRate;
        }

        public AudioEncodeOptions(int inSampleRate, int inChannels, int outSampleRate, int outChannels, long bitRate = DefaultBitrate)
        {
            InputSampleRate = inSampleRate;
            InputChannels = inChannels;
            OutputSampleRate = outSampleRate;
            OutputChannels = outChannels;
            Bitrate = bitRate;
        }

        public void Validate()
        {
            FFmpegUtils.ValidateSampleRate(nameof(InputSampleRate), InputSampleRate);
            FFmpegUtils.ValidateSampleRate(nameof(OutputSampleRate), OutputSampleRate);
            FFmpegUtils.ValidateChannels(nameof(InputChannels), InputChannels);
            FFmpegUtils.ValidateChannels(nameof(OutputChannels), OutputChannels);
        }

        public void Deconstruct(out int inSampleRate, out int inChannels, out int outSampleRate, out int outChannels, out long bitRate)
        {
            inSampleRate = InputSampleRate;
            inChannels = InputChannels;
            outSampleRate = OutputSampleRate;
            outChannels = OutputChannels;
            bitRate = Bitrate;
        }
    }
}
