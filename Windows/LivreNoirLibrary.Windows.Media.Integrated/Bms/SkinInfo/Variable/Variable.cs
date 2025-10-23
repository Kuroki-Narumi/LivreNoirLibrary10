using System;
using System.Windows.Markup;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    [ContentProperty(nameof(Converters))]
    public sealed class Variable : SkinNode, IKeyNode
    {
        public string Key { get; set => SetValue(ref field, value); } = "";
        public ValueExpression? Value { get; set => SetValue(ref field, value); }
        public ConvertCollection Converters { get; } = [];

        public string? GetActualValue(string? value)
        {
            return value is not null && Converters.TryGetValue(value, out var result) ? result.To : value;
        }
    }
}
