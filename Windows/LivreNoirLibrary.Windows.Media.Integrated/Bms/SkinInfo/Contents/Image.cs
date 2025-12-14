using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Image : TextureNode, IStretchElement
    {
        public ValueExpression? Texture { get; set => SetValue(ref field, value); }
        public Stretch Stretch { get; set; } = Stretch.Fill;
        public ValueExpression? MaxWidth { get; set; }
        public ValueExpression? MaxHeight { get; set; }
    }
}
