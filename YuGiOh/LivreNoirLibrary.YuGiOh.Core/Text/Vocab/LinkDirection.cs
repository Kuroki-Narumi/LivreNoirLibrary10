using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string Marker = "マーカー";
        public const string LinkMarker = $"{Link}{Marker}";

        public const string Direction_1 = "左下";
        public const string Direction_2 = "下";
        public const string Direction_3 = "右下";
        public const string Direction_4 = "左";
        public const string Direction_6 = "右";
        public const string Direction_7 = "左上";
        public const string Direction_8 = "上";
        public const string Direction_9 = "右上";

        private static readonly Dictionary<LinkDirection, string> _link2name = new()
        {
            { LinkDirection.LowerLeft, Direction_1 },
            { LinkDirection.Lower, Direction_2 },
            { LinkDirection.LowerRight, Direction_3 },
            { LinkDirection.Left, Direction_4 },
            { LinkDirection.Right, Direction_6 },
            { LinkDirection.UpperLeft, Direction_7 },
            { LinkDirection.Upper, Direction_8 },
            { LinkDirection.UpperRight, Direction_9 },
        };

        private static readonly Dictionary<LinkDirection, List<string>> _link2names = [];

        private static readonly Dictionary<string, LinkDirection>.AlternateLookup<ReadOnlySpan<char>> _name2link = CreateInvertedDictionary(_link2name);

        public static ReadOnlySpan<string> GetNames(this LinkDirection value)
        {
            if (!_link2names.TryGetValue(value, out var list))
            {
                list = [];
                foreach (var (link, name) in _link2name)
                {
                    if ((value & link) is not 0)
                    {
                        list.Add(name);
                    }
                }
                _link2names[value] = list;
            }
            return list.AsSpan();
        }

        public static string GetName(this LinkDirection value) => string.Join('/', GetNames(value));

        public static LinkDirection GetDirection(this ReadOnlySpan<char> text)
        {
            var result = LinkDirection.None;
            foreach (var range in text.Split(Separators))
            {
                var name = text[range].Trim();
                if (TryGetEnumValue(name, _name2link, out var val))
                {
                    result |= val;
                }
            }
            return result;
        }
    }
}
