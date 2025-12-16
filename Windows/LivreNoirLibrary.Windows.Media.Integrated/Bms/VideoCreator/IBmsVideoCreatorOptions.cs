using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public interface IBmsVideoCreatorOptions
    {
        Rational FrameRate { get; }
        bool IsHevc { get; }
        int QP { get; }
        int ApproximateKbps { get; }
        int AudioSampleRate { get; }
        double AudioDelay { get; }
    }
}
