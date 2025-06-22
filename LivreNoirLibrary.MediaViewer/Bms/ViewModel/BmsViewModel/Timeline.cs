using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsViewModel
    {
        public event EventHandler? RequestRefreshNotes;
        public event EventHandler? RequestRenderNotes;

        private readonly Selection _selection = [];
        private readonly List<NoteViewModel> _notes = [];
        private readonly List<NoteViewModel> _inheritedNotes = [];
        private readonly ObservableList<ProblemItem> _problems = [];
        private bool _updatingNotes;
        [ObservableProperty]
        private bool _isSelectionMoving;

        public ReadOnlySpan<NoteViewModel> Notes => CollectionsMarshal.AsSpan(_notes);
        public ReadOnlySpan<NoteViewModel> InheritedNotes => CollectionsMarshal.AsSpan(_inheritedNotes);
        public IList<ProblemItem> Problems => _problems;
        public bool IsSelectionEmpty => _selection.IsEmpty();

        private void RaiseRequestRefreshNotes() => RequestRefreshNotes?.Invoke(this, EventArgs.Empty);
        private void RaiseRequestRenderNotes() => RequestRenderNotes?.Invoke(this, EventArgs.Empty);

        private void OnIsSelectionMovingChanged() => RaiseRequestRenderNotes();

        private void RefreshNotes(BaseData source)
        {
            _updatingNotes = true;
            var notes = _notes;
            foreach (var note in CollectionsMarshal.AsSpan(notes))
            {
                note.IsSelectedChanged -= IsSelectedChanged_Note;
            }
            var inherited = _inheritedNotes;
            var problems = _problems;
            notes.Clear();
            inherited.Clear();
            problems.ClearWithoutNotify();
            var selection = _selection;
            var hash = selection.GetNoteHash();
            selection.Clear();

            HashSet<(BarPosition, int)> duplicated = [];
            Dictionary<int, NoteViewModel> lastNotes = [];

            var radix = source.Base;

            foreach (var (pos, beat, note) in source.EachNote())
            {
                var lane = note.Lane;
                NoteViewModel vm = new(pos, beat, note, radix)
                {
                    IsSelected = hash.Contains(note),
                };
                vm.IsSelectedChanged += IsSelectedChanged_Note;
                if (vm.IsSelected)
                {
                    selection.Add(pos, beat, note);
                }
                void AddProblem(ProblemType type)
                {
                    problems.Add(new(pos, lane, type));
                    vm.HasProblem = true;
                }
                notes.Add(vm);
                if (note.IsLongEnd())
                {
                    if (lastNotes.TryGetValue(lane, out var last))
                    {
                        last.SetLongEnd(beat);
                        lastNotes.Remove(lane);
                    }
                    else if (note.IsKey())
                    {
                        AddProblem(ProblemType.AloneLongEnd);
                    }
                }
                else if (note.IsVisibleKey())
                {
                    lastNotes[lane] = vm;
                }
                if (note.IsInvalidMeta())
                {
                    AddProblem(ProblemType.InvalidMeta);
                }
                if (!duplicated.Add((pos, lane)))
                {
                    AddProblem(ProblemType.Duplicated);
                }
                if (pos.Bar is 0 && note.IsKey() && !note.IsInvisible())
                {
                    AddProblem(ProblemType.ZeroPosition);
                }
            }
            problems.NotifyCollectionReset();

            // inherited
            lastNotes.Clear();
            foreach (var (pos, beat, note) in source.EachNote(true))
            {
                var lane = note.Lane;
                NoteViewModel vm = new(pos, beat, note, radix);
                inherited.Add(vm);
                if (note.IsLongEnd())
                {
                    if (lastNotes.TryGetValue(lane, out var last))
                    {
                        last.SetLongEnd(beat);
                        lastNotes.Remove(lane);
                    }
                }
                else if (note.IsVisibleKey())
                {
                    lastNotes[lane] = vm;
                }
            }
            RaiseRequestRefreshNotes();
            _updatingNotes = false;
        }

        private void IsSelectedChanged_Note(NoteViewModel item, bool isSelected)
        {
            if (!_updatingNotes)
            {
                var selection = _selection;
                if (selection.TryFind(item.Note, out var actualItem))
                {
                    if (!isSelected)
                    {
                        selection.Remove(actualItem.ActualPosition, actualItem);
                        RaiseRequestRenderNotes();
                    }
                }
                else if (isSelected)
                {
                    _selection.Add(item.Position, item.ActualPosition, item.Note);
                    RaiseRequestRenderNotes();
                }
            }
        }

        private void RefreshBarPosition(BaseData source)
        {
            var selection = _selection;
            selection.Clear();
            foreach (var item in CollectionsMarshal.AsSpan(_notes))
            {
                item.Position = source.GetPosition(item.ActualPosition);
                if (item.IsSelected)
                {
                    selection.Add(item.Position, item.ActualPosition, item.Note);
                }
            }
            foreach (var item in CollectionsMarshal.AsSpan(_inheritedNotes))
            {
                item.Position = source.GetPosition(item.ActualPosition);
            }
            RaiseRequestRefreshNotes();
        }

        private void RestoreSelection(SelectionHistoryData source)
        {
            _updatingNotes = true;
            var selection = _selection;
            selection.Clear();
            var notes = _notes;
            for (var i = notes.Count - 1; i >= 0; i--)
            {
                var item = notes[i];
                var pos = item.Position;
                if (source.TryGetValue(pos, out var list))
                {
                    var note = item.Note;
                    var index = list.FindIndex(note.Equals);
                    if (index is >= 0)
                    {
                        item.IsSelected = true;
                        selection.Add(pos, item.ActualPosition, note);
                        list.RemoveAt(index);
                        if (list.Count is 0)
                        {
                            source.Remove(pos);
                        }
                    }
                }
            }
            RaiseRequestRenderNotes();
            _updatingNotes = false;
        }
    }
}
