using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class IBmsDataExtensions
    {
        public static BarPosition DefaultLength { get; } = new(4);

        public static BarPosition GetFirstPosition(this IBmsData data) => data.Timeline.FirstPosition;
        public static int GetFirstBar(this IBmsData data) => GetFirstPosition(data).Bar;
        public static Rational GetFirstBeat(this IBmsData data) => GetHead(data, GetFirstPosition(data));

        public static BarPosition GetLastPosition(this IBmsData data) => BarPosition.Max(data.Timeline.LastPosition, DefaultLength);
        public static int GetLastBar(this IBmsData data) => GetLastPosition(data).Bar;
        public static Rational GetLastBeat(this IBmsData data) => GetTail(data, GetLastPosition(data));

        public static Rational GetHead(this IBmsData data, BarPosition position) => data.GetHead(position.Bar);
        public static Rational GetTail(this IBmsData data, BarPosition position) => data.GetHead(position.Bar + 1);
        public static int GetNumber(this IBmsData data, Rational beat) => data.GetBarPosition(beat).Bar;

        public static IEnumerable<BarInfo> EnumerateBars(this IBmsData data, Rational first, Rational last) => data.EnumerateBars(data.GetNumber(first), data.GetNumber(last));

        private static readonly Dictionary<Rational, List<INote>> _moves = [];
        public static bool ResizeBar(this IBmsData data, BarResizeOptions options)
        {
            var value = options.Length;
            var ratioMode = options.RatioMode;
            var numbers = options._numbers;
            if (numbers.Count is 0 || value.IsNegativeOrZero() || (ratioMode && value == 1))
            {
                return false;
            }
            var modified = false;
            var timeline = data.Timeline;
            var moves = _moves;
            switch (options.Mode)
            {
                case BarResizeMode.Trim or BarResizeMode.Overlap:
                    var overlap = options.Mode is BarResizeMode.Overlap;
                    foreach (var number in numbers)
                    {
                        var current = data.GetBarLength(number);
                        var (newLength, ratio) = ratioMode ? (current * value, value) : (value, value / current);
                        if (newLength != current)
                        {
                            continue;
                        }
                        var range = RangeUtils.Get<BarPosition>(new(number), new(number + 1));
                        foreach (var (pos, list) in timeline.EachList(range))
                        {
                            // 重なりが有効か、切り捨て範囲外のノート
                            if (overlap || pos.Offset < ratio)
                            {
                                moves.Add(data.GetAbsolutePosition(pos), list);
                            }
                        }
                        // 小節長が変更される範囲は一旦削除する
                        timeline.RemoveRange(range);
                        data.SetBarLength(number, newLength);
                        // 変更後の小節位置に再配置
                        foreach (var (abs, list) in moves)
                        {
                            timeline.Add(data.GetBarPosition(abs), list);
                        }
                        moves.Clear();
                        modified = true;
                    }
                    break;
                case BarResizeMode.Stretch:
                    var withTempo = options.StretchWithTempo;
                    if (withTempo)
                    {
                        modified = StreatchBarWithTempo(data, numbers, value, ratioMode);
                    }
                    else
                    {
                        UpdateBarLength();
                    }
                    break;
                case BarResizeMode.Slide:
                    foreach (var (pos, list) in timeline.EachList(RangeUtils.Get<BarPosition>(new(numbers.Min), new(numbers.Max + 1))))
                    {
                        moves.Add(data.GetAbsolutePosition(pos), list);
                    }
                    UpdateBarLength();
                    if (modified)
                    {
                        foreach (var (pos, list) in moves)
                        {
                            timeline.Add(data.GetBarPosition(pos), list);
                        }
                    }
                    moves.Clear();
                    break;
            }
            return modified;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void UpdateBarLength()
            {
                foreach (var number in numbers)
                {
                    var current = data.GetBarLength(number);
                    var newLength = ratioMode ? current * value : value;
                    if (newLength != current)
                    {
                        modified = true;
                        data.SetBarLength(number, newLength);
                    }
                }
            }
        }

        private static readonly Dictionary<BarPosition, decimal> _tempoChanges = [];
        public static bool StreatchBarWithTempo(this IBmsData data, SortedSet<int> numbers, Rational value, bool ratioMode)
        {
            var timeline = data.Timeline;
            var changes = _tempoChanges;
            var actualBpm = (decimal)data.Bpm;
            var currentBpm = actualBpm;
            var lastPos = BarPosition.Zero;
            var modified = false;
            foreach (var number in numbers)
            {
                var currentLength = data.GetBarLength(number);
                var (newLength, ratio) = numbers.Contains(number) ? (ratioMode ? (currentLength * value, value) : (value, value / currentLength)) : (currentLength, Rational.One);
                var needChange = ratio != 1;
                var head = new BarPosition(number);
                var nextHead = new BarPosition(number + 1);
                // テンポ変化の検出
                foreach (var (pos, list) in timeline.EachList(RangeUtils.Get(lastPos, needChange ? head : nextHead)))
                {
                    if (list.FindLast(n => n.IsTempo(out _)) is IConductorNote c)
                    {
                        currentBpm = actualBpm = c.Value;
                    }
                }
                if (needChange)
                {
                    var needSetHeadTempo = true;
                    foreach (var (pos, list) in timeline.EachList(RangeUtils.Get(head, nextHead)))
                    {
                        foreach (var note in list.Where(n => n.IsTempo(out _)).Select(n => (n as IConductorNote)!))
                        {
                            if (pos.Offset.IsZero())
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
                    data.SetBarLength(number, newLength);
                    modified = true;
                }
                lastPos = nextHead;
            }
            foreach (var (pos, bpm) in changes)
            {
                timeline.Add(pos, new ConductorNote(Channel.Bpm, bpm));
            }
            changes.Clear();
            return modified;
        }

        public static bool AddBarLineAt(this IBmsData data, BarPosition position)
        {
            var (bar, offset) = position;
            if (offset.IsZero())
            {
                return false;
            }
            var timeline = data.Timeline;
            var current = data.GetBarLength(bar);
            var first = current * offset;
            var second = current - first;
            data.SetBarLength(bar, second);
            data.InsertBar(bar, first);
            var newHead = new BarPosition(bar + 1);
            position = new BarPosition(bar + 1, offset);
            timeline.Move(p => new(p.Bar, (p.Offset - offset) * current / second), RangeUtils.Get(position, new(bar + 2)));
            timeline.Move(p => new(p.Bar - 1, p.Offset * current / first), RangeUtils.Get(newHead, position));
            return true;
        }

        public static bool MergeBar(this IBmsData data, int number, int count)
        {
            if (count is <= 1)
            {
                return false;
            }
            var timeline = data.Timeline;
            BarPosition first = new(number);
            BarPosition last = new(number + count);
            var range = RangeUtils.Get(first, last);
            var move = _moves;
            foreach (var (pos, list) in timeline.EachList(range))
            {
                move.Add(data.GetAbsolutePosition(pos), list);
            }
            timeline.RemoveRange(range);
            var length = data.GetAbsolutePosition(last) - data.GetAbsolutePosition(first);
            data.DeleteBar(number + 1, count - 1);
            data.SetBarLength(number, length);
            foreach (var (offset, list) in move)
            {
                timeline.Add(data.GetBarPosition(offset), list);
            }
            move.Clear();
            return true;
        }

        public static bool SplitBar(this IBmsData data, BarSplitOptions options)
        {
            if (!options.IsEffective())
            {
                return false;
            }
            var timeline = data.Timeline;
            var numbers = options._numbers;
            var first = options.FirstLength;
            var max = options.MaxCount;
            var rpn = options._rpn;
            return false;
        }
    }
}
