using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public class VideoFrameBacket :IBacket<VideoFrameInfo, VideoFrameBacket>
    {
        public UnmanagedArray<byte> Buffer { get; } = new();
        public long Tick { get; private set; }

        private VideoFrameBacket(VideoFrameInfo info)
        {
            SetData(info);
        }

        public static VideoFrameBacket Create(in VideoFrameInfo input) => new(input);

        public void ClearData()
        {
            Buffer.Clear();
            Tick = -1;
        }

        public void SetData(in VideoFrameInfo input)
        {
            var source = input.Buffer;
            Buffer.EnsureSize(source.Length);
            Buffer.CopyFrom(source);
            Tick = input.Tick;
        }

        public void CopyPixels(WriteableBitmap target)
        {
            using var p = target.BeginWrite();
            p.CopyFrom(Buffer.AsSpan(), p.Width);
        }
    }
}
