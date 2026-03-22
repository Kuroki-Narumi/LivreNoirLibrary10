using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        public static BarPosition DefaultLength { get; } = new(4);

        private static readonly Dictionary<double, List<Note>> _moves = [];
        private static readonly Dictionary<BarPosition, double> _tempoChanges = [];

        extension (IBmsViewModel vm)
        {
            public (double First, double Last) GetBeatRange()
            {
                var (first, last) = vm.Root.GetRange();
                return (vm.GetHead(first), vm.GetTail(last));
            }
            public int GetNumber(double beat) => vm.GetBarPosition(beat).Bar;

            public IEnumerable<(int Number, double Head, double Length)> EnumerateBars() => vm.EnumerateBars(0, BmsConstants.MaxBarNumber);

            public void OnConductorChanged(int number = int.MaxValue)
            {
                vm.BarLengthCache.Clear(number);
            }

            public void InsertBar(int number, double length)
            {
                InsertBar(vm.Root, vm.CurrentData, number, 1);
                vm.CurrentData.BarDefs.Set(number, length);
                vm.OnConductorChanged(number);
                vm.OnModified();
            }

            public void DeleteBar(int number, int count)
            {
                DeleteBar(vm.Root, vm.CurrentData, number, count);
                vm.OnConductorChanged(number);
                vm.OnModified();
            }

            public void DeleteBar(int number) => DeleteBar(vm, number, 1);

            public void ResizeBar(BarResizeOptions options)
            {
                var value = options.Length;
                var ratioMode = options.RatioMode;
                var numbers = options._numbers;
                if (numbers.Count is 0 || value is <= 0 || (ratioMode && value == 1))
                {
                    return;
                }
                var root = vm.Root;
                var currentData = vm.CurrentData;
                var modified = false;
                var moves = _moves;
                var cache = vm.BarLengthCache;
                Range<BarPosition> range;
                switch (options.Mode)
                {
                    case BarResizeMode.Trim or BarResizeMode.Overlap:
                        var overlap = options.Mode is BarResizeMode.Overlap;
                        foreach (var number in numbers)
                        {
                            var current = vm.GetBarLength(number);
                            var (newLength, ratio) = ratioMode ? (current * value, value) : (value, value / current);
                            if (newLength == current)
                            {
                                continue;
                            }
                            range = new(new(number), new(number + 1));
                            foreach (var data in root.EnumerateChildren(currentData, true))
                            {
                                var timeline = data.Timeline;
                                foreach (var (pos, list) in timeline.EnumerateList(range))
                                {
                                    // 重なりが有効か、切り捨て範囲外のノート
                                    if (overlap || pos.Offset < ratio)
                                    {
                                        moves.Add(vm.GetAbsolutePosition(pos), list);
                                    }
                                }
                                // 小節長が変更される範囲は一旦削除する
                                timeline.RemoveRange(range);
                                if (data == currentData)
                                {
                                    currentData.BarDefs.Set(number, newLength);
                                    cache.Clear(number);
                                }
                                // 変更後の小節位置に再配置
                                foreach (var (abs, list) in moves)
                                {
                                    timeline.AddRange(vm.GetBarPosition(abs), list);
                                }
                                moves.Clear();
                            }
                            modified = true;
                        }
                        break;
                    case BarResizeMode.Stretch:
                        var withTempo = options.StretchWithTempo;
                        if (withTempo)
                        {
                            StreatchBarWithTempo(vm, numbers, value, ratioMode);
                            return;
                        }
                        else
                        {
                            UpdateBarLength();
                        }
                        break;
                    case BarResizeMode.Slide:
                        range = new(new(numbers.Min), new(numbers.Max + 1));
                        foreach (var data in root.EnumerateChildren(currentData, true))
                        {
                            var timeline = data.Timeline;
                            foreach (var (pos, list) in timeline.EnumerateList(range))
                            {
                                moves.Add(vm.GetAbsolutePosition(pos), list);
                            }
                            if (data == currentData)
                            {
                                UpdateBarLength();
                            }
                            if (modified)
                            {
                                foreach (var (pos, list) in moves)
                                {
                                    timeline.AddRange(vm.GetBarPosition(pos), list);
                                }
                            }
                            moves.Clear();
                        }
                        break;
                }
                if (modified)
                {
                    vm.OnConductorChanged(numbers.Min);
                    vm.OnModified();
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                void UpdateBarLength()
                {
                    var cache = vm.BarLengthCache;
                    foreach (var number in numbers)
                    {
                        var current = vm.GetBarLength(number);
                        var newLength = ratioMode ? current * value : value;
                        if (newLength != current)
                        {
                            modified = true;
                            currentData.BarDefs.Set(number, newLength);
                            cache.Clear(number);
                        }
                    }
                }
            }

            public void StreatchBarWithTempo(SortedSet<int> numbers, double value, bool ratioMode)
            {
                var data = vm.CurrentData;
                var timeline = data.Timeline;
                var changes = _tempoChanges;
                var actualBpm = (double)vm.Bpm;
                var currentBpm = actualBpm;
                var lastPos = BarPosition.Zero;
                var modified = false;
                foreach (var number in numbers)
                {
                    var currentLength = vm.GetBarLength(number);
                    var (newLength, ratio) = numbers.Contains(number) ? (ratioMode ? (currentLength * value, value) : (value, value / currentLength)) : (currentLength, 1);
                    var needChange = ratio != 1;
                    var head = new BarPosition(number);
                    var nextHead = new BarPosition(number + 1);
                    // テンポ変化の検出
                    foreach (var (pos, list) in timeline.EnumerateList(RangeUtils.Get(lastPos, needChange ? head : nextHead)))
                    {
                        if (list.FindLast(IsTempo) is { } c)
                        {
                            currentBpm = actualBpm = c.Value;
                        }
                    }
                    if (needChange)
                    {
                        var needSetHeadTempo = true;
                        foreach (var (pos, list) in timeline.EnumerateList(RangeUtils.Get(head, nextHead)))
                        {
                            foreach (var note in list.Where(IsTempo))
                            {
                                if (pos.Offset is 0)
                                {
                                    changes.Remove(pos);
                                    needSetHeadTempo = false;
                                }
                                actualBpm = note.Value;
                                currentBpm = (note.Value *= ratio);
                            }
                        }
                        if (needSetHeadTempo)
                        {
                            var bpm = actualBpm * ratio;
                            if (bpm == currentBpm)
                            {
                                changes.Remove(head);
                            }
                            else
                            {
                                changes[head] = bpm;
                            }
                            currentBpm = bpm;
                        }
                        changes[nextHead] = actualBpm;
                        data.BarDefs.Set(number, newLength);
                        modified = true;
                    }
                    lastPos = nextHead;
                }
                foreach (var (pos, bpm) in changes)
                {
                    timeline.Add(pos, new Note(Channel.Bpm, bpm));
                }
                changes.Clear();
                if (modified)
                {
                    vm.OnConductorChanged(numbers.Min);
                    vm.OnModified();
                }
            }

            public void AddBarLineAt(BarPosition position)
            {
                var (bar, offset) = position;
                var current = vm.GetBarLength(bar);
                if (offset is 0 || offset >= current)
                {
                    return;
                }
                var root = vm.Root;
                var currentData = vm.CurrentData;
                var first = current * offset;
                var second = current - first;
                var newHead = new BarPosition(bar + 1);
                position = new BarPosition(bar + 1, offset);
                foreach (var data in root.EnumerateChildren(currentData, true))
                {
                    var timeline = data.Timeline;
                    data.InsertBar(bar, 1);
                    timeline.Move(p => new(p.Bar, (p.Offset - offset) * current / second), RangeUtils.Get(position, new(bar + 2)));
                    timeline.Move(p => new(p.Bar - 1, p.Offset * current / first), RangeUtils.Get(newHead, position));
                }
                currentData.BarDefs.Set(bar, first);
                currentData.BarDefs.Set(bar + 1, second);
                vm.OnConductorChanged(bar);
                vm.OnModified();
            }

            public void MergeBar(int number, int count)
            {
                if (count is <= 1)
                {
                    return;
                }
                var root = vm.Root;
                var currentData = vm.CurrentData;
                var cache = vm.BarLengthCache;
                BarPosition first = new(number);
                BarPosition last = new(number + count);
                var length = vm.GetAbsolutePosition(last) - vm.GetAbsolutePosition(first);
                var range = RangeUtils.Get(first, last);
                var moves = _moves;
                foreach (var data in root.EnumerateChildren(currentData, true))
                {
                    var timeline = data.Timeline;
                    foreach (var (pos, list) in timeline.EnumerateList(range))
                    {
                        moves.Add(vm.GetAbsolutePosition(pos), list);
                    }
                    timeline.RemoveRange(range);
                    data.DeleteBar(number + 1, count - 1);
                    if (data == currentData)
                    {
                        currentData.BarDefs.Set(number, length);
                        cache.Clear(number);
                    }
                    foreach (var (offset, list) in moves)
                    {
                        timeline.AddRange(vm.GetBarPosition(offset), list);
                    }
                    moves.Clear();
                }
                vm.OnConductorChanged(number);
                vm.OnModified();
            }

            public void SplitBar(BarSplitOptions options)
            {
                if (!options.IsEffective())
                {
                    return;
                }
                var root = vm.Root;
                var currentData = vm.CurrentData;
                var numbers = options._numbers;
                var firstLength = options.FirstLength;
                var maxCount = options.MaxCount;
                var rpn = options._rpn;
                var vals = options._rpn_vals;
                var range = RangeUtils.StartAt(new BarPosition(numbers.Min));
                var modified = false;

                // 変更前のノートを退避
                var movesList = new Dictionary<IBmsDataUnit, Dictionary<double, List<Note>>>();
                foreach (var data in root.EnumerateChildren(currentData, true))
                {
                    var timeline = data.Timeline;
                    var moves = new Dictionary<double, List<Note>>();
                    foreach (var (pos, list) in timeline.EnumerateList(range))
                    {
                        moves.Add(vm.GetAbsolutePosition(pos), list);
                    }
                    timeline.RemoveRange(range);
                    movesList.Add(data, moves);
                }
                // 分割後の小節長のリスト
                List<double> lines = [];
                foreach (var n in numbers.Reverse())
                {
                    var originalLength = vm.GetBarLength(n);
                    if (firstLength >= originalLength)
                    {
                        continue;
                    }
                    vals.Setup(originalLength, firstLength, maxCount);
                    lines.Clear();
                    var isFirstZero = firstLength is 0;
                    lines.Add(firstLength);
                    if (rpn.IsEffective())
                    {
                        for (var i = isFirstZero ? 1 : 2; i < maxCount; i++)
                        {
                            vals.Index = i;
                            if (rpn.TryEvaluate(vals.TryGetValue, out var result, out var ex))
                            {
                                if (result <= vals.Previous || result >= originalLength)
                                {
                                    break;
                                }
                                lines.Add(result);
                            }
                            else
                            {
                                ExConsole.Write($"Exception has occurred in #{n:D3}, index:{i}");
                                ExConsole.Write(ex);
                                break;
                            }
                            vals.UpdatePrevious(result);
                        }
                    }
                    lines.Add(originalLength);
                    if (isFirstZero)
                    {
                        lines.RemoveAt(0);
                    }
                    var c = lines.Count;
                    if (c is >= 2)
                    {
                        var bars = currentData.BarDefs;
                        bars.Insert(n, c - 1);
                        bars.Set(n, lines[0]);
                        for (var i = 1; i < c; i++)
                        {
                            bars.Set(n + i, lines[i] - lines[i - 1]);
                        }
                        modified = true;
                    }
                }
                vm.OnConductorChanged(numbers.Min);
                foreach (var (data, moves) in movesList)
                {
                    var timeline = data.Timeline;
                    foreach (var (offset, list) in moves)
                    {
                        timeline.AddRange(vm.GetBarPosition(offset), list);
                    }
                }
                if (modified)
                {
                    vm.OnModified();
                }
            }
        }
    }
}
