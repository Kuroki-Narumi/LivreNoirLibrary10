using System;
using System.Collections.Generic;
using System.Xml.Linq;

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

        private static string[] Attr2Name { get; } = [None, Attr_Light, Attr_Dark, Attr_Water, Attr_Fire, Attr_Earth, Attr_Wind, Attr_Divine];
        private static string[] Attr2ShortName { get; } = [None, Light, Dark, Water, Fire, Earth, Wind, Divine];

        private static Dictionary<string, Attribute>.AlternateLookup<ReadOnlySpan<char>> Name2Attr { get; } = CreateName2Attr();
        private static Dictionary<string, Attribute>.AlternateLookup<ReadOnlySpan<char>> CreateName2Attr()
        {
            var dic = CreateInvertedDictionary<Attribute>();
            var ary1 = Attr2Name;
            var ary2 = Attr2ShortName;
            foreach (var value in EnumUtils.Attributes)
            {
                var index = (int)value;
                dic[ary1[index]] = value;
                dic[ary2[index]] = value;
                dic[value.ToString()] = value;
            }
            return dic;
        }

        public static string GetName(this Attribute value) => GetEnumName(value, (int)value, Attr2Name);
        public static string GetShortName(this Attribute value) => GetEnumName(value, (int)value, Attr2ShortName);
        public static Attribute GetAttribute(ReadOnlySpan<char> name) => GetEnumValue(name, Name2Attr);
    }
}
