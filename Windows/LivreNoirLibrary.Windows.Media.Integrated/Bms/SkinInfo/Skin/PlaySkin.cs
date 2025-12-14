using System;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class PlaySkin : Skin
    {
        public Int32Collection KeyCount { get; set => SetValue(ref field, value); } = [];
        public ValueExpression LoadTime { get; set => SetValue(ref field, value); } = 2;
        public ValueExpression ReadyTime { get; set => SetValue(ref field, value); } = 1;
        public ValueExpression MarginTime { get; set => SetValue(ref field, value); } = 3;
    }
}
