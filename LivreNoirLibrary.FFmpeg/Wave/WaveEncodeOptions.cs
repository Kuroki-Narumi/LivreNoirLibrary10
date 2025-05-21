using System;
using LivreNoirLibrary.Media.FFmpeg;

namespace LivreNoirLibrary.Media.Wave
{
    public readonly struct WaveEncodeOptions
    {
        public const SampleFormat DefaultFormat = SampleFormat.Int16;

        public readonly int InputSampleRate;
        public readonly int InputChannels;
        public readonly int OutputSampleRate;
        public readonly int OutputChannels;
        public readonly SampleFormat Format;

        public WaveEncodeOptions(int sampleRate, int channels, SampleFormat format)
        {
            InputSampleRate = OutputSampleRate = sampleRate;
            InputChannels = OutputChannels = channels;
            Format = format.IsValid() ? format : DefaultFormat;
        }

        public WaveEncodeOptions(int inSampleRate, int inChannels, int outSampleRate, int outChannels, SampleFormat format)
        {
            InputSampleRate = inSampleRate;
            InputChannels = inChannels;
            OutputSampleRate = outSampleRate;
            OutputChannels = outChannels;
            Format = format.IsValid() ? format : DefaultFormat;
        }

        public void Validate()
        {
            FFmpegUtils.ValidateSampleRate(nameof(InputSampleRate), InputSampleRate);
            FFmpegUtils.ValidateSampleRate(nameof(OutputSampleRate), OutputSampleRate);
            FFmpegUtils.ValidateChannels(nameof(InputChannels), InputChannels);
            FFmpegUtils.ValidateChannels(nameof(OutputChannels), OutputChannels);
            if (!Format.IsValid())
            {
                throw new ArgumentOutOfRangeException(nameof(Format), $"{Format} is unsupported format.");
            }
        }

        public void Deconstruct(out int inSampleRate, out int inChannels, out int outSampleRate, out int outChannels, out SampleFormat format)
        {
            inSampleRate = InputSampleRate;
            inChannels = InputChannels;
            outSampleRate = OutputSampleRate;
            outChannels = OutputChannels;
            format = Format;
        }
    }
}
