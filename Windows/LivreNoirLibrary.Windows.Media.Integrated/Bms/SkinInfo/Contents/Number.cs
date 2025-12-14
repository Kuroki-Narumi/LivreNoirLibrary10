using System;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Number : TextureNode
    {
        public ValueExpression? Digits { get; set => SetValue(ref field, value); }
        public ValueExpression? Padding { get; set => SetValue(ref field, value); }
        public ValueExpression? Point { get; set => SetValue(ref field, value); }
        public ValueExpression? Separator { get; set => SetValue(ref field, value); }
        public ValueExpression? Value { get; set => SetValue(ref field, value); }
        public ValueExpression? MinDigits { get; set => SetValue(ref field, value); }
    }
}
