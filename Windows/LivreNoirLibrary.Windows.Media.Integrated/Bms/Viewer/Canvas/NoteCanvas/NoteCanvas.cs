using System;
using System.Collections.Generic;
using System.Windows.Media;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class NoteCanvas : BmsNoteCanvas
    {
        private bool _selectionMoving;
        private readonly List<NoteRect> _selected = [];

        protected override void OnRender(DrawingContext drawingContext)
        {
            _selectionMoving = ViewModel is { } v && v.IsSelectionMoving;
            var selected = _selected;
            selected.Clear();

            base.OnRender(drawingContext);

            if (selected.Count is > 0)
            {
                drawingContext.PushOpacity(0.8);
                foreach (var note in selected.AsSpan())
                {
                    note.RenderSelectionMoving(drawingContext, this);
                }
                drawingContext.Pop();
            }
        }

        protected override void RenderItem(DrawingContext drawingContext, NoteRect item)
        {
            if (_selectionMoving && item.IsSelected)
            {
                _selected.Add(item);
            }
            else
            {
                base.RenderItem(drawingContext, item);
            }
        }
    }
}
