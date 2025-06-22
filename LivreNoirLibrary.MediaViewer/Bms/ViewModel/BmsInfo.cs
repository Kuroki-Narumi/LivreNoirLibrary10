using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsInfo : ObservableObjectBase
    {
        [ObservableProperty(SetterScope = Scope.Private)]
        private KeyType _keyType;
        [ObservableProperty(SetterScope = Scope.Private)]
        private BarPosition _firstPosition;
        [ObservableProperty(SetterScope = Scope.Private)]
        private BarPosition _lastPosition;
        [ObservableProperty(SetterScope = Scope.Private, Related = [nameof(LengthText)])]
        private Rational _length;
        [ObservableProperty(SetterScope = Scope.Private, Related = [nameof(DurationText)])]
        private TimeSpan _duration;
        [ObservableProperty(SetterScope = Scope.Private, Related = [nameof(PlayDurationText)])]
        private TimeSpan _playDuration;
        [ObservableProperty(SetterScope = Scope.Private)]
        private long _requiredResolution;

        private readonly SortedDictionary<DefType, HashSet<int>> _usedDef = [];
        private readonly SortedDictionary<int, NotesCountInfo> _notesCount = [];
        private readonly SortedDictionary<int, NotesCountInfo> _selectionCount = [];

        public string LengthText => Length.ToMixedString();
        public string DurationText => Duration.AutoFormat();
        public string PlayDurationText => PlayDuration.AutoFormat();

        public SortedDictionary<DefType, HashSet<int>> UsedDef => _usedDef;
        public SortedDictionary<int, NotesCountInfo> NotesCount => _notesCount;
        public SortedDictionary<int, NotesCountInfo>? SelectionNotesCount => _selectionCount.Count is 0 ? null : _selectionCount;

        internal void Refresh(BmsData root, BaseData current, Selection selection)
        {
            var def = _usedDef;
            var entire = _notesCount;
            def.Clear();
            entire.Clear();

            BarPosition first = BarPosition.MaxValue, last = default, entireLast = default;
            foreach (var d in root.EachData())
            {
                var tl = d.Timeline;
                var isCurrent = ReferenceEquals(d, current);
                if (tl.LastPosition > entireLast)
                {
                    entireLast = tl.LastPosition;
                }
                foreach (var (pos, note) in d.Timeline)
                {
                    var lane = note.Lane;
                    var type = note.Type;
                    var defType = BmsUtils.GetDefType(lane);
                    if (isCurrent)
                    {
                        AddNoteCount(entire, lane, type);
                    }
                    if (note.IsVisibleKey())
                    {
                        if (pos < first)
                        {
                            first = pos;
                        }
                        if (pos > last)
                        {
                            last = pos;
                        }
                    }
                    if (defType is not 0)
                    {
                        if (!def.TryGetValue(defType, out var set))
                        {
                            set = [];
                            def.Add(defType, set);
                        }
                        set.Add(note.Id);
                    }
                }
            }
            if (first > last)
            {
                first = last;
            }

            KeyType = root.GetKeyType();
            RequiredResolution = root.CalcResolution();
            FirstPosition = first;
            LastPosition = last;
            TimeCounter counter = new(root);
            var lastB = current.GetBeat(entireLast);
            Length = lastB;
            Duration = counter.Beat2TimeSpan(lastB);
            PlayDuration = counter.IntervalTimeSpan(current.GetBeat(first), current.GetBeat(last));

            SendPropertyChanged(nameof(UsedDef));
            SendPropertyChanged(nameof(NotesCount));

            var sel = _selectionCount;
            sel.Clear();
            foreach (var (_, _, note) in selection.EachItem())
            {
                AddNoteCount(sel, note.Lane, note.Type);
            }
            SendPropertyChanged(nameof(SelectionNotesCount));
        }

        private static void AddNoteCount(SortedDictionary<int, NotesCountInfo> dic, int lane, NoteType type)
        {
            if (BmsUtils.IsSoundLane(lane))
            {
                if (dic.TryGetValue(lane, out var current))
                {
                    dic[lane] = current.Add(type);
                }
                else
                {
                    dic.Add(lane, NotesCountInfo.Create(type));
                }
            }
        }
    }
}
