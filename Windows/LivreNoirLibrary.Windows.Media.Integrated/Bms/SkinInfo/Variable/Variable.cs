using System;
using System.Windows.Markup;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    [ContentProperty(nameof(Converters))]
    public sealed class Variable : SkinNode, IKeyNode
    {
        public string Key { get; set => SetValue(ref field, value); } = "";
        public ValueExpression? Source { get; set => SetValue(ref field, value); }
        public ValueExpression? DefaultValue { get; set => SetValue(ref field, value); }
        public ConvertCollection Converters { get; } = [];

        public override string ToString() => $"{nameof(Variable)}{{Key={Key}, Source={Source}, DefaultValue={DefaultValue}}}";
    }
}
