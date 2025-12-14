
namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Include : SkinNode, IKeyNode
    {
        public string Key { get; set => SetValue(ref field, value); } = "";
    }
}
