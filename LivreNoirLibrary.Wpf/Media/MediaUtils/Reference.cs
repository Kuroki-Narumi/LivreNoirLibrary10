using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media
{
    public static partial class MediaUtils
    {
        [GeneratedRegex(@"(?<=\.)[^\.]+$", RegexOptions.CultureInvariant)]
        private static partial Regex Regex_Ext { get; }

        public static bool IsFileReferenced(string filePath, string serachFilePath, params ReadOnlySpan<string> compatibleExts)
        {
            if (File.Exists(filePath))
            {
                var relative = Path.GetRelativePath(filePath, serachFilePath);
                var withoutExt = Regex_Ext.Replace(relative, "");
                using var reader = new StreamReader(filePath);
                while (reader.ReadLine() is string line)
                {
                    if (line.IndexOf(relative, StringComparison.OrdinalIgnoreCase) is >= 0)
                    {
                        return true;
                    }
                    var index = line.IndexOf(withoutExt, StringComparison.OrdinalIgnoreCase);
                    if (index is >= 0)
                    {
                        index += withoutExt.Length;
                        foreach (var ext in compatibleExts)
                        {
                            if (line.IndexOf(ext, index, StringComparison.OrdinalIgnoreCase) == index)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
