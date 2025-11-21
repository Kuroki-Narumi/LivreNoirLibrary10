using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public interface IRectNode
    {
        ValueExpression? X { get; set; }
        ValueExpression? Y { get; set; }
        ValueExpression? Width { get; set; }
        ValueExpression? Height { get; set; }
    }
}
