using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public interface IRectNode
    {
        public ValueExpression? X { get; set; }
        public ValueExpression? Y { get; set; }
        public ValueExpression? Width { get; set; }
        public ValueExpression? Height { get; set; }
    }
}
