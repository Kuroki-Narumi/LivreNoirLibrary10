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

        public const string Direction_Delim = ",";

        public static string GetName(this LinkDirection value, string delim = Direction_Delim)
        {
            List<string> names = [];
            foreach (var (link, name) in _link2name)
            {
                if ((value & link) is not 0)
                {
                    names.Add(name);
                }
            }
            return string.Join(delim, names);
        }

        public static LinkDirection GetDirection(this string? name, string delim = Direction_Delim)
        {
            if (name is not null)
            {
                if (_name2link.TryGetValue(name, out var value))
                {
                    return value;
                }
                else
                {
                    return GetDirection(name.Split(delim));
                }
            }
            return 0;
        }

        public static LinkDirection GetDirection(this IEnumerable<string> names)
        {
            LinkDirection value = 0;
            foreach (var name in names)
            {
                if (_name2link.TryGetValue(name, out var val))
                {
                    value |= val;
                }
            }
            return value;
        }

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

        private static readonly Dictionary<string, LinkDirection> _name2link = _link2name.Invert();
    }
}
