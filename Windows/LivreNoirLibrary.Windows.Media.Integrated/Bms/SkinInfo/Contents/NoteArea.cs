using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public class NoteArea : SkinElement
    {
        public ValueExpression? BarLine { get; set => SetValue(ref field, value); }
        public ValueExpression? JudgeLine { get; set => SetValue(ref field, value); }
        public ValueExpression? BaseHeight { get; set => SetValue(ref field, value); }
    }
}
