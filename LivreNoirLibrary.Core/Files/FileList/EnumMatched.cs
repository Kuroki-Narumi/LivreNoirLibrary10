using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Files
{
    public static partial class FileList
    {
        public static IEnumerable<string> EnumMatched<T>(this T paths, Regex acceptExt)
            where T : IEnumerable<string>
        {
            if (typeof(T) == typeof(string[]))
            {
                foreach (var path in (paths as string[])!)
                {
                    if (acceptExt.IsMatch(path))
                    {
                        yield return path;
                    }
                }
            }
            else if (typeof(T) == typeof(List<string>))
            {
                var list = (paths as List<string>)!;
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var path = list[i];
                    if (acceptExt.IsMatch(path))
                    {
                        yield return path;
                    }
                }
            }
            else
            {
                foreach (var path in paths)
                {
                    if (acceptExt.IsMatch(path))
                    {
                        yield return path;
                    }
                }
            }
        }

        public static IEnumerable<string> EnumMatched<T1, T2>(this T1 paths, T2 acceptExts)
            where T1 : IEnumerable<string>
            where T2 : IEnumerable<Regex>
        {
            if (typeof(T2) == typeof(Regex[]))
            {
                foreach (var ext in (acceptExts as Regex[])!)
                {
                    foreach (var path in EnumMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
            else if (typeof(T2) == typeof(IList<Regex>))
            {
                var list = (acceptExts as IList<Regex>)!;
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var ext = list[i];
                    foreach (var path in EnumMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
            else
            {
                foreach (var ext in acceptExts)
                {
                    foreach (var path in EnumMatched(paths, ext))
                    {
                        yield return path;
                    }
                }
            }
        }
    }
}
