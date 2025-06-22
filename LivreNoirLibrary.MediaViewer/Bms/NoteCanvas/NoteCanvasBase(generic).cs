using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract partial class NoteCanvasBase<T> : NoteCanvasBase
        where T : RectBase
    {
        protected readonly List<T> _children = [];
        protected readonly List<T> _visible = [];

        public ReadOnlySpan<T> Children => CollectionsMarshal.AsSpan(_children);
        public ReadOnlySpan<T> VisibleChildren => CollectionsMarshal.AsSpan(_visible);

        public void Clear()
        {
            _children.Clear();
            _visible.Clear();
            ReserveViewportRefresh();
        }

        protected override void RefreshVertical()
        {
            var bottom = Bottom;
            var sy = ScaleY;
            foreach (var item in CollectionsMarshal.AsSpan(_children))
            {
                item.SetVertical(bottom, sy);
            }
        }

        protected override void RefreshVisible()
        {
            _visible.Clear();
            Rect rect = new(_vx, _vy, _vw, _vh);
            foreach (var item in CollectionsMarshal.AsSpan(_children))
            {
                if (item.Intersects(rect))
                {
                    _visible.Add(item);
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            foreach (var item in CollectionsMarshal.AsSpan(_visible))
            {
                RenderItem(drawingContext, item);
            }
        }

        protected abstract void RenderItem(DrawingContext drawingContext, T item);

        public bool HitTest(Point point, [MaybeNullWhen(false)] out T rect)
        {
            RefreshChildrenIfNeeded();
            var span = CollectionsMarshal.AsSpan(_visible);
            for (int i = span.Length - 1; i >= 0; i--)
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
