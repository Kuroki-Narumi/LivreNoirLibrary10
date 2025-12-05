using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Image : TextureNode
    {
        public string? Texture { get; set => SetValue(ref field, value); }
    }
}
