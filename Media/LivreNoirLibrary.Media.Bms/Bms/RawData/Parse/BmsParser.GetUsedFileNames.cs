using System;
using System.Collections.Generic;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly record struct UsedFileInfo(string Filename, bool IsImage);
    public partial class BmsParser
    {
        public static List<UsedFileInfo> GetUsedFileNames(string path, List<UsedFileInfo>? result = null)
        {
            BmsParser reader;
            using (var file = File.OpenRead(path))
            {
                reader = new(file);
            }
            return reader.GetUsedFileNames(result);
        }

        public List<UsedFileInfo> GetUsedFileNames(List<UsedFileInfo>? result = null)
        {
            result ??= [];
            var radix = Radix;
            foreach (var line in RawText.EnumerateLines())
            {
                var span = line.TrimStart();
                if (TryGetDef(span, Tags.Wav, radix, out _, out var valueSpan))
                {
                    result.Add(new(valueSpan.ToString(), false));
                }
                else if (TryGetDef(span, Tags.Bmp, radix, out _, out valueSpan))
                {
                    result.Add(new(valueSpan.ToString(), true));
                }
                else if (TryGetHeader(span, out var keySpan, out valueSpan) && Enum.TryParse<HeaderType>(keySpan, true, out var headerType))
                {
                    switch (headerType)
                    {
                        case HeaderType.StageFile or HeaderType.Banner or HeaderType.BackBmp:
                            result.Add(new(valueSpan.ToString(), true));
                            break;
                        case HeaderType.Preview:
                            result.Add(new(valueSpan.ToString(), false));
                            break;
                    }
                }
            }
            return result;
        }
    }
}