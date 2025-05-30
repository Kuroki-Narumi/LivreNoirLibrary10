﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Text
{
    public static partial class TextConvert
    {
        [GeneratedRegex(@"[！-～“”]")]
        static partial Regex Regex_Wide { get; }
        public static string Wide2Half(this string text) => Regex_Wide.Replace(text, m => _wide2half[m.Value]);

        public static string Wide2Half_General(this string text) => _wide2half.TryGetValue(text, out var half) ? half : text;

        static readonly Dictionary<string, string> _wide2half = new()
        {
            { "！", "!" },
            { "＂", "\"" },
            { "“", "\"" },
            { "”", "\"" },
            { "＃", "#" },
            { "＄", "$" },
            { "％", "%" },
            { "＆", "&" },
            { "＇", "'" },
            { "（", "(" },
            { "）", ")" },
            { "＊", "*" },
            { "＋", "+" },
            { "，", "," },
            { "－", "-" },
            { "．", "." },
            { "／", "/" },
            { "０", "0" },
            { "１", "1" },
            { "２", "2" },
            { "３", "3" },
            { "４", "4" },
            { "５", "5" },
            { "６", "6" },
            { "７", "7" },
            { "８", "8" },
            { "９", "9" },
            { "：", ":" },
            { "；", ";" },
            { "＜", "<" },
            { "＝", "=" },
            { "＞", ">" },
            { "？", "?" },
            { "＠", "@" },
            { "Ａ", "A" },
            { "Ｂ", "B" },
            { "Ｃ", "C" },
            { "Ｄ", "D" },
            { "Ｅ", "E" },
            { "Ｆ", "F" },
            { "Ｇ", "G" },
            { "Ｈ", "H" },
            { "Ｉ", "I" },
            { "Ｊ", "J" },
            { "Ｋ", "K" },
            { "Ｌ", "L" },
            { "Ｍ", "M" },
            { "Ｎ", "N" },
            { "Ｏ", "O" },
            { "Ｐ", "P" },
            { "Ｑ", "Q" },
            { "Ｒ", "R" },
            { "Ｓ", "S" },
            { "Ｔ", "T" },
            { "Ｕ", "U" },
            { "Ｖ", "V" },
            { "Ｗ", "W" },
            { "Ｘ", "X" },
            { "Ｙ", "Y" },
            { "Ｚ", "Z" },
            { "［", "[" },
            { "＼", "\\" },
            { "］", "]" },
            { "＾", "^" },
            { "＿", "_" },
            { "｀", "`" },
            { "ａ", "a" },
            { "ｂ", "b" },
            { "ｃ", "c" },
            { "ｄ", "d" },
            { "ｅ", "e" },
            { "ｆ", "f" },
            { "ｇ", "g" },
            { "ｈ", "h" },
            { "ｉ", "i" },
            { "ｊ", "j" },
            { "ｋ", "k" },
            { "ｌ", "l" },
            { "ｍ", "m" },
            { "ｎ", "n" },
            { "ｏ", "o" },
            { "ｐ", "p" },
            { "ｑ", "q" },
            { "ｒ", "r" },
            { "ｓ", "s" },
            { "ｔ", "t" },
            { "ｕ", "u" },
            { "ｖ", "v" },
            { "ｗ", "w" },
            { "ｘ", "x" },
            { "ｙ", "y" },
            { "ｚ", "z" },
            { "｛", "{" },
            { "｜", "|" },
            { "｝", "}" },
            { "～", "~" },
        };
    }
}
