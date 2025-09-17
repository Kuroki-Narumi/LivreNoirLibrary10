using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public class Timeline : XMultiTimelineBase<BarPosition, INote, Operator_BarPosition>, ITimeline, ICloneable<Timeline>, IDumpable, ILoadable<Timeline>
    {
        public Timeline Clone()
        {
            Timeline result = [];
            CopyTo(result);
            return result;
        }

        public void Dump(BinaryWriter writer) => ProcessDump(writer, INoteExtensions.Write);
        public void ProcessLoad(BinaryReader reader) => ProcessLoad(reader, INoteExtensions.ReadINote);

        public static Timeline Load(BinaryReader reader)
        {
            Timeline result = [];
            result.ProcessLoad(reader);
            return result;
        }

        public void ReplaceBy(BinaryReader reader)
        {
            Clear();
            ProcessLoad(reader);
        }

        public void InsertBar(int number)
        {
            var poss = _pos_list;
            var (s, l) = GetPositionIndex(RangeUtils.StartAt(new BarPosition(number)));
            var e = s + l;
            for (var i = s; i < e; i++)
            {
                poss[s] = new(poss[s].Bar + 1, poss[s].Offset);
            }
        }

        public void DeleteBar(int number)
        {
            var first = new BarPosition(number);
            var last = new BarPosition(number + 1);
            RemoveRange(RangeUtils.Get(first, last));
            var poss = _pos_list;
            var (s, l) = GetPositionIndex(RangeUtils.StartAt(last));
            var e = s + l;
            for (var i = s; i < e; i++)
            {
                poss[s] = new(poss[s].Bar - 1, poss[s].Offset);
            }
        }

        private readonly List<(Rational, List<INote>)> _moveBuffer = [];

        internal bool ResizeBar(BaseData data, BarResizeOptions options)
        {
            var value = options.Length;
            var ratioMode = options.RatioMode;
            var numbers = options._numbers;
            if (numbers.Count is 0 || value.IsNegativeOrZero() || (ratioMode && value == Rational.One))
            {
                return false;
            }
            var changed = options.Mode switch
            {
                BarResizeMode.Trim => ResizeBar_Each(data, value, ratioMode, numbers, false),
                BarResizeMode.Overlap => ResizeBar_Each(data, value, ratioMode, numbers, true),
                BarResizeMode.Stretch => options.StretchWithTempo ?
                        ResizeBar_StretchWithTempo(data, value, ratioMode, numbers) :
                        ResizeBar_Stretch(data, value, ratioMode, numbers),
                BarResizeMode.Slide => ResizeBar_Slide(data, value, ratioMode, numbers),
                _ => false
            };
            if (changed)
            {
                data.ClearBarLengthCache(numbers.Min);
                return true;
            }
            return false;
        }

        public void CutBar(IBarPositionProvider provider, Dictionary<int, BarResizeInfo> newValues, bool overlap)
        {
            var poss = _pos_list;
            var values = _value_list;
            var moves = _moveBuffer;
            foreach (var (n, (newLength, ratio)) in newValues)
            {
                var current = provider.GetBarLength(n);
                var c = current.CompareTo(newLength);
                if (c is 0)
                {
                    continue;
                }
                moves.Clear();
                if (c is -1)
                {
                    var (s, l) = _pos_list.IndexRange(RangeUtils.Get<BarPosition>(new(n, ratio), new(n + 1, 0), false));
                    for (; l is > 0; l--)
                    {
                        if (overlap)
                        {
                            moves.Add((provider.GetAbsolutePosition(poss[s]), values[s]));
                        }
                        RemoveItem(s);
                    }
                }
                foreach (var (abs, list) in moves)
                {
                    Add(provider.GetBarPosition(abs), list);
                }
            }
        }

        private bool ResizeBar_Each(BaseData data, Rational value, bool ratioMode, SortedSet<int> numbers, bool overlap)
        {
            var changed = false;
            var poss = _pos_list;
            var values = _value_list;
            var moves = _moveBuffer;
            foreach (var n in numbers)
            {
                var current = data.GetBarLength(n);
                var (newLength, ratio) = ratioMode ? (current * value, value) : (value, value / current);
                var c = current.CompareTo(newLength);
                if (c is 0)
                {
                    continue;
                }
                if (c is -1)
                {
                    var (s, l) = _pos_list.IndexRange(RangeUtils.Get<BarPosition>(new(n, ratio), new(n + 1, 0), false));
                    for (; l is > 0; l--)
                    {
                        if (overlap)
                        {
                            moves.Add((data.GetAbsolutePosition(poss[s]), values[s]));
                        }
                        RemoveItem(s);
                    }
                }
                data.SetBarLength(n, newLength);
                foreach (var (abs, list) in moves)
                {
                    Add(data.GetBarPosition(abs), list);
                }
                moves.Clear();
                changed = true;
            }
            return changed;
        }

        private bool ResizeBar_Slide(BaseData data, Rational value, bool ratioMode, SortedSet<int> numbers)
        {
            var moves = _moveBuffer;
            var buffer = ArrayPool<int>.Shared.Rent(numbers.Count);
            try
            {
                var notChanged = true;
                numbers.CopyTo(buffer, 0);
                var span = buffer.AsSpan(0, numbers.Count);
                var numberIndex = 0;
                var startNumber = 0;
                while (numberIndex < span.Length)
                {
                    startNumber = span[numberIndex];
                    var current = data.GetBarLength(startNumber);
                    var newLength = ratioMode ? current * value : value;
                    if (current == newLength)
                    {
                        numberIndex++;
                    }
                    else
                    {
                        notChanged = false;
                        break;
                    }
                }
                if (notChanged)
                {
                    return false;
                }
                var poss = _pos_list;
                var values = _value_list;
                var posIndex = poss.FindIndex(new BarPosition(startNumber), SearchMode.NextOrEqual);
                for (var i = posIndex; i < poss.Count; i++)
                {
                    moves.Add((data.GetAbsolutePosition(poss[i]), values[i]));
                }
                RemoveRangeCore((posIndex,  poss.Count - posIndex));
                foreach (var n in span[numberIndex..])
                {
                    data.SetBarLength(n, ratioMode ? data.GetBarLength(n) * value : value);
                }
                foreach (var (abs, list) in moves)
                {
                    Add(data.GetBarPosition(abs), list);
                }
                return true;
            }
            finally
            {
                ArrayPool<int>.Shared.Return(buffer);
                moves.Clear();
            }
        }

        private static bool ResizeBar_Stretch(BaseData data, Rational value, bool ratioMode, SortedSet<int> numbers)
        {
            var changed = false;
            var bars = data.Bars;
            foreach (var n in numbers)
            {
                var current = data.GetBarLength(n);
                if (ratioMode)
                {
                    bars.Set(n, current * value);
                    changed = true;
                }
                else if (current != value)
                {
                    bars.Set(n, value);
                    changed = true;
                }
            }
            return changed;
        }

        private bool ResizeBar_StretchWithTempo(BaseData data, Rational value, bool ratioMode, SortedSet<int> numbers)
        {
            var changed = false;
            var bars = data.Bars;
            var poss = _pos_list;
            var vals = _value_list;

            var index = 0;
            bool IsMatch(Predicate<BarPosition> condition) => index < poss.Count && condition(poss[index]);

            var actualBpm = (Rational)data.Bpm;
            var currentBpm = actualBpm;

            void AppendHeadBpm(int number)
            {
                BarPosition head = new(number);
                // 現在のノート位置が小節頭の場合
                if (IsMatch(p => p == head))
                {
                    var list = vals[index];
                    // 小節頭にテンポノートが無い場合は、本来のテンポに戻すためのノートを追加する
                    if (list.FindLast(n => n.IsTempo(out _)) is null)
                    {
                        list.Insert(0, INote.Tempo(actualBpm));
                    }
                }
                else
                {
                    // 小節頭に正しいテンポのノートを配置
                    InsertItem(index, head, [INote.Tempo(actualBpm)]);
                }
            }

            foreach (var n in numbers)
            {
                // 変更されない小節はテンポ変化のチェックだけ行う
                for (; IsMatch(p => p.Bar < n); index++)
                {
                    var list = vals[index];
                    if (list.FindLast(n => n.IsTempo(out _)) is ConductorNote note)
                    {
                        currentBpm = actualBpm = note.Value;
                    }
                }

                var current = data.GetBarLength(n);
                var (newLength, ratio) = ratioMode ? (current * value, value) : (value, value / current);
                if (current == newLength)
                {
                    continue;
                }
                changed = true;
                bars.Set(n, newLength);
                // 小節頭に現在のテンポをノートとして設置
                AppendHeadBpm(n);
                // 小節内位置の変更
                while (IsMatch(p => p.Bar == n))
                {
                    // 位置の更新
                    //poss[index] = new(n, poss[index].Offset * ratio);
                    var list = vals[index];
                    // テンポ変化の反映
                    for (var i = 0; i < list.Count;)
                    {
                        if (list[i] is ConductorNote note)
                        {
                            switch (note.Channel)
                            {
                                case Channel.Bpm:
                                    actualBpm = note.Value;
                                    var newBpm = actualBpm * ratio;
                                    // テンポ変化がなくなる場合は、そのテンポノートを削除する
                                    if (newBpm == currentBpm)
                                    {
                                        list.RemoveAt(i);
                                        continue;
                                    }
                                    note.Value = currentBpm = newBpm;
                                    break;
                                case Channel.Stop:
                                    note.Value *= ratio;
                                    break;
                            }
                        }
                        i++;
                    }
                    if (list.Count is 0)
                    {
                        RemoveItem(index);
                    }
                    else
                    {
                        index++;
                    }
                }
                // 次の小節頭にBPMを元に戻すためのノートを設置
                AppendHeadBpm(n + 1);
            }
            return changed;
        }

        internal bool AddBarLineAt(IBmsData data, BarPosition position)
        {
            if (position.Offset.IsNegativeOrZero())
            {
                return false;
            }
            var number = position.Bar;
            var nextNumber = number + 1;
            var current = data.GetBarLength(number);
            var first = position.Offset * current;
            var second = current - first;
            InsertBar(data, nextNumber, second);
            var poss = _pos_list;
            var (s, l) = GetPositionIndex(RangeUtils.Get(position, new(nextNumber)));
            var e = s + l;
            for (var i = s; i < e; i++)
            {
                poss[i] = new(nextNumber, (poss[i].Offset * current - first) / second);
            }
            data.SetBarLength(number, first);
            return true;
        }

        internal bool MergeBar(IBmsData data, int number, int count)
        {
            if (count is <= 1)
            {
                return false;
            }
            BarPosition first = new(number);
            BarPosition last = new(number + count);
            var head = data.GetHead(number);
            var poss = _pos_list;
            var (s, l) = GetPositionIndex(RangeUtils.Get(first, last));
            var e = s + l;
            for (var i = s; i < e; i++)
            {

            }
            data.Bars.Merge(number, count);


            var firstBeat = GetBeat(first);
            ProcessMove(p => new(number, GetBeat(p) - firstBeat), first, last);
            ProcessMove(p => new(p.Bar - count + 1, p.Offset), last, null);
            Bars.MergeLines(number, count);
            Root.ClearBarCache(number);
            return BarEditResult.Applied;
        }
    }
}
