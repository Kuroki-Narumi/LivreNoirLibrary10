
namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class IncludeSource : SkinNode, IKeyNode
    {
        public string Key { get; set => SetValue(ref field, value); } = "";
        public string? Source { get => field; set => SetValue(ref field, value); }

        public override string ToString() => $"{nameof(IncludeSource)}{{Key={Key}, Source={Source}}}";
    }
}
