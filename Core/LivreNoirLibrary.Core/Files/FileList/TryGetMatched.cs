using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Files
{
    public static partial class FileList
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetMatched(this IEnumerable<string> paths, Regex acceptExt, [MaybeNullWhen(false)] out string path)
        {
            foreach (var p in paths)
            {
                if (acceptExt.IsMatch(p))
                {
                    path = p;
                    return true;
                }
            }
            path = null;
            return false;
        }

        public static bool TryGetMatched(List<string> paths, Regex acceptExt, [MaybeNullWhen(false)] out string path)
            => TryGetMatchedCore(CollectionsMarshal.AsSpan(paths), acceptExt, out path);
        public static bool TryGetMatched(string[] paths, Regex acceptExt, [MaybeNullWhen(false)] out string path)
            => TryGetMatchedCore(paths, acceptExt, out path);
        public static bool TryGetMatched(this Span<string> paths, Regex acceptExt, [MaybeNullWhen(false)] out string path)
            => TryGetMatchedCore(paths, acceptExt, out path);
        public static bool TryGetMatched(this ReadOnlySpan<string> paths, Regex acceptExt, [MaybeNullWhen(false)] out string path)
            => TryGetMatchedCore(paths, acceptExt, out path);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetMatchedCore(ReadOnlySpan<string> paths, Regex acceptExt, [MaybeNullWhen(false)]out string path)
        {
            foreach (var p in paths)
            {
                if (acceptExt.IsMatch(p))
                {
                    path = p;
                    return true;
                }
            }
            path = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetMatched(this IEnumerable<string> paths, [MaybeNullWhen(false)] out string path, params ReadOnlySpan<Regex> acceptExts)
        {
            foreach (var ext in acceptExts)
            {
                if (TryGetMatched(paths, ext, out path))
                {
                    return true;
                }
            }
            path = null;
            return false;
        }

        public static bool TryGetMatched(List<string> paths, [MaybeNullWhen(false)] out string path, params ReadOnlySpan<Regex> acceptExts)
            => TryGetMatchedCore(CollectionsMarshal.AsSpan(paths), acceptExts, out path);
        public static bool TryGetMatched(string[] paths, [MaybeNullWhen(false)] out string path, params ReadOnlySpan<Regex> acceptExts)
            => TryGetMatchedCore(paths, acceptExts, out path);
        public static bool TryGetMatched(this Span<string> paths, [MaybeNullWhen(false)] out string path, params ReadOnlySpan<Regex> acceptExts)
            => TryGetMatchedCore(paths, acceptExts, out path);
        public static bool TryGetMatched(this ReadOnlySpan<string> paths, [MaybeNullWhen(false)] out string path, params ReadOnlySpan<Regex> acceptExts)
            => TryGetMatchedCore(paths, acceptExts, out path);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetMatchedCore(ReadOnlySpan<string> paths, ReadOnlySpan<Regex> acceptExts, [MaybeNullWhen(false)] out string path)
        {
            foreach (var ext in acceptExts)
            {
                if (TryGetMatched(paths, ext, out path))
                {
                    return true;
                }
            }
            path = null;
            return false;
        }
    }
}
