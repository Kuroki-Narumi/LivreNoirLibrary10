using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Media
{
    public class VideoSaveState : IVideoSaveState
    {
        public required int PixelWidth { get; init; }
        public required int PixelHeight { get; init; }
        public required Rational FrameRate { get; init; }

        public required bool AudioExists { get; init; }
        public required int AudioSampleRate { get; init; }
        public required int AudioChannels { get; init; }

        public required double TotalTime { get; init; }
        public required double AbortDeadline { get; init; }

        public required int ApproximateKbps { get; init; }
        public bool IsHevc { get; init; }

        public void OnAbort(ref double time) { }
    }
}
