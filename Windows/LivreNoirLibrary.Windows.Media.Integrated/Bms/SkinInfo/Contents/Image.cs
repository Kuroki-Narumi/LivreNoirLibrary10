using System;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Image : SkinElement
    {
        public string? Texture { get; set => SetValue(ref field, value); }
        public TimerId SourceTimer { get; set => SetValue(ref field, value); }
    }
}
