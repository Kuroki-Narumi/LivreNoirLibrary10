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
    public class PackSearchConditions : TextSearchConditions
    {
        public static PackSearchConditions Default { get; } = new();

        [JsonPropertyName(JsonPropertyNames.Search_Count)]
        public NumberRange CardCount { get; set; } = new(0, 999, false, false);

        [JsonPropertyName(JsonPropertyNames.Search_FirstDate)]
        public DateRange Date { get; set; } = new();

        [JsonPropertyName(JsonPropertyNames.Search_DateLocale)]
        public LocaleType DateLocale { get; set; } = 0;

        private bool _req_ocg;
        private bool _req_tcg;

        public void Prepare()
        {
            var locale = DateLocale;
            _req_ocg = locale is LocaleType.Ocg;
            _req_tcg = locale is LocaleType.Tcg;

            PrepareText();
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
            return IsTextMatch(pack);
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
