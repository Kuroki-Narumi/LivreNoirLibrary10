using System;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Number : TextureNode
    {
        public string? Digits { get; set => SetValue(ref field, value); }
        public string? Padding { get; set => SetValue(ref field, value); }
        public string? Point { get; set => SetValue(ref field, value); }
        public string? Separator { get; set => SetValue(ref field, value); }
        public ValueExpression? Value { get; set => SetValue(ref field, value); }
        public ValueExpression? MinDigits { get; set => SetValue(ref field, value); }
    }
}
