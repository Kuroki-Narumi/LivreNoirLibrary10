using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media
{
    public static partial class SliceUtils
    {
        public const string Suffix_Index1 = "<idx>";
        public const string Suffix_Index2 = "<idxX,Y>";
        public const string Suffix_Index3 = "<idxX,Y,Z>";
        public const string Suffix_Name = "<name>";
        public const string Suffix_NN = "<nn>";
        public const string Suffix_Vel = "<vel>";
        public const string Suffix_Gate = "<gate>";
        public const string Suffix_Auto = $"n{Suffix_NN}_v{Suffix_Vel}_g{Suffix_Gate}";

        public static bool ContainsIndexSuffix(string format) => Regex_IndexSuffix.IsMatch(format);

        public static string GetIndexFormat(int count)
        {
            var maxDigits = count switch
            {
                >= 9999 => 5,
                >= 999 => 4,
                >= 99 => 3,
                >= 9 => 2,
                _ => 1,
            };
            return $"{{0:D{maxDigits}}}";
        }

        public static string ReplaceIndexSuffix(string format, int index, int maxIndex)
        {
            return Regex_IndexSuffix.Replace(format, matched =>
            {
                if (!int.TryParse(matched.Groups[1]?.Value ?? "1", out int start))
                {
                    start = 1;
                }
                if (!int.TryParse(matched.Groups[2]?.Value ?? "1", out int step))
                {
                    step = 1;
                }
                if (!int.TryParse(matched.Groups[3]?.Value ?? "_invalid_", out int decimals))
                {
                    decimals = (start + (maxIndex - 1) * step + 1) switch
                    {
                        < 10 => 1,
                        < 100 => 2,
                        < 1000 => 3,
                        < 10000 => 4,
                        _ => 5,
                    };
                }
                return string.Format($"{{0:D{decimals}}}", start + index * step);
            });
        }

        public static string ReplaceAutoSuffix(string format, Func<string> getName, Func<string> getNN, Func<string> getVel, Func<string> getGate)
        {
            return Regex_AutoSuffix.Replace(format, matched => matched.Value.ToLower() switch
            {
                Suffix_Name => getName(),
                Suffix_NN => getNN(),
                Suffix_Vel => getVel(),
                Suffix_Gate => getGate(),
                _ => ""
            });
        }

        [GeneratedRegex(@"<idx(?:(\d+)(?:,(\d+))?(?:,(\d+))?)?>", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_IndexSuffix { get; }
        [GeneratedRegex($"{Suffix_Name}|{Suffix_NN}|{Suffix_Vel}|{Suffix_Gate}", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_AutoSuffix { get; }
    }
}
