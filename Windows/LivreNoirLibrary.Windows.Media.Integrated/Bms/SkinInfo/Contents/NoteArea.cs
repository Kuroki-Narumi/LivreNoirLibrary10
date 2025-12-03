using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public class NoteArea : SkinElement
    {
        public string? BarLine { get; set => SetValue(ref field, value); }
        public string? JudgeLine { get; set => SetValue(ref field, value); }
        public ValueExpression? BaseHeight { get; set => SetValue(ref field, value); }
    }
}
