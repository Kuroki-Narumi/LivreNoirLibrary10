using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class NoteLane : SkinNode
    {
        public int Lane { get; set => SetValue(ref field, value); }
        public ValueExpression? X { get; set => SetValue(ref field, value); }
        public ValueExpression? Width { get; set => SetValue(ref field, value); }
        public string? Note { get; set => SetValue(ref field, value); }
        public string? LongHead { get; set => SetValue(ref field, value); }
        public string? LongTail { get; set => SetValue(ref field, value); }
        public string? LongBody { get; set => SetValue(ref field, value); }
        public string? ActiveLongBody { get; set => SetValue(ref field, value); }
        public string? Mine { get; set => SetValue(ref field, value); }
    }
}
