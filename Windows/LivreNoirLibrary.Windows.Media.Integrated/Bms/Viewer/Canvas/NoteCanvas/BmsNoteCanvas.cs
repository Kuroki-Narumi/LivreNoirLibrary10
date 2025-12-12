using System;
using System.Windows.Media;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract partial class BmsNoteCanvas : NoteCanvasBase<NoteRect>
    {
        [DependencyProperty]
        private TimelineViewModel? _viewModel;

        private void OnViewModelChanged(TimelineViewModel? oldValue, TimelineViewModel? newValue)
        {
            if (oldValue is not null)
            {
                oldValue.RequestRefresh -= OnRequestRefresh;
            }
            if (newValue is not null)
            {
                newValue.RequestRefresh += OnRequestRefresh;
                RefreshChildren();
            }
        }

        private void OnRequestRefresh(object sender, RequestRefreshEventArgs e)
        {
            switch (e.Type)
            {
                case RequestRefreshType.RefreshAll:
                    RefreshChildren();
                    break;
                case RequestRefreshType.RefreshPosition:
                    UpdateVisual();
                    break;
                case RequestRefreshType.Redraw:
                    InvalidateVisual();
                    break;
            }
        }

        protected virtual void RefreshChildren()
        {
            if (_viewModel is { } vm)
            {
                var sourceSpan = vm.Notes.AsSpan();
                var children = _children;
                children.EnsureCapacity(sourceSpan.Length);
                var childSpan = Children;
                var count = children.Count;
                for (var i = 0; i < count; i++)
                {
                    childSpan[i].ViewModel = sourceSpan[i];
                }
                var dif = count - sourceSpan.Length;
                if (dif is < 0)
                {
                    foreach (var note in sourceSpan[count..])
                    {
                        children.Add(new(note));
                    }
                }
                else if (dif is > 0)
                {
                    children.RemoveRange(sourceSpan.Length, dif);
                }
            }
            else
            {
                _children.Clear();
            }
        }

        protected override void RefreshVisible() { }

        protected virtual void UpdateVisual()
        {
            foreach (var child in Children)
            {
                child.UpdateVisual();
            }
        }

        protected override void RenderItem(DrawingContext drawingContext, NoteRect item)
        {
            item.Render(drawingContext, this);
        }
    }
}
