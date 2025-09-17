using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Files
{
    public static partial class FileList
    {
        public static bool AnyMatch(this IEnumerable<string> paths, Regex acceptExt) => paths.Any(acceptExt.IsMatch);
        public static bool AnyMatch(this List<string> paths, Regex acceptExt) => AnyMatchCore(CollectionsMarshal.AsSpan(paths), acceptExt);
        public static bool AnyMatch(this string[] paths, Regex acceptExt) => AnyMatchCore(paths, acceptExt);
        public static bool AnyMatch(this Span<string> paths, Regex acceptExt) => AnyMatchCore(paths, acceptExt);
        public static bool AnyMatch(this ReadOnlySpan<string> paths, Regex acceptExt) => AnyMatchCore(paths, acceptExt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AnyMatchCore(this ReadOnlySpan<string> paths, Regex acceptExt)
        {
            foreach (var path in paths)
            {
                if (acceptExt.IsMatch(path))
                {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyMatch<T>(this T paths, params ReadOnlySpan<Regex> acceptExts) where T : IEnumerable<string>
        {
            foreach (var ext in acceptExts)
            {
                if (AnyMatch(paths, ext))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AnyMatch(this List<string> paths, params ReadOnlySpan<Regex> acceptExts) => AnyMatchCore(CollectionsMarshal.AsSpan(paths), acceptExts);
        public static bool AnyMatch(this string[] paths, params ReadOnlySpan<Regex> acceptExts) => AnyMatchCore(paths, acceptExts);
        public static bool AnyMatch(this Span<string> paths, params ReadOnlySpan<Regex> acceptExts) => AnyMatchCore(paths, acceptExts);
        public static bool AnyMatch(this ReadOnlySpan<string> paths, params ReadOnlySpan<Regex> acceptExts) => AnyMatchCore(paths, acceptExts);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AnyMatchCore(this ReadOnlySpan<string> paths, params ReadOnlySpan<Regex> acceptExts)
        {
            foreach (var ext in acceptExts)
            {
                if (paths.AnyMatch(ext))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
