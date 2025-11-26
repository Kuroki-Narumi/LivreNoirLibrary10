using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public sealed class GroupElement(Group source) : GroupElementBase(source)
    {
        private readonly Group _source = source;

        public bool ClipToBounds { get; private set; }
        public List<ScreenElement> Children { get; } = [];

        public override void DetermineExpressions(Skin skin, IVariableProvider? provider)
        {
            base.DetermineExpressions(skin, provider);
            foreach (var child in Children.AsSpan())
            {
                child.DetermineExpressions(skin, provider);
            }
            ClipToBounds = _source.ClipToBounds;
        }

        public override void Update(in UpdateArgs args)
        {
            base.Update(args);
            foreach (var child in Children.AsSpan())
            {
                child.Update(args);
            }
        }

        protected override void RenderChildren(IBitmap target, FloatBitmap buffer1, UnmanagedArray<float> buffer2)
        {
            foreach (var child in Children.AsSpan())
            {
                child.Render(target, buffer1, buffer2);
            }
        }
    }
}
