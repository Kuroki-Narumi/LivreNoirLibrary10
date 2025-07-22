using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms.RawData
{
    internal partial class BmsTextReader
    {
        public static IEnumerable<UsedFileInfo> EnumUsedFiles(string path)
        {
            string[] list;
            using (var file = File.OpenRead(path))
            {
                list = ReadLines(file);
            }
            static bool IsValid(Match match, [MaybeNullWhen(false)]out string value)
            {
                if (match.Success)
                {
                    value = match.Groups[2].Value.Trim();
                    return !string.IsNullOrWhiteSpace(value);
                }
                value = null;
                return false;
            }
            foreach (var line in list)
            {
                var match = GR_WavDef.Match(line);
                if (IsValid(match, out var value))
                {
                    yield return new(value, false);
                    continue;
                }
                match = GR_BmpDef.Match(line);
                if (IsValid(match, out value))
                {
                    yield return new(value, true);
                    continue;
                }
                match = GR_Header.Match(line);
                if (IsValid(match, out value) && HeaderBase.TryGetType(match.Groups[1].Value, out var type))
                {
                    switch (type)
                    {
                        case HeaderType.StageFile or HeaderType.Banner or HeaderType.BackBmp:
                            yield return new(value, true);
                            continue;
                        case HeaderType.Preview:
                            yield return new(value, false);
                            continue;
                    }
                }
            }
        }
    }
}

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsUtils
    {
        public static IEnumerable<UsedFileInfo> EnumUsedFiles(string path) => RawData.BmsTextReader.EnumUsedFiles(path);
    }

    public readonly record struct UsedFileInfo(string Value, bool IsImage);
}