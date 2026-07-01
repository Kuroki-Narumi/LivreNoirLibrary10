using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Scraping
{
    public static partial class Url
    {
        public const string BaseUrl = @"https://www.db.yugioh-card.com/yugiohdb/";
        public const string CardFormat = $@"{BaseUrl}card_search.action?ope=2&cid={{0}}&request_locale={{1}}";
        public const string PackFormat = $@"{BaseUrl}card_search.action?ope=1&sess=1&pid={{0}}&rp=99999&request_locale={{1}}";
        public const string PackListFormat = $@"{BaseUrl}card_list.action?request_locale={{0}}";
        public const string RegulationFormat = $@"{BaseUrl}forbidden_limited.action?request_locale={{0}}";

        public static string Card(int id, bool tcg) => string.Format(CardFormat, id, LocaleSuffix.Get(tcg));
        public static string Card(int id, LocaleType locale) => string.Format(CardFormat, id, LocaleSuffix.Get(locale));

        public static string Pack(string pid)
        {
            string suffix;
            if (Data.CardPack.IsTcgPack(pid))
            {
                pid = pid.Replace("e", "");
                suffix = LocaleSuffix.English;
            }
            else
            {
                suffix = LocaleSuffix.Japanese;
            }
            return string.Format(PackFormat, pid, suffix);
        }

        public static string PackList(bool tcg) => string.Format(PackListFormat, LocaleSuffix.Get(tcg));
        public static string PackList(LocaleType locale) => string.Format(PackListFormat, LocaleSuffix.Get(locale));

        public static string Regulation(bool tcg) => string.Format(RegulationFormat, LocaleSuffix.Get(tcg));
        public static string Regulation(LocaleType locale) => string.Format(RegulationFormat, LocaleSuffix.Get(locale));

        [GeneratedRegex(@"(?<=\?ope=2&cid=)(\d+)")]
        private static partial Regex Regex_CardUrl { get; }

        [GeneratedRegex(@"(?<=\?ope=1&sess=1&pid=)([^&])+")]
        private static partial Regex Regex_PackUrl { get; }

        public static bool TryGetCardId(ReadOnlySpan<char> text, out int id)
        {
            foreach (var match in Regex_CardUrl.EnumerateMatches(text))
            {
                id = int.Parse(text.Slice(match.Index, match.Length));
                return true;
            }
            id = -1;
            return false;
        }

        public static bool TryGetPackId(ReadOnlySpan<char> text, out ReadOnlySpan<char> id)
        {
            foreach (var match in Regex_PackUrl.EnumerateMatches(text))
            {
                id = text.Slice(match.Index, match.Length);
                return true;
            }
            id = default;
            return false;
        }
    }
}
