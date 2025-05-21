using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Numerics
{
    public static partial class RpnConstants
    {
        public const int Priority_AddSub = 100;
        public const int Priority_MulDiv = 200;

        public const char Space = ' ';
        public const char Delimiter = ',';
        public const char Plus = '+';
        public const char Minus = '-';
        public const char Multiply = '*';
        public const char Divide = '/';
        public const char Modulo = '%';
        public const char Power = '^';

        public const char OpenBracket = '(';
        public const char CloseBracket = ')';

        [GeneratedRegex(@"[+-]?\d\.?(?:\d+?)?")]
        public static partial Regex Regex_Number { get; }

        public static Regex GetRegex(this IRpnToken token) => new($"{token.Symbol}(?=\\s|$)", RegexOptions.IgnoreCase);
    }
}
