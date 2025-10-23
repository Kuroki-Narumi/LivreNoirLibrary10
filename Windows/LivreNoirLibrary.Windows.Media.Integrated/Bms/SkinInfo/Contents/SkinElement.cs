using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class SkinElement : SkinContainer
    {
        public TimerId DestTimer { get; set => SetValue(ref field, value); }
        public ValueExpression? DestLoopStart { get; set => SetValue(ref field, value); }
        public ValueExpression? DestLoopEnd { get; set => SetValue(ref field, value); }
    }
}
