using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Files
{
    public static class FileUtils
    {
        public static bool TryGetAudioFilename(string? path, [MaybeNullWhen(false)] out string actualName) => Exts.TryGetCompatible(path, out actualName, Exts.ExAudioExts);
        public static bool TryGetImageFilename(string? path, [MaybeNullWhen(false)] out string actualPath) => Exts.TryGetCompatible(path, out actualPath, Exts.ImageExts);
        public static bool TryGetVideoFilename(string? path, [MaybeNullWhen(false)] out string actualPath) => Exts.TryGetCompatible(path, out actualPath, Exts.VideoExts);
        public static bool TryGetMediaFilename(string? path, [MaybeNullWhen(false)] out string actualPath) => Exts.TryGetCompatible(path, out actualPath, Exts.MediaExts);

        public static string[] GetAllAudioFilenames(string? path) => Exts.GetAllCompatible(path, Exts.AudioExts);
        public static string[] GetAllImageFilenames(string? path) => Exts.GetAllCompatible(path, Exts.ImageExts);
        public static string[] GetAllVideoFilenames(string? path) => Exts.GetAllCompatible(path, Exts.VideoExts);
        public static string[] GetAllMediaFilenames(string? path) => Exts.GetAllCompatible(path, Exts.MediaExts);

    }
}
