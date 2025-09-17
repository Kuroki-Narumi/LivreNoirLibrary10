using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public readonly struct AudioDecodeOptions(int streamIndex, int sampleRate, int channels)
    {
        public readonly int SampleRate = sampleRate;
        public readonly int Channels = channels;
        // -1 をデフォルト値(struct default)にするため、引数を1増やした状態で保持する
        private readonly int _stream_index = streamIndex + 1;

        public readonly int StreamIndex => _stream_index - 1;

        public AudioDecodeOptions(int sampleRate, int channels) : this(-1, sampleRate, channels) { }
        public AudioDecodeOptions(in Wave.FormatChunk format) : this(-1, (int)format.SampleRate, format.Channels) { }

        public static implicit operator AudioDecodeOptions((int, int) tuple) => new(-1, tuple.Item1, tuple.Item2);
        public static implicit operator AudioDecodeOptions((int, int, int) tuple) => new(tuple.Item1, tuple.Item2, tuple.Item3);

        public void Deconstruct(out int sampleRate, out int channels)
        {
            sampleRate = SampleRate;
            channels = Channels;
        }

        public void Deconstruct(out int streamIndex, out int sampleRate, out int channels)
        {
            sampleRate = SampleRate;
            channels = Channels;
            streamIndex = StreamIndex;
        }
    }
}
