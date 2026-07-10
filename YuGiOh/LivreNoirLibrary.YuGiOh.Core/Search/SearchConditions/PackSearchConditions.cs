using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class PackSearchConditions
    {
        [JsonIgnore]
        public string SearchText { get; set; } = "";

        [JsonPropertyName(JsonPropertyNames.Search_TextFlags)]
        public TextSearchFlags TextFlags { get; set; } = TextSearchFlags.IgnoreCase | TextSearchFlags.IgnoreSymbols;

        [JsonPropertyName(JsonPropertyNames.Search_Count)]
        public NumberRange CardCount { get; set; } = new(0, 999, false, false);

        [JsonPropertyName(JsonPropertyNames.Search_FirstDate)]
        public DateRange Date { get; set; } = new();

        [JsonPropertyName(JsonPropertyNames.Search_DateLocale)]
        public LocaleType DateLocale { get; set; } = 0;

        private bool _req_ocg;
        private bool _req_tcg;
        private bool _notEffective;
        private Regex? _regex;
        private TextForSearchStringConverter _converter;
        private readonly List<SearchSegment> _input = [];

        public void Prepare()
        {
            var locale = DateLocale;
            _req_ocg = locale is LocaleType.Ocg;
            _req_tcg = locale is LocaleType.Tcg;

            var input = _input;
            input.Clear();
            _regex = null;
            _notEffective = false;

            var text = SearchText.AsSpan();
            var flags = TextFlags;
            if (text.IsWhiteSpace())
            {
                return;
            }

            var ignoreCase = (flags & TextSearchFlags.IgnoreCase) is not 0;
            var ignoreSymbols = (flags & TextSearchFlags.IgnoreSymbols) is not 0;
            var converter = _converter = new(ignoreCase, ignoreSymbols);

            var regex = (flags & TextSearchFlags.UseRegex) is not 0;
            // 正規表現が有効な場合は生成して終了
            if (regex)
            {
                try
                {
                    _regex = new(text.ToHalfRegex(ignoreCase), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                    _notEffective = false;
                    return;
                }
                catch
                {
                    // 正規表現の生成に失敗した場合は通常検索に切り替える
                }
            }

            // 検索語の整理
            foreach (var (segment, flag) in text.EnumerateSearchSegments())
            {
                var t1 = converter.Convert(segment);
                input.Add(new(t1, flag));
            }

            _notEffective = input.Count is 0;
        }

        public bool IsMatch(CardPack pack)
        {
            // 発売日
            if (SearchUtils.NotMatch(Date, pack.Date)) return false;
            // ロケール
            if ((_req_ocg && pack.IsTcg) || (_req_tcg && !pack.IsTcg)) return false;
            // 収録カード数
            if (SearchUtils.NotMatch(CardCount, pack.Count)) return false;

            // 名前
            if (_notEffective)
            {
                return true;
            }
            var buffer = StringBuffer.Get();
            var converter = _converter;
            if (_regex is { } regex)
            {
                return SearchUtils.IsMatch(true, pack.Name, buffer, regex, converter);
            }
            else
            {
                return SearchUtils.IsMatch(true, pack.Name, buffer, _input.AsSpan(), converter);
            }
        }

        public static void Copy(PackSearchConditions from, PackSearchConditions to, bool copyText)
        {
            if (copyText)
            {
                to.SearchText = from.SearchText;
            }
            to.TextFlags = from.TextFlags;
            to.CardCount.CopyFrom(from.CardCount);
            to.Date.CopyFrom(from.Date);
            to.DateLocale = from.DateLocale;
        }
    }
}
