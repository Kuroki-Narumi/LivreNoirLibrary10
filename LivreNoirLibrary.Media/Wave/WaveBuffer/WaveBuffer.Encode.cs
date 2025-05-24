using System;
using System.Threading;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class WaveBuffer
    {
        public static void ReportingEncode(IWaveBuffer source, IAudioEncoder encoder, ProgressReporter p, CancellationToken c)
        {
            c.ThrowIfCancellationRequested();
            var data = source.Data;
            var rate = source.SampleRate;
            var ch = source.Channels;
            var unit = rate * ch;
            var index = 0;
            var rest = data.Length;
            p.Report(null, 0, rest);
            while (rest is > 0)
            {
                c.ThrowIfCancellationRequested();
                if (rest < unit)
                {
                    unit = rest;
                }
                encoder.Write(data.Slice(index, unit));
                index += unit;
                rest -= unit;
                p.Report(null, index);
            }
        }
    }
}
