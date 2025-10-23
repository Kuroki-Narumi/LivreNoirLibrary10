using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Bmson;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class IBmsDataExtensions
    {
        public static Selection CreateSelection(this IBmsData data, IEnumerable<(BarPosition, INote)> notes)
        {
            Selection selection = [];
            foreach (var (p, v) in notes)
            {
                selection.Add(data.GetHead(p), data.GetAbsolutePosition(p), v);
            }
            return selection;
        }

        public static Selection SelectRange(this IBmsData data, BarPosition? first, BarPosition? last, bool removeSelected = false)
        {
            var range = RangeUtils.GetAuto(first, last);
            var notes = data.Timeline.Range(range);
            var selection = CreateSelection(data, notes);
            if (removeSelected)
            {
                RemoveSelection(data, selection);
            }
            return selection;
        }

        public static void RemoveSelection(this IBmsData data, Selection selection)
        {
            var timeline = data.Timeline;
            foreach (var (_, p, n) in selection)
            {
                timeline.Remove(data.GetBarPosition(p), n);
            }
        }

        public static void AddSelection(this IBmsData data, Selection selection)
        {
            var timeline = data.Timeline;
            foreach (var (_, p, n) in selection)
            {
                timeline.Add(data.GetBarPosition(p), n);
            }
        }

        public static void AddSelection(this IBmsData data, Selection selection, Rational offset)
        {
            var timeline = data.Timeline;
            foreach (var (_, p, n) in selection)
            {
                timeline.Add(data.GetBarPosition(p + offset), n);
            }
        }

        public static void AddSelection(this IBmsData data, Selection selection, int barStart) => AddSelection(data, selection, data.GetHead(barStart));

        public static bool ReplaceSelection(this IBmsData data, Selection selection, int asmId, out Selection newSelection)
        {
            var initialized = false;
            var head = BarPosition.Zero;
            var headLane = -1;
            HashSet<int> longHeadLanes = [];
            var timeline = data.Timeline;
            foreach (var (_, p, note) in selection)
            {
                if (note is ISoundNote n)
                {
                    var lane = n.Lane;
                    var isNormal = n.IsNormal();
                    var bp = data.GetBarPosition(p);
                    if ((n.IsLongEnd() && longHeadLanes.Contains(lane)) || isNormal)
                    {
                        timeline.Remove(bp, n);
                        if (isNormal)
                        {
                            longHeadLanes.Add(lane);
                        }
                        else
                        {
                            longHeadLanes.Remove(lane);
                        }
                        if (!initialized || bp < head)
                        {
                            head = bp;
                            headLane = n.Lane;
                            initialized = true;
                        }
                    }
                }
            }
            if (initialized)
            {
                SoundNote headNote = new(headLane, asmId);
                timeline.Add(head, headNote);
                newSelection = [new(data.GetHead(head), data.GetAbsolutePosition(head), headNote)];
                return true;
            }
            else
            {
                newSelection = selection;
                return false;
            }
        }

        private record NoteCache(BarPosition Position, ISoundNote Note);

        public static bool ReplaceSelectionAll(this IBmsData data, Selection selection, int asmId, int marginMs, out Selection newSelection)
        {
            // preparation
            TimeCounter counter = new(data);
            if (!selection.TryGetFirstSound(out var firstItem, false))
            {
                newSelection = selection;
                return false;
            }
            var selectionOffset = firstItem.AbsolutePosition;
            var offset = counter.Beat2Time(selectionOffset);
            List<(decimal Offset, string DefValue)> noteList = [];
            HashSet<int> longHeadLanes = [];
            string GetDefValue(int id) => data.TryGetDef(DefType.Wav, id, out var value) ? value : $@"\\\***{id}***\\\";
            foreach (var (_, p, note) in selection)
            {
                if (note is ISoundNote n)
                {
                    var lane = n.Lane;
                    var isNormal = n.IsNormal();
                    if (isNormal || (n.IsLongEnd() && longHeadLanes.Contains(lane)))
                    {
                        noteList.Add((counter.Beat2Time(p) - offset, GetDefValue(n.Value)));
                        if (isNormal)
                        {
                            longHeadLanes.Add(lane);
                        }
                        else
                        {
                            longHeadLanes.Remove(lane);
                        }
                    }
                }
            }
            if (noteList.Count is 0)
            {
                newSelection = selection;
                return false;
            }
            noteList.Sort((a, b) => a.Offset.CompareTo(b.Offset));

            // create candidates
            var firstDefValue = noteList[0].DefValue;
            var timeline = data.Timeline;
            DecimalMultiTimeline<NoteCache> decTimeline = [];
            SortedSet<(BarPosition, decimal, int)> candidatePositions = [];
            foreach (var (pos, note) in timeline)
            {
                if (note is ISoundNote n)
                {
                    var time = counter.Beat2Time(data.GetAbsolutePosition(pos));
                    decTimeline.Add(time, new(pos, n));
                    if (GetDefValue(n.Value) == firstDefValue)
                    {
                        candidatePositions.Add((pos, time, n.Lane));
                    }
                }
            }

            // replace
            var result = false;
            newSelection = [];
            var m = marginMs * 0.001m + 0.000001m; // 1us default margin
            List<(decimal Second, decimal Offset, NoteCache Note)> nearestNotes = [];
            List<(decimal Second, NoteCache Note)> sequence = [];
            foreach (var (headPos, headSecond, lane) in candidatePositions)
            {
                var success = true;
                foreach (var (innerSecond, defValue) in CollectionsMarshal.AsSpan(noteList))
                {
                    try
                    {
                        // マージン範囲全体を探索
                        var second = headSecond + innerSecond;
                        foreach (var (actualSecond, nlist) in decTimeline.EachList(RangeUtils.Get(second - m, second + m, true)))
                        {
                            if (nlist.Find(pn => GetDefValue(pn.Note.Value) == defValue) is { } actualNote)
                            {
                                nearestNotes.Add((actualSecond, Math.Abs(actualSecond - second), actualNote));
                            }
                        }
                        if (nearestNotes.Count is > 0)
                        {
                            nearestNotes.Sort((a, b) => a.Offset.CompareTo(b.Offset));
                            var (actualSecond, _, note) = nearestNotes[0];
                            sequence.Add((actualSecond, note));
                        }
                        else
                        {
                            success = false;
                            break;
                        }
                    }
                    finally
                    {
                        nearestNotes.Clear();
                    }
                }
                if (success)
                {
                    SoundNote newNote = new(lane, asmId);
                    foreach (var (time, note) in CollectionsMarshal.AsSpan(sequence))
                    {
                        timeline.Remove(note.Position, note.Note);
                        decTimeline.Remove(time, note);
                    }
                    timeline.Add(headPos, newNote);
                    newSelection.Add(data.GetHead(headPos), data.GetAbsolutePosition(headPos), newNote);
                    result = true;
                }
                sequence.Clear();
            }
            return result;
        }
    }
}
