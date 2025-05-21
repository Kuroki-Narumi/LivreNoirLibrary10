using System;
using System.IO;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Ogg.Vorbis;
using LivreNoirLibrary.Media.FFmpeg;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class WaveBuffer
    {
        public static WaveBuffer AutoOpen(string path)
        {
            using var sw = ExStopwatch.LoadProcessTime(path);
            if (WaveDecoder.IsSupported(path))
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
                data.Load(stream);
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
    }
}
