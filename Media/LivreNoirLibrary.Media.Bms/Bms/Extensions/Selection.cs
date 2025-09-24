using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

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
            var offset = counter.Beat2Second(selectionOffset);
            List<(decimal Offset, ISoundNote Note)> noteList = [];
            HashSet<int> longHeadLanes = [];
            foreach (var (_, p, note) in selection)
            {
                if (note is ISoundNote n)
                {
                    var lane = n.Lane;
                    var isNormal = n.IsNormal();
                    if ((n.IsLongEnd() && longHeadLanes.Contains(lane)) || isNormal)
                    {
                        noteList.Add((counter.Beat2Second(p) - offset, n));
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
            var i = 0;
            noteList.Sort((a, b) =>
            {
                var c = a.Offset.CompareTo(b.Offset);
                if (c is 0)
                {
                    var an = a.Note;
                    var bn = b.Note;
                    c = an.Lane.CompareTo(bn.Lane);
                    if (c is 0)
                    {
                        c = an.Value.CompareTo(bn.Value);
                    }
                }
                i++;
                return c;
            });

            // create candidates
            var firstNoteId = noteList[0].Note.Value;
            var timeline = data.Timeline;
            SortedSet<(BarPosition, decimal, int)> candidatePositions = [];
            foreach (var (pos, note) in timeline)
            {
                if (note is ISoundNote s && s.Value == firstNoteId)
                {
                    candidatePositions.Add((pos, counter.Beat2Second(data.GetAbsolutePosition(pos)), s.Lane));
                }
            }

            // replace
            var result = false;
            newSelection = [];
            List<(BarPosition, ISoundNote)> list = [];
            var m = marginMs * 0.001m + 0.000001m; // 1us margin
            foreach (var (headPos, headSecond, lane) in candidatePositions)
            {
                var success = true;
                foreach (var (innerS, note) in CollectionsMarshal.AsSpan(noteList))
                {
                    var second = headSecond + innerS;
                    var pos = data.GetBarPosition(new((long)(counter.Second2Beat(second) * 1000000), 1000000)); // 1us quantize
                    var value = note.Value;
                    bool Pred(INote n) => n is ISoundNote s && s.Value == value;
                    if (timeline.TryGetNearest(pos, out var actualPos, out var actualList) && // 推定される位置に最も近いリスト
                        actualList.Find(Pred) is ISoundNote actualNote &&                     // の中にIDの合致するノートが存在し
                        Math.Abs(counter.Beat2Second(data.GetAbsolutePosition(actualPos)) - second) <= m) // 実際の位置が許容誤差以下の場合
                    {
                        list.Add((actualPos, actualNote));
                    }
                    else
                    {
                        success = false;
                        break;
                    }
                }
                if (success)
                {
                    SoundNote newNote = new(lane, asmId);
                    foreach (var (pos, note) in CollectionsMarshal.AsSpan(list))
                    {
                        timeline.Remove(pos, note);
                    }
                    timeline.Add(headPos, newNote);
                    newSelection.Add(data.GetHead(headPos), data.GetAbsolutePosition(headPos), newNote);
                    result = true;
                }
                list.Clear();
            }
            return result;
        }
    }
}
