using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Scraping
{
    public static class LocaleSuffix
    {
        public const string Japanese = "ja";
        public const string Korean = "ko";
        public const string Asian = "ae";

        public const string English = "en";
        public const string Deutsch = "de";
        public const string Francais = "fr";
        public const string Italiano = "it";
        public const string Espanol = "es";
        public const string Portugues = "pt";

        private static readonly Dictionary<LocaleType, string> _dic = new()
        {
            { LocaleType.Japanese, Japanese },
            { LocaleType.Korean, Korean },
            { LocaleType.Asian, Asian },
            { LocaleType.English, English },
            { LocaleType.Deutsch, Deutsch },
            { LocaleType.Francais, Francais },
            { LocaleType.Italiano, Italiano },
            { LocaleType.Espanol, Espanol },
            { LocaleType.Portugues, Portugues },
        };

        public static string Get(bool tcg) => tcg ? English : Japanese;
        public static string Get(LocaleType type) => _dic[type];
    }

    public enum LocaleType
    {
        Japanese,
        Korean,
        Asian,
        English,
        Deutsch,
        Francais,
        Italiano,
        Espanol,
        Portugues,
    }
}
