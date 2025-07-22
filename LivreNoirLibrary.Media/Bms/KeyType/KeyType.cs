using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms
{
    [TypeConverter(typeof(KeyTypeTypeConverter))]
    [JsonConverter(typeof(KeyTypeJsonConverter))]
    public readonly partial struct KeyType(ChartType type, int keys)
    {
        public readonly ChartType Type = type;
        public readonly int Keys = keys;

        public void Deconstruct(out ChartType type, out int keys)
        {
            type = Type;
            keys = Keys;
        }

        public override string ToString() => $"{Type.ToString().ToLower()}-{Keys}k";

        [GeneratedRegex(@"(?<type>[^-]+)-(?<keys>\d+)")]
        private static partial Regex GR_Parser { get; }

        public static KeyType Parse(string? text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var match = GR_Parser.Match(text);
                if (match.Success)
                {
                    var type = ChartTypeExtensions.Parse(match.Groups["type"].Value);
                    var keys = int.Parse(match.Groups["keys"].Value);
                    return new(type, keys);
                }
            }
            return default;
        }

        public static readonly KeyType Beat_5 = new(ChartType.Beat, 5);
        public static readonly KeyType Beat_10 = new(ChartType.Beat, 10);
        public static readonly KeyType Beat_7 = new(ChartType.Beat, 7);
        public static readonly KeyType Beat_14 = new(ChartType.Beat, 14);

        public static readonly KeyType Pop_3 = new(ChartType.Popn, 3);
        public static readonly KeyType Pop_5 = new(ChartType.Popn, 5);
        public static readonly KeyType Pop_9 = new(ChartType.Popn, 9);
        public static readonly KeyType Pop_18 = new(ChartType.Popn, 18);

        public static readonly KeyType Generic_24 = new(ChartType.Generic, 24);
        public static readonly KeyType Generic_48 = new(ChartType.Generic, 48);
    }
}
