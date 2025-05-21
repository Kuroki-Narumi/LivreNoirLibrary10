using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public readonly struct VideoDecodeOptions(int streamIndex, int width, int height)
    {
        public readonly int Width = width;
        public readonly int Height = height;
        // -1 をデフォルト値(struct default)にするため、引数を1増やした状態で保持する
        private readonly int _stream_index = streamIndex + 1;

        public readonly int StreamIndex => _stream_index - 1;

        public VideoDecodeOptions(int width, int height) : this(-1, width, height) { }

        public static implicit operator VideoDecodeOptions((int, int) tuple) => new(-1, tuple.Item1, tuple.Item2);
        public static implicit operator VideoDecodeOptions((int, int, int) tuple) => new(tuple.Item1, tuple.Item2, tuple.Item3);

        public void Deconstruct(out int width, out int height)
        {
            width = Width;
            height = Height;
        }

        public void Deconstruct(out int streamIndex, out int width, out int height)
        {
            width = Width;
            height = Height;
            streamIndex = StreamIndex;
        }
    }
}
