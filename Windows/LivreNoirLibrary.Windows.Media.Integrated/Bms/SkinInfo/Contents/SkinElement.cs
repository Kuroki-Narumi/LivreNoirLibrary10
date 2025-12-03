using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class SkinElement : SkinContainer
    {
        public TimerId DestTimer { get; set => SetValue(ref field, value); } = TimerId.Scene_Start;
        public ValueExpression? DestLoopStart { get; set => SetValue(ref field, value); }
        public ValueExpression? DestLoopEnd { get; set => SetValue(ref field, value); }
        public BlendMode Blend { get; set => SetValue(ref field, value); } = BlendMode.Alpha;
    }
}
