using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract class NoteCanvasBase<T> : NoteCanvasBase
        where T : NoteRectBase
    {
        protected readonly List<T> _children = [];

        public ReadOnlySpan<T> Children => _children.AsSpan();

        protected override void RefreshVertical()
        {
            var headHeight = HeadHeight;
            var bottom = Bottom;
            var sy = ScaleY;
            foreach (var item in Children)
            {
                item.UpdateVertical(headHeight, bottom, sy);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Rect rect = new(ViewportLeft, ViewportTop, ViewportWidth, ViewportHeight);
            foreach (var item in Children)
            {
                if (item.Intersects(rect))
                {
                    RenderItem(drawingContext, item);
                }
            }
        }

        protected abstract void RenderItem(DrawingContext drawingContext, T item);

        public bool HitTest(Point point, [MaybeNullWhen(false)] out T rect)
        {
            var span = Children;
            for (var i = span.Length - 1; i >= 0; i--)
            {
                var item = span[i];
                if (item.Contains(point))
                {
                    rect = item;
                    return true;
                }
            }
            rect = null;
            return false;
        }
    }
}
