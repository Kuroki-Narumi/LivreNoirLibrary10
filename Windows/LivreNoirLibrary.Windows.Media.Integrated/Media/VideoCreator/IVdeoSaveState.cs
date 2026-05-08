using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Media
{
    public interface IVideoSaveState
    {
        int PixelWidth { get; }
        int PixelHeight { get; }
        Rational FrameRate { get; }

        bool AudioExists { get; }
        int AudioSampleRate { get; }
        int AudioChannels { get; }

        double TotalTime { get; }
        double AbortDeadline { get; }

        int ApproximateKbps { get; }
        bool IsHevc { get; }

        void OnAbort(ref double time);
    }
}
