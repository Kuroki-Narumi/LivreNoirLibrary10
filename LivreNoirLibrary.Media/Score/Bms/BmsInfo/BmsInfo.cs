using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public class BmsInfo
    {
        public KeyType KeyType { get; }
        public BarPosition FirstPosition { get; }
        public BarPosition LastPosition { get; }
        public Rational Length { get; }
        public TimeSpan Duration { get; }
        public TimeSpan PlayDuration { get; }
        public long RequiredResolution { get; }

        public string LengthText => Length.ToMixedString();
        public string DurationText => Duration.AutoFormat();
        public string PlayDurationText => PlayDuration.AutoFormat();

        public SortedDictionary<DefType, HashSet<int>> UsedDef { get; }
        public SortedDictionary<int, NotesCountInfo> NotesCount { get; }
        public SortedDictionary<int, NotesCountInfo>? SelectionNotesCount { get; }

        public BmsInfo(BmsData root, BaseData current, Selection? selection = null)
        {
            KeyType = root.GetKeyType();
            RequiredResolution = root.CalcResolution();
            SortedDictionary <DefType, HashSet<int>> def = [];
            SortedDictionary<int, NotesCountInfo> entire = [];
            SortedDictionary<int, NotesCountInfo>? sel = null;
            BarPosition first = BarPosition.MaxValue, last = default, entireLast = default;
            foreach (var d in root.EachData())
            {
                var tl = d.Timeline;
                var isCurrent = ReferenceEquals(root, current);
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

            TimeCounter counter = new(root);
            FirstPosition = first;
            LastPosition = last;
            var lastB = current.GetBeat(entireLast);
            Length = lastB;
            Duration = counter.Beat2TimeSpan(lastB);
            PlayDuration = counter.IntervalTimeSpan(current.GetBeat(first), current.GetBeat(last));

            if (selection is not null && selection.Count is > 0)
            {
                sel = [];
                foreach (var (_, _, note) in selection.EachItem())
                {
                    AddNoteCount(sel, note.Lane, note.Type);
                }
            }

            UsedDef = def;
            NotesCount = entire;
            SelectionNotesCount = sel;
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
