using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Bms.Play;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Judge : SkinElement
    {
        public ValueExpression Player { get; set => SetValue(ref field, value); } = 0;
        public ValueExpression? Padding { get; set => SetValue(ref field, value); }
    }

    public partial class JudgeTexture : SkinNode
    {
        public JudgeType Type { get; set => SetValue(ref field, value); }
        public ValueExpression? Texture { get; set => SetValue(ref field, value); }
        public ValueExpression? ComboTexture { get; set => SetValue(ref field, value); }
    }
}
