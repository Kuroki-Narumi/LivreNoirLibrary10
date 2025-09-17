using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public enum AmfUsage
    {
        None = 0,
        transcoding,
        ultralowlatency,
        lowlatency,
        webcam,
        high_quality,
    }
}
