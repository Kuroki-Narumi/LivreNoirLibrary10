using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public interface IStretchElement
    {
        public Stretch Stretch { get; }
        public ValueExpression? MaxWidth { get; }
        public ValueExpression? MaxHeight { get; }
    }
}
