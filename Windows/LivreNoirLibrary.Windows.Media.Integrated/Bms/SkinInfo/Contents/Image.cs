using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Image : TextureNode
    {
        public string? Texture { get; set => SetValue(ref field, value); }
        public Stretch Stretch { get; set; } = Stretch.Fill;
    }
}
