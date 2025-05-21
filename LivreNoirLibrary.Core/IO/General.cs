using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.IO
{
    public static class General
    {
        public static bool Tweet { get; set; } = true;

        public static bool TryOpen<T>(string path, Func<Stream, T> readFunc, [MaybeNullWhen(false)] out T result, [MaybeNullWhen(true)] out Exception exception)
        {
            try
            {
                using var stream = File.OpenRead(path);
                result = readFunc(stream);
                exception = null;
                return true;
            }
            catch (Exception e)
            {
                result = default;
                exception = e;
                return false;
            }
        }

        private static T OpenInternal<T>(string path, Func<Stream, T> readFunc)
        {
            using var sw = ExStopwatch.LoadProcessTime(path, Tweet);
            using var stream = File.OpenRead(path);
            var result = readFunc(stream);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Open<T>(string path, Func<Stream, T> readFunc) => OpenInternal(path, readFunc);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Open<T>(string path) where T : IStreamDumpable<T> => OpenInternal(path, T.Load);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Open<T>(string path, Func<BinaryReader, T> readFunc)
        {
            return OpenInternal(path, stream =>
            {
                using BinaryReader reader = new(stream);
                return readFunc(reader);
            });
        }

        public static FileStream CreateSafe(string path)
        {
            EnsureDirectory(path);
            return File.Create(path);
        }

        public static void PrepareSave(ref string path, Regex? extReg = null, string ext = "")
        {
            if (extReg is not null && !extReg.IsMatch(path))
            {
                path += $".{ext}";
            }
            EnsureDirectory(path);
        }

        private static void SaveInternal(string path, Action<Stream> writeAction, Regex? extReg, string ext)
        {
            PrepareSave(ref path, extReg, ext);
            using var sw = ExStopwatch.SaveProcessTime(path, Tweet);
            using var stream = File.Create(path);
            writeAction(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save(string path, Action<Stream> writeAction, Regex? extReg = null, string ext = "") => SaveInternal(path, writeAction, extReg, ext);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save<T>(string path, T data, Regex? extReg = null, string ext = "") where T : IStreamDumpable<T> => SaveInternal(path, data.Dump, extReg, ext);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save(string path, Action<BinaryWriter> writeAction, Regex? extReg = null, string ext = "")
        {
            SaveInternal(path, stream =>
            {
                using BinaryWriter writer = new(stream);
                writeAction(writer);
            }, extReg, ext);
        }

        public static char[] InvalidPathChars { get; } = Path.GetInvalidPathChars();
        public static char[] InvalidFileNameChars { get; } = Path.GetInvalidFileNameChars();

        public static string GetSafePathName(string path) => string.Join("_", path.Split(InvalidPathChars));
        public static string GetSafeFileName(string path) => string.Join("_", path.Split(InvalidFileNameChars));

        public static string GetAssemblyDir() => AppDomain.CurrentDomain.BaseDirectory;

        public static string GetAppDataPath(string appName)
        {
            string path = Path.Join(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData
                    ),
                "LivreNoir", appName);
            Directory.CreateDirectory(path);
            return path;
        }

        public static void SetCurrentDirectory(string path)
        {
            if (Path.IsPathFullyQualified(path) && GetDirectoryName(path) is string p)
            {
                Directory.SetCurrentDirectory(p);
            }
        }

        public static void EnsureDirectory(string path)
        {
            var dir = GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static string? GetDirectoryName(string path)
        {
            if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                return Path.GetDirectoryName(path);
            }
        }
    }
}
