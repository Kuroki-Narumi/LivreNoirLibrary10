using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension (IBmsViewModel vm)
        {
            public Selection CreateSelection(IListEnumerable<BarPosition, Note> timeline, Range<BarPosition> range, Selection? selection = null)
            {
                selection ??= [];
                var counter = vm.TimeCounter;
                foreach (var (position, list) in timeline.EnumerateList(range))
                {
                    var head = vm.GetHead(position);
                    var absPos = vm.GetAbsolutePosition(position);
                    var time = counter.Beat2Time(absPos);
                    foreach (var note in list.AsSpan())
                    {
                        selection.Add(position, head, absPos, time, note);
                    }
                }
                return selection;
            }

            public Selection CreateSelection(Range<BarPosition> range, Selection? selection = null) => CreateSelection(vm, vm.CurrentData.Timeline, range, selection);

            public void RemoveSelection(Selection selection)
            {
                var timeline = vm.CurrentData.Timeline;
                var modified = false;
                var barNumber = int.MaxValue;
                foreach (var (pos, note) in selection)
                {
                    var barPosition = vm.GetBarPosition(pos);
                    if (timeline.Remove(barPosition, note))
                    {
                        if (note.IsConductor())
                        {
                            barNumber = Math.Min(barPosition.Bar, barNumber);
                        }
                        modified = true;
                    }
                }
                if (modified)
                {
                    if (barNumber is not int.MaxValue)
                    {
                        vm.OnConductorChanged(barNumber);
                    }
                    vm.OnModified();
                }
            }

            public void AddSelection(Selection selection, double offset = 0, bool useRealTime = false)
            {
                if (selection.IsEmpty)
                {
                    return;
                }
                var timeline = vm.CurrentData.Timeline;
                var barNumber = int.MaxValue;
                var counter = vm.TimeCounter;
                if (useRealTime)
                {
                    var timeOffset = counter.Beat2Time(offset);
                    foreach (var (_, _, time, note) in selection)
                    {
                        Add(counter.Time2Beat(time + timeOffset), note);
                    }
                }
                else
                {
                    foreach (var (pos, note) in selection)
                    {
                        Add(pos + offset, note);
                    }
                }
                if (barNumber is not int.MaxValue)
                {
                    vm.OnConductorChanged(barNumber);
                }
                vm.OnModified();

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                void Add(double absPos, Note note)
                {
                    var barPosition = vm.GetBarPosition(absPos);
                    timeline.Add(barPosition, note);
                    if (note.IsConductor())
                    {
                        barNumber = Math.Min(barPosition.Bar, barNumber);
                    }
                }
            }

            public void AddSelection(Selection selection, int number, bool useRealTime = false) => AddSelection(vm, selection, vm.GetHead(number), useRealTime);

            public Selection CombineSequence(Selection source, int targetKey)
            {
                var initialized = false;
                var head = BarPosition.Zero;
                var headLane = Channel.None;
                HashSet<Channel> longHeadLanes = [];
                var timeline = vm.CurrentData.Timeline;
                foreach (var (_, pos, note) in source)
                {
                    if (note.IsPlayableSound(true))
                    {
                        var ch = note.Channel;
                        var isNormal = note.IsNormal();
                        var bp = vm.GetBarPosition(pos);
                        if ((note.IsLongEnd() && longHeadLanes.Contains(ch)) || isNormal)
                        {
                            timeline.Remove(bp, note);
                            if (isNormal)
                            {
                                longHeadLanes.Add(ch);
                            }
                            else
                            {
                                longHeadLanes.Remove(ch);
                            }
                            if (!initialized || bp < head)
                            {
                                head = bp;
                                headLane = ch;
                                initialized = true;
                            }
                        }
                    }
                }
                if (initialized)
                {
                    Note headNote = new(headLane, targetKey);
                    timeline.Add(head, headNote);
                    vm.OnModified();
                    return [new(head, vm, headNote)];
                }
                else
                {
                    return source;
                }
            }

            public Selection CombineSequenceAll(Selection source, int targetKey, double margin)
            {
                // preparation
                if (!source.TryGetFirstSound(out var firstItem, false))
                {
                    return source;
                }
                var selectionOffset = firstItem.AbsolutePosition;
                var counter = vm.TimeCounter;
                var offset = counter.Beat2Time(selectionOffset);
                List<(double Offset, string DefValue)> noteList = [];
                HashSet<Channel> longHeadLanes = [];
                string GetDefValue(double id) => vm.GetDefValue(DefType.Wav, (int)id) ?? $@"\\\***{id}***\\\";
                foreach (var (_, p, note) in source)
                {
                    if (note.IsPlayableSound(true))
                    {
                        var ch = note.Channel;
                        var isNormal = note.IsNormal();
                        if (isNormal || (note.IsLongEnd() && longHeadLanes.Contains(ch)))
                        {
                            noteList.Add((counter.Beat2Time(p) - offset, GetDefValue(note.Value)));
                            if (isNormal)
                            {
                                longHeadLanes.Add(ch);
                            }
                            else
                            {
                                longHeadLanes.Remove(ch);
                            }
                        }
                    }
                }
                if (noteList.Count is 0)
                {
                    return source;
                }
                noteList.Sort((a, b) => a.Offset.CompareTo(b.Offset));

                // create candidates
                var firstDefValue = noteList[0].DefValue;
                var timeline = vm.CurrentData.Timeline;
                DoubleMultiTimeline<NoteCache> decTimeline = [];
                SortedSet<(BarPosition, double, Channel)> candidatePositions = [];
                foreach (var (pos, note) in timeline)
                {
                    if (note.IsPlayableSound(true))
                    {
                        var time = counter.Beat2Time(vm.GetAbsolutePosition(pos));
                        decTimeline.Add(time, new NoteCache(pos, note));
                        if (GetDefValue(note.Value) == firstDefValue)
                        {
                            candidatePositions.Add((pos, time, note.Channel));
                        }
                    }
                }

                // replace
                var modified = false;
                Selection newSelection = [];
                margin = Math.Max(0, margin) + 0.000001; // 1us default margin
                List<(double Second, double Offset, NoteCache Note)> nearestNotes = [];
                List<(double Second, NoteCache Note)> sequence = [];
                foreach (var (headPos, headSecond, lane) in candidatePositions)
                {
                    var success = true;
                    foreach (var (innerSecond, defValue) in noteList.AsSpan())
                    {
                        try
                        {
                            // マージン範囲全体を探索
                            var second = headSecond + innerSecond;
                            foreach (var (actualSecond, nlist) in decTimeline.EnumerateList(RangeUtils.Get(second - margin, second + margin, true)))
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
                        Note newNote = new(lane, targetKey);
                        foreach (var (time, note) in sequence.AsSpan())
                        {
                            timeline.Remove(note.Position, note.Note);
                            decTimeline.Remove(time, note);
                        }
                        timeline.Add(headPos, newNote);
                        newSelection.Add(headPos, vm, newNote);
                        modified = true;
                    }
                    sequence.Clear();
                }
                if (modified)
                {
                    vm.OnModified();
                    return newSelection;
                }
                else
                {
                    return source;
                }
            }
        }

        private record NoteCache(BarPosition Position, Note Note);
    }
}
