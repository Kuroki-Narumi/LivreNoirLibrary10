using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class TimelineViewModel : ObservableObjectBase
    {
        public event RequestRefreshEventHandler? RequestRefresh;

        public int Radix { get; set => SetValue(ref field, value, OnRadixChanged); } = BmsConstants.Base_Default;
        public IBmsViewModel? Source { get; set => SetValue(ref field, value, OnSourceChanged); }

        public Theme Theme { get; set => SetValue(ref field, value, OnThemeChanged); } = new();
        public int ConductorIndex { get; set => SetValue(ref field, value, OnThemeChanged); }
        public int MetaIndex { get; set => SetValue(ref field, value, OnThemeChanged); }
        public int KeyIndex { get; set => SetValue(ref field, value, OnThemeChanged); }
        public ScratchPosition ScratchPosition { get; set => SetValue(ref field, value, OnThemeChanged); }
        public int BgmCount { get; set => SetValue(ref field, value, OnThemeChanged); }
        public int LaneScale { get; set => SetValue(ref field, value, OnThemeChanged); }
        public bool IsSelectionMoving { get; set => SetValue(ref field, value); }

        public LaneIndexMap LaneIndexMap { get; } = new();
        public ObservableList<NoteViewModel> Notes { get; } = [];
        public ObservableList<NoteViewModel> HiddenNotes { get; } = [];
        public ObservableList<NoteViewModel> InheritedNotes { get; } = [];
        public ObservableList<ProblemItem> Problems { get; } = [];

        private readonly HashSet<(BarPosition, Channel)> _duplicated_channel = [];
        private readonly Dictionary<Channel, NoteViewModel> _lastNotes = [];
        private readonly HashSet<Note> _selectionHash = [];

        private int _lane_pivot;
        private int _lane_offset_min;
        private int _lane_offset_max;
        private double _selection_offset_y;
        private double _selection_offset_min;
        private double _selection_offset_max;
        private readonly Dictionary<Channel, Channel> _channel_map = [];

        protected void RaiseRequestRefreshAll() => RequestRefresh?.Invoke(this, RequestRefreshEventArgs.RefreshAll);
        protected void RaiseRequestRefreshPosition() => RequestRefresh?.Invoke(this, RequestRefreshEventArgs.RefreshPosition);
        protected void RaiseRequestRedraw() => RequestRefresh?.Invoke(this, RequestRefreshEventArgs.Redraw);

        private void OnSourceChanged(IBmsViewModel? oldValue, IBmsViewModel? newValue) => LoadData(newValue);

        private void LoadData(IBmsViewModel? source)
        {
            var notes = Notes;
            var hidden = HiddenNotes;
            var inherited = InheritedNotes;
            var problems = Problems;
            notes.ClearWithoutNotify();
            hidden.ClearWithoutNotify();
            inherited.ClearWithoutNotify();
            problems.ClearWithoutNotify();
            if (source is not null)
            {
                var counter = source.TimeCounter;
                var duplicatedChannel = _duplicated_channel;
                var lastNotes = _lastNotes;
                var radix = Radix;
                var map = LaneIndexMap;

                foreach (var (pos, list) in source.CurrentData.Timeline.EnumerateList())
                {
                    var head = source.GetHead(pos);
                    var absPos = source.GetAbsolutePosition(pos);
                    var time = counter.Beat2Time(absPos);
                    foreach (var note in list.AsSpan())
                    {
                        var ch = note.Channel;
                        NoteViewModel vm = new(pos, head, absPos, time, note, radix);
                        notes.AddWithoutNotify(vm);
                        if (!vm.UpdateVisualParameters(map))
                        {
                            hidden.AddWithoutNotify(vm);
                        }
                        if (!duplicatedChannel.Add((pos, ch)))
                        {
                            AddProblem(ProblemType.Duplicated);
                        }

                        if (note.IsBgm() || note.IsKey())
                        {
                            if (note.IsLongEnd())
                            {
                                if (lastNotes.TryGetValue(ch, out var last))
                                {
                                    last.SetLongEnd(absPos);
                                    lastNotes.Remove(ch);
                                }
                                else if (note.IsKey())
                                {
                                    AddProblem(ProblemType.AloneLongEnd);
                                }
                            }
                            else if (note.IsVisibleKey(false))
                            {
                                lastNotes[ch] = vm;
                            }
                            if (note.IsInvalidMeta())
                            {
                                AddProblem(ProblemType.InvalidMeta);
                            }
                            if (pos.Bar is 0 && note.IsPlayableSound())
                            {
                                AddProblem(ProblemType.ZeroPosition);
                            }
                        }

                        void AddProblem(ProblemType type)
                        {
                            problems.AddWithoutNotify(new(pos, note, type));
                            vm.HasProblem = true;
                        }
                    }
                }

                duplicatedChannel.Clear();
                lastNotes.Clear();
            }
            notes.NotifyCollectionReset();
            hidden.NotifyCollectionReset();
            inherited.NotifyCollectionReset();
            problems.NotifyCollectionReset(); 
            RaiseRequestRefreshAll();
        }

        private void OnRadixChanged(int oldValue, int newValue)
        {
            foreach (var note in Notes.AsSpan())
            {
                note.UpdateValueText(newValue);
            }
            RaiseRequestRefreshPosition();
        }

        private void OnThemeChanged()
        {
            var map = LaneIndexMap;
            map.ApplyTheme(Theme, ConductorIndex, MetaIndex, KeyIndex, ScratchPosition, BgmCount, LaneScale);
            var hidden = HiddenNotes;
            hidden.ClearWithoutNotify();
            foreach (var note in Notes.AsSpan())
            {
                if (!note.UpdateVisualParameters(map))
                {
                    hidden.AddWithoutNotify(note);
                }
            }
            hidden.NotifyCollectionReset();
            RaiseRequestRefreshPosition();
        }

        public void Select(NoteViewModel note, bool isSelected = true)
        {
            note.IsSelected = isSelected;
            RaiseRequestRedraw();
        }

        public void ApplyToSelection(Selection selection)
        {
            selection.Clear();
            foreach (var note in Notes.AsSpan())
            {
                if (note.Note is { } n)
                {
                    selection.Add(note.Position, note.HeadPosition, note.AbsolutePosition, note.Time, n);
                }
            }
        }

        public void LoadSelection(Selection selection)
        {
            var set = _selectionHash;
            selection.GetNoteHash(set);
            foreach (var note in Notes.AsSpan())
            {
                note.IsSelected = note.Note is { } n && set.Contains(n);
            }
            RaiseRequestRedraw();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SelectNote(NoteViewModel note, bool isSelected, Selection selection)
        {
            note.IsSelected = isSelected;
            if (isSelected && note.Note is { } n)
            {
                selection.Add(note.Position, note.HeadPosition, note.AbsolutePosition, note.Time, n);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void UnSelectNote(NoteViewModel note, bool unselect, Selection selection)
        {
            if (unselect)
            {
                note.IsSelected = false;
                if (note.Note is { } n)
                {
                    selection.Remove(n);
                }
            }
        }

        public void SelectAll(bool isSelected, Selection selection)
        {
            selection.Clear();
            foreach (var note in Notes.AsSpan())
            {
                SelectNote(note, isSelected, selection);
            }
            RaiseRequestRedraw();
        }

        public void SelectRange(in Rect rect, SelectionMode mode, Selection selection)
        {
            switch (mode)
            {
                case SelectionMode.New:
                    SelectNew(rect, selection);
                    break;
                case SelectionMode.Union:
                    SelectUnion(rect, selection);
                    break;
                case SelectionMode.Except:
                    SelectExcept(rect, selection);
                    break;
                case SelectionMode.Intersect:
                    SelectIntersect(rect, selection);
                    break;
            }
        }

        public void SelectNew(in Rect rect, Selection selection)
        {
            selection.Clear();
            foreach (var note in Notes.AsSpan())
            {
                SelectNote(note, note.Intersects(rect), selection);
            }
            RaiseRequestRedraw();
        }

        public void SelectUnion(in Rect rect, Selection selection)
        {
            foreach (var note in Notes.AsSpan())
            {
                SelectNote(note, note.Intersects(rect), selection);
            }
            RaiseRequestRedraw();
        }

        public void SelectExcept(in Rect rect, Selection selection)
        {
            foreach (var note in Notes.AsSpan())
            {
                UnSelectNote(note, note.Intersects(rect), selection);
            }
            RaiseRequestRedraw();
        }

        public void SelectIntersect(in Rect rect, Selection selection)
        {
            foreach (var note in Notes.AsSpan())
            {
                UnSelectNote(note, !note.Intersects(rect), selection);
            }
            RaiseRequestRedraw();
        }

        public void StartSelectionMove(int pivotLane)
        {

        }
    }
}
