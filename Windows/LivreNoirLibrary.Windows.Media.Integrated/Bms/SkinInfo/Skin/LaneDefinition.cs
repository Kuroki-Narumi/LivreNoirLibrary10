using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public class LaneDefinition : SkinNode
    {
        public string Channel { get; set => SetValue(ref field, value); } = "";
        public ValueExpression? Lane { get; set => SetValue(ref field, value); }
        public ValueExpression Player { get; set => SetValue(ref field, value); } = 1;
    }
}
