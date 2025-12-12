using LivreNoirLibrary.Media.Bms.Play;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public abstract class TextureNode : SkinElement
    {
        public TimerId SourceTimer { get; set => SetValue(ref field, value); } = TimerId.Scene_Start;
    }
}
