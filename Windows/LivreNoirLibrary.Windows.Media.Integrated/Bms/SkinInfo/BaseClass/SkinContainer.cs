using System.Windows.Markup;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    [ContentProperty(nameof(Children))]
    public partial class SkinContainer : SkinNode
    {
        public ObservableList<SkinNode> Children { get; } = [];
    }
}
