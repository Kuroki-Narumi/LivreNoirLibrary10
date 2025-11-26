using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public sealed class BgaElement(Bga source) : GroupElementBase(source)
    {
        private BgaParams? _params;

        public override void Update(in UpdateArgs args)
        {
            base.Update(args);
            _params = args.Bga;
        }

        protected override void RenderChildren(IBitmap target, FloatBitmap buffer1, UnmanagedArray<float> buffer2)
        {
            _params?.Render(target, buffer1, buffer2);
        }
    }
}
