using System;
using System.IO;
using System.Threading;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Media.Ogg.Vorbis;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class WaveBuffer
    {
        public static WaveBuffer AutoOpen(string path)
        {
            using var sw = ExStopwatch.LoadProcessTime(path);
            if (WaveInfo.IsSupported(path))
            {
                WaveData data = new();
                using WaveDecoder decoder = new(path, 0, 0);
                data.Load(decoder);
                return data;
            }
            else if (VorbisCommentEditor.IsSupported(path))
            {
                VorbisData data = new();
                using var stream = File.OpenRead(path);
                data.LoadStream(stream);
                return data;
            }
            else
            {
                GenericWaveBuffer data = new();
                using AudioDecoder decoder = new(path);
                data.Load(decoder);
                return data;
            }
        }

        public static void ReportingEncode(IWaveBuffer source, IAudioEncodeContext encoder, ProgressReporter p, CancellationToken c)
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
