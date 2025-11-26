using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using System;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public abstract class GroupElementBase(SkinElement source) : ScreenElement(source)
    {
        private readonly FloatBitmap _buffer = new(0, 0);

        protected override bool TryGetBitmap([MaybeNullWhen(false)] out IBitmap bitmap, out Rectangle rect, FloatBitmap buffer1, UnmanagedArray<float> buffer2)
        {
            var w = (int)DestWidth;
            var h = (int)DestHeight;
            var buffer = _buffer;
            buffer.Resize(w, h, false);
            buffer.Clear();
            RenderChildren(buffer, buffer1, buffer2);
            bitmap = _buffer;
            rect = buffer.Rect;
            return true;
        }

        protected abstract void RenderChildren(IBitmap target, FloatBitmap buffer1, UnmanagedArray<float> buffer2);
    }
}
