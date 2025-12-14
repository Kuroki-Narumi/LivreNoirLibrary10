using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class NoteLane : SkinNode
    {
        public int Lane { get; set => SetValue(ref field, value); }
        public ValueExpression? X { get; set => SetValue(ref field, value); }
        public ValueExpression? Width { get; set => SetValue(ref field, value); }
        public ValueExpression? Note { get; set => SetValue(ref field, value); }
        public ValueExpression? LongHead { get; set => SetValue(ref field, value); }
        public ValueExpression? LongTail { get; set => SetValue(ref field, value); }
        public ValueExpression? LongBody { get; set => SetValue(ref field, value); }
        public ValueExpression? ActiveLongBody { get; set => SetValue(ref field, value); }
        public ValueExpression? Mine { get; set => SetValue(ref field, value); }
    }
}
