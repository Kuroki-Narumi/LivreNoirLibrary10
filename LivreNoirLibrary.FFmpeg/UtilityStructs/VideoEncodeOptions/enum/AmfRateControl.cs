using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public enum AmfRateControl
    {
        None = 0,
        cqp,
        cbr,
        hqcbr,
        vbr_peak,
        vbr_latencyk,
        qvbr,
        hqvbr,
    }
}
