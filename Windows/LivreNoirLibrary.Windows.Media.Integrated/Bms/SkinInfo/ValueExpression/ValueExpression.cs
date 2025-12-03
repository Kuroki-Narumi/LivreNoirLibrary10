using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    [TypeConverter(typeof(ValueExpressionConverter))]
    public sealed partial class ValueExpression : SkinNode
    {
        public ReflectionType Type { get; set => SetValue(ref field, value); }
        public string? Key { get; set => SetValue(ref field, value); }
        public string? Value { get; set => SetValue(ref field, value); }

        public ValueExpression() { }

        public ValueExpression(string text)
        {
            var match = GR_Parser.Match(text);
            var typeKey = match.Groups[1].Value;
            if (match.Success)
            {
                if (Enum.TryParse<ReflectionType>(typeKey, true, out var type))
                {
                    Type = type;
                    Key = match.Groups[2].Value;
                }
                else
                {
                    throw new NotSupportedException($"unknown reference key: {typeKey}");
                }
            }
            else
            {
                Value = text;
            }
        }

        public override string ToString() => Type is 0 ? $"{Value}" : $"${Type}.{Key}";

        [GeneratedRegex(@"^\$([^.\s]+)[$.\s](.+)")]
        private static partial Regex GR_Parser { get; }
    }
}
