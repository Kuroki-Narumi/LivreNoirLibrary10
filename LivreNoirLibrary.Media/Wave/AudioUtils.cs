using System.Diagnostics.CodeAnalysis;
using NAudio.Wave;
using LivreNoirLibrary.Files;

namespace LivreNoirLibrary.Media
{
    public static class AudioUtils
    {
        public static bool TryGetWaveStream(string? path, [MaybeNullWhen(false)]out Wave.AudioFileReader stream)
        {
            stream = null;
            if (FileUtils.TryGetAudioFilename(path, out var path2))
            {
                stream = Wave.AudioFileReader.AutoOpen(path2);
            }
            return stream is not null;
        }
    }
}
