using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public class NoteArea : SkinElement
    {
        public string? BarLine { get; set => SetValue(ref field, value); }
    }
}
