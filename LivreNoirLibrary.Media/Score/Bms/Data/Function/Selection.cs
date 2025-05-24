using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BaseData
    {
        public Selection CreateSelection(IEnumerable<(BarPosition, Note)> notes)
        {
            Selection selection = [];
            foreach (var (p, v) in notes)
            {
                selection.Add(p, GetBeat(p), v);
            }
            return selection;
        }

        public Selection SelectRange(BarPosition? first, BarPosition? last, bool removeSelected = false)
        {
            var range = RangeUtils.GetAuto(first, last);
            var selection = CreateSelection(Timeline.Range(range));
            if (removeSelected)
            {
                RemoveSelection(selection);
            }
            return selection;
        }

        public Selection Select(SelectionOptions options, Selection? selection = null)
        {
            Selection newSelection = [];
            options.Prepare(LnObj is 0);
            if (options.Target.Type is ConvertTargetType.Selected)
            {
                if (selection is not null)
                {
                    foreach (var item in selection.EachItem())
                    {
                        if (options.IsMatch(item.Position, item.Note))
                        {
                            newSelection.Add(item);
                        }
                    }
                }
            }
            else
            {
                foreach (var (pos, beat, note) in EachNote())
                {
                    if (options.IsMatch(pos, note))
                    {
                        newSelection.Add(pos, beat, note);
                    }
                }
            }
            return newSelection;
        }

        public void RemoveSelection(Selection selection)
        {
            foreach (var (p, _, n) in selection.EachItem())
            {
                Timeline.Remove(p, n);
            }
        }

        public void AddSelection(Selection selection)
        {
            foreach (var (_, p, n) in selection.EachItem())
            {
                Timeline.Add(GetPosition(p), n);
            }
        }

        public void AddSelection(Selection selection, Rational offset)
        {
            foreach (var (_, p, n) in selection.EachItem())
            {
                Timeline.Add(GetPosition(p + offset), n);
            }
        }

        public void AddSelection(Selection selection, int barStart) => AddSelection(selection, GetHead(barStart));

        public bool EditSelection(SelectionOptions options, Selection selection)
        {
            var lnEnd = LnObj is 0;
            Func<Note, bool> process;
            var mode = options.ReplaceMode;
            var val = options.ReplaceValue;
            if (mode is ValueOperationMode.Set)
            {
                var id = (int)val;
                if (id is 0)
                {
                    return false;
                }
                process = n =>
                {
                    if (n.IsIndex(lnEnd))
                    {
                        n.Id = id;
                        return true;
                    }
                    return false;
                };
            }
            else if (ValueOperation.TryGetOperator(mode, val, out var op))
            {
                var max = MaxDefIndex;
                process = n =>
                {
                    if (n.IsIndex(lnEnd))
                    {
                        var id = Math.Clamp((int)op(n.Value), 1, max);
                        if (n.Id != id)
                        {
                            n.Id = id;
                            return true;
                        }
                    }
                    else
                    {
                        var value = op(n.Value);
                        if (n.Value != value)
                        {
                            n.Value = value;
                            return true;
                        }
                    }
                    return false;
                };
            }
            else
            {
                return false;
            }
            var edited = false;
            foreach (var (_, _, note) in selection.EachItem())
            {
                if (process(note))
                {
                    edited = true;
                }
            }
            return edited;
        }

        public bool MoveSelection(SelectionOptions options, Selection selection)
        {
            if (ValueOperation.TryGetOperator(options.MoveMode, options.MoveValue, out var func))
            {
                return ProcessMoveSelection(selection, func);
            }
            return false;
        }

        public bool QuantizeSelection(SelectionOptions options, Selection selection)
        {
            var q = options.QuantizeValue;
            if (q.IsPositiveThanZero())
            {
                return ProcessMoveSelection(selection, p => Midi.INote.GetQuantized(p, q));
            }
            return false;
        }

        private bool ProcessMoveSelection(Selection selection, Func<Rational, Rational> func)
        {
            var timeline = Timeline;
            Selection newSelection = [];
            var offset = selection.GetFirstBarHead();
            foreach (var (bp, p, note) in selection.EachItem())
            {
                var newP = func(p - offset) + offset;
                if (newP.IsNegative())
                {
                    newP = Rational.Zero;
                }
                if (p != newP)
                {
                    newSelection.Add(GetPosition(newP), newP, note);
                }
                else
                {
                    newSelection.Add(bp, p, note);
                }
                timeline.Remove(bp, note);
            }

            if (newSelection.Count is > 0)
            {
                foreach (var (bp, _, note) in newSelection.EachItem())
                {
                    timeline.Add(bp, note);
                }
                return true;
            }
            return false;
        }

        public bool ReplaceSelection(Selection selection, int asmId, out Selection newSelection)
        {
            var initialized = false;
            var head = BarPosition.Zero;
            var headLane = -1;
            HashSet<int> longHeadLanes = [];
            foreach (var (bp, p, n) in selection.EachItem())
            {
                var lane = n.Lane;
                var isNormal = n.IsNormal();
                if (n.IsWavObject(true) && (longHeadLanes.Contains(lane) || isNormal))
                {
                    Timeline.Remove(bp, n);
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

            if (initialized)
            {
                Note headNote = new(NoteType.Normal, headLane, asmId);
                Timeline.Add(head, headNote);
                newSelection = [new(head, GetBeat(head), headNote)];
                return true;
            }
            else
            {
                newSelection = selection;
                return false;
            }
        }

        public bool ReplaceSelectionAll(Selection selection, int asmId, int marginMs, out Selection newSelection)
        {
            // preparatoin
            TimeCounter counter = new(this);
            BarPosition Second2Pos(Rational second) => GetPosition(counter.Second2Beat(second));

            var selectionOffset = selection.GetFirstBeat();
            var offset = counter.Beat2Second(selectionOffset);
            var m = new Rational(marginMs, 1000);
            List<(Rational Offset, Note Note)> noteList = [];
            HashSet<int> longHeadLanes = [];
            foreach (var (_, p, n) in selection.EachItem())
            {
                var lane = n.Lane;
                var isNormal = n.IsNormal();
                if (n.IsWavObject(true) && (longHeadLanes.Contains(lane) || isNormal))
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
                    c = a.Note.Lane.CompareTo(b.Note.Lane);
                    if (c is 0)
                    {
                        c = a.Note.Id.CompareTo(b.Note.Id);
                    }
                }
                i++;
                return c;
            });

            // create candidates
            var firstNoteId = noteList[0].Note.Id;
            SortedSet<(BarPosition, Rational, int)> posList = [];
            var timeline = Timeline;
            foreach (var (pos, note) in timeline)
            {
                if (note.IsWavObject(false) && note.Id == firstNoteId)
                {
                    posList.Add((pos, counter.Beat2Second(GetBeat(pos)), note.Lane));
                }
            }

            // replace
            var result = false;
            newSelection = [];
            List<(BarPosition, Note)> list = [];
            foreach (var (headPos, headSecond, lane) in posList)
            {
                var success = true;
                foreach (var (innerS, note) in CollectionsMarshal.AsSpan(noteList))
                {
                    var second = headSecond + innerS;
                    var pos = Second2Pos(second);
                    var type = note.Type;
                    var id = note.Id;
                    bool Pred(Note n) => n.IsSound() && n.Type == type && n.Id == id;
                    if (timeline.Find(pos, Pred, out var aNote))
                    {
                        list.Add((pos, aNote));
                    }
                    else
                    {
                        if (marginMs is not 0)
                        {
                            var flag = false;
                            var range = RangeUtils.Get(Second2Pos(second - m), Second2Pos(second + m));
                            foreach (var (bp, n) in timeline.Range(range))
                            {
                                if (Pred(n))
                                {
                                    list.Add((bp, n));
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                continue;
                            }
                        }
                        if (note.IsNormal())
                        {
                            success = false;
                            break;
                        }
                    }
                }
                if (success)
                {
                    Note newNote = new(NoteType.Normal, lane, asmId);
                    foreach (var (pos, note) in CollectionsMarshal.AsSpan(list))
                    {
                        timeline.Remove(pos, note);
                    }
                    timeline.Add(headPos, newNote);
                    newSelection.Add(headPos, GetBeat(headPos), newNote);
                    result = true;
                }
                list.Clear();
            }
            return result;
        }
    }
}
