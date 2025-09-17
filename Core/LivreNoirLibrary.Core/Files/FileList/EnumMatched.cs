using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Files
{
    public static partial class FileList
    {
        public static IEnumerable<string> EnumerateMatched(this string[] paths, Regex acceptExt)
        {
            foreach (var path in paths)
            {
                if (acceptExt.IsMatch(path))
                {
                    yield return path;
                }
            }
        }

        public static IEnumerable<string> EnumerateMatched(this IList<string> paths, Regex acceptExt)
        {
            var count = paths.Count;
            for (int i = 0; i < count; i++)
            {
                var path = paths[i];
                if (acceptExt.IsMatch(path))
                {
                    yield return path;
                }
            }
        }

        public static IEnumerable<string> EnumerateMatched(this IEnumerable<string> paths, Regex acceptExt)
        {
            foreach (var path in paths)
            {
                if (acceptExt.IsMatch(path))
                {
                    yield return path;
                }
            }
        }

        public static IEnumerable<string> EnumerateMatched<T>(this string[] paths, T acceptExts)
            where T : IEnumerable<Regex>
        {
            if (typeof(T) == typeof(Regex[]))
            {
                foreach (var ext in (acceptExts as Regex[])!)
                {
                    foreach (var path in EnumerateMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
            else if (typeof(T) == typeof(IList<Regex>))
            {
                var list = (acceptExts as IList<Regex>)!;
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var ext = list[i];
                    foreach (var path in EnumerateMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
            else
            {
                foreach (var ext in acceptExts)
                {
                    foreach (var path in EnumerateMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
        }

        public static IEnumerable<string> EnumerateMatched<T>(this IList<string> paths, T acceptExts)
            where T : IEnumerable<Regex>
        {
            if (typeof(T) == typeof(Regex[]))
            {
                foreach (var ext in (acceptExts as Regex[])!)
                {
                    foreach (var path in EnumerateMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
            else if (typeof(T) == typeof(IList<Regex>))
            {
                var list = (acceptExts as IList<Regex>)!;
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var ext = list[i];
                    foreach (var path in EnumerateMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
            else
            {
                foreach (var ext in acceptExts)
                {
                    foreach (var path in EnumerateMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
        }

        public static IEnumerable<string> EnumerateMatched<T>(this IEnumerable<string> paths, T acceptExts)
            where T : IEnumerable<Regex>
        {
            if (typeof(T) == typeof(Regex[]))
            {
                foreach (var ext in (acceptExts as Regex[])!)
                {
                    foreach (var path in EnumerateMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
            else if (typeof(T) == typeof(IList<Regex>))
            {
                var list = (acceptExts as IList<Regex>)!;
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var ext = list[i];
                    foreach (var path in EnumerateMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
            else
            {
                foreach (var ext in acceptExts)
                {
                    foreach (var path in EnumerateMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
        }
    }
}
