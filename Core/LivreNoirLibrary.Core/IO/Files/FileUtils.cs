using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.IO
{
    public static class FileUtils
    {
        public static bool TryGetAudioFileName(string? path, [MaybeNullWhen(false)] out string actualName) => Exts.TryGetCompatible(path, out actualName, Exts.ExAudioExts);
        public static bool TryGetImageFileName(string? path, [MaybeNullWhen(false)] out string actualPath) => Exts.TryGetCompatible(path, out actualPath, Exts.ImageExts);
        public static bool TryGetVideoFileName(string? path, [MaybeNullWhen(false)] out string actualPath) => Exts.TryGetCompatible(path, out actualPath, Exts.VideoExts);
        public static bool TryGetMediaFileName(string? path, [MaybeNullWhen(false)] out string actualPath) => Exts.TryGetCompatible(path, out actualPath, Exts.MediaExts);

        public static IEnumerable<string> GetAllAudioFileNames(string? path) => Exts.GetAllCompatible(path, Exts.AudioExts);
        public static IEnumerable<string> GetAllMediaFileNames(string? path) => Exts.GetAllCompatible(path, Exts.MediaExts);
    }
}
