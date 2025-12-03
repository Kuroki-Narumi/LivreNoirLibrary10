using LivreNoirLibrary.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public sealed class BgaElement(Bga source) : GroupElementBase(source)
    {
        private BgaSource? _params;

        public override void Update(in UpdateArgs args)
        {
            base.Update(args);
            _params = args.Bga;
        }

        protected override void RenderChildren(in RenderArgs args)
        {
            _params?.Render(args);
        }
    }
}
