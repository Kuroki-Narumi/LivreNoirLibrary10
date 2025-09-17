using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class FieldSeparators
    {
        public const string Header = "*---------------------- HEADER FIELD";
        public const string Others = "*---------------------- COMMENT FIELD";
        public const string Def = "*---------------------- DEFINITION FIELD";
        public const string Data = "*---------------------- MAIN DATA FIELD";
        public const string Flows = "*---------------------- FLOW FIELD";

        public static bool IsMatch(ReadOnlySpan<char> span) => Regex.IsMatch(span);

        [GeneratedRegex(@"^\*-+[^-]+FIELD", RegexOptions.IgnoreCase)]
        private static partial Regex Regex { get; }
    }
}
