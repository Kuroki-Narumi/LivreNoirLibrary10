using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string Attribute = "属性";

        public const string Light = "光";
        public const string Dark = "闇";
        public const string Water = "水";
        public const string Fire = "炎";
        public const string Earth = "地";
        public const string Wind = "風";
        public const string Divine = "神";

        public const string Attr_Light = $"{Light}{Attribute}";
        public const string Attr_Dark = $"{Dark}{Attribute}";
        public const string Attr_Water = $"{Water}{Attribute}";
        public const string Attr_Fire = $"{Fire}{Attribute}";
        public const string Attr_Earth = $"{Earth}{Attribute}";
        public const string Attr_Wind = $"{Wind}{Attribute}";
        public const string Attr_Divine = $"{Divine}{Attribute}";

        public static string GetName(this Attribute value) => GetEnumName(value, _attr2name);
        public static string GetShortName(this Attribute value) => GetEnumName(value, _attr2name_short);
        public static Attribute GetAttribute(this string? name) => GetEnumValue(name, _name2attr);
        public static bool TryGetAttribute(this string name, out Attribute type) => TryGetEnumValue(name, _name2attr, out type);

        private static readonly Dictionary<Attribute, string> _attr2name = new()
        {
            { YuGiOh.Attribute.Light,  Attr_Light },
            { YuGiOh.Attribute.Dark,   Attr_Dark },
            { YuGiOh.Attribute.Water,  Attr_Water },
            { YuGiOh.Attribute.Fire,   Attr_Fire },
            { YuGiOh.Attribute.Earth,  Attr_Earth },
            { YuGiOh.Attribute.Wind,   Attr_Wind },
            { YuGiOh.Attribute.Divine, Attr_Divine },
        };

        private static readonly Dictionary<string, Attribute> _name2attr = CreateName2Attr();
        private static Dictionary<string, Attribute> CreateName2Attr()
        {
            var dic = _attr2name.Invert();
            foreach (var (attr, name) in _attr2name)
            {
                dic.Add(name.Replace(Attribute, ""), attr);
            }
            AppendEnglishNames(dic);
            return dic;
        }

        private static readonly Dictionary<Attribute, string> _attr2name_short = new()
        {
            { YuGiOh.Attribute.Light,  Light },
            { YuGiOh.Attribute.Dark,   Dark },
            { YuGiOh.Attribute.Water,  Water },
            { YuGiOh.Attribute.Fire,   Fire },
            { YuGiOh.Attribute.Earth,  Earth },
            { YuGiOh.Attribute.Wind,   Wind },
            { YuGiOh.Attribute.Divine, Divine },
        };
    }
}
