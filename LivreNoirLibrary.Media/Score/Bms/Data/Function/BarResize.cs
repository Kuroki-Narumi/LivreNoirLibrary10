using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BaseData
    {
        private void ProcessRemove(BarPosition? first, BarPosition? last)
        {
            var range = RangeUtils.GetAuto(first, last);
            Timeline.RemoveRange(range);
        }

        private void ProcessMove(Func<BarPosition, BarPosition> converter, BarPosition? first, BarPosition? last)
        {
            var range = RangeUtils.GetAuto(first, last);
            Timeline.Move(converter, range);
        }

        public void InsertBar(int number, in Rational length)
        {
            ProcessMove(p => new(p.Bar + 1, p.Beat), new(number), null);
            Bars.Insert(number, length);
            Root.ClearBarCache(number);
        }

        public Selection DeleteBar(int number)
        {
            BarPosition first = new(number);
            BarPosition last = new(number + 1);
            var selection = SelectRange(first, last, true);
            ProcessMove(p => new(p.Bar - 1, p.Beat), last, null);
            Bars.Delete(number);
            Root.ClearBarCache(number);
            return selection;
        }

        public bool ResizeBar(BarResizeOptions options)
        {
            var ratio = options.Ratio;
            if (ratio && options.Length == Rational.One)
            {
                return false;
            }
            var mode = options.Mode;
            if (options.StretchWithTempo && mode is BarResizeMode.Stretch)
            {
                return StretchBar(options);
            }
            var flag = false;
            var len = options.Length;
            foreach (var n in options._numbers)
            {
                if (ResizeBar(n, len, ratio, mode))
                {
                    flag = true;
                }
            }
            return flag;
        }

        private bool ResizeBar(int number, Rational length, bool ratioMode, BarResizeMode mode)
        {
            var current = Bars.Get(number);
            if (length.IsNegativeOrZero())
            {
                length = Bars.GetDefault(number);
            }
            else if (ratioMode)
            {
                length = current * length;
            }
            if (current == length)
            {
                return false;
            }
            switch (mode)
            {
                case BarResizeMode.Trim:
                    ResizeBar_Trim(number, current, length);
                    break;
                case BarResizeMode.Overlap:
                    if (ResizeBar_Overlap(number, current, length))
                    {
                        return true;
                    }
                    break;
                case BarResizeMode.Stretch:
                    ResizeBar_Stretch(number, current, length);
                    return true;
                case BarResizeMode.Slide:
                    ResizeBar_Slide(number, current, length);
                    return true;
            }
            SetBarLength(number, length);
            return true;
        }

        private void SetBarLength(int number, in Rational length)
        {
            Bars.Set(number, length);
            Root.ClearBarCache(number);
        }

        private void ResizeBar_Trim(int number, in Rational oldLength, in Rational newLength)
        {
            if (newLength < oldLength)
            {
                ProcessRemove(new(number, newLength), new(number + 1));
            }
        }

        private bool ResizeBar_Overlap(int number, in Rational oldLength, in Rational newLength)
        {
            if (newLength < oldLength)
            {
                ProcessSlide(number, newLength, new(number, newLength), new(number + 1));
                return true;
            }
            return false;
        }

        private bool StretchBar(BarResizeOptions options) => Timeline.StretchWithTempo(this, options._numbers, options.Length, options.Ratio);

        private void ResizeBar_Stretch(int number, in Rational oldLength, in Rational newLength)
        {
            var ratio = newLength / oldLength;
            BarPosition first = new(number);
            BarPosition last = new(number + 1);
            ProcessMove(p => new(p.Bar, p.Beat * ratio), first, last);
            SetBarLength(number, newLength);
        }

        private void ResizeBar_Slide(int number, in Rational oldLength, in Rational newLength)
        {
            ProcessSlide(number, newLength, new(number, Rational.Min(newLength, oldLength)), null);
        }

        private void ProcessSlide(int number, Rational newLength, BarPosition? first, BarPosition? last)
        {
            var selection = SelectRange(first, last, true);
            SetBarLength(number, newLength);
            AddSelection(selection);
        }

        public bool AddBarlineAt(BarPosition pos)
        {
            var number = pos.Bar;
            var beat = pos.Beat;
            var current = GetBarLength(number);
            if (beat.IsNegativeOrZero() || beat >= current)
            {
                return false;
            }
            BarPosition nextBar = new(number + 1);
            ProcessMove(p => new(p.Bar + 1, p.Beat), nextBar, null);
            ProcessMove(p => new(number + 1, p.Beat - beat), pos, nextBar);
            Bars.Set(number, current - beat);
            Bars.Insert(number, beat);
            Root.ClearBarCache(number);
            return true;
        }

        public bool MergeBar(int number, int count)
        {
            if (count is <= 1)
            {
                return false;
            }
            BarPosition first = new(number);
            BarPosition last = new(number + count);
            var firstBeat = GetBeat(first);
            ProcessMove(p => new(number, GetBeat(p) - firstBeat), first, last);
            ProcessMove(p => new(p.Bar - count + 1, p.Beat), last, null);
            Bars.MergeLines(number, count);
            Root.ClearBarCache(number);
            return true;
        }

        public bool SplitBar(BarSplitOptions options)
        {
            if (!options.IsEffective())
            {
                return false;
            }
            var timeline = Timeline;
            var numbers = options._numbers;
            var first = options.FirstLength;
            var max = options.MaxCount;
            var rpn = options._rpn;

            var firstNum = numbers.Min;
            List<(Rational, List<Note>)> notes = [];
            var range = RangeUtils.StartAt(new BarPosition(firstNum));
            foreach (var (pos, list) in timeline.EachList(range))
            {
                notes.Add((GetBeat(pos), list));
            }
            timeline.RemoveRange(range);

            var bars = Bars;
            List<Rational> lines = [];
            BarRpnVariables vars = [];
            Rational limit;
            void ResetVars(int number)
            {
                lines.Clear();
                limit = bars.Get(number);
                vars.Setup(limit, first, max);
            }
            foreach (var n in numbers.Reverse())
            {
                ResetVars(n);
                if (first >= limit)
                {
                    continue;
                }
                lines.Add(first);
                if (rpn.IsEffective())
                {
                    for (var i = first.IsZero() ? 1 : 2; i < max; i++)
                    {
                        vars.Index = i;
                        if (rpn.TryEvaluate(vars, out var pos, out var e))
                        {
                            if (pos <= vars.Previous || pos >= limit)
                            {
                                break;
                            }
                            lines.Add(pos);
                        }
                        else
                        {
                            ExConsole.Write($"Exception has occurred in #{n:D3}, index:{i}");
                            ExConsole.Write(e);
                            break;
                        }
                        vars.UpdatePrevious(pos);
                    }
                }
                lines.Add(limit);
                lines.Remove(Rational.Zero);
                var c = lines.Count - 1;
                limit = lines[0];
                bars.Set(n, limit);
                for (var i = 1; i <= c; i++)
                {
                    var pos = lines[i];
                    bars.Insert(n + i, pos - limit);
                    limit = pos;
                }
            }
            Root.ClearBarCache(firstNum);
            foreach (var (beat, list) in CollectionsMarshal.AsSpan(notes))
            {
                timeline.Add(GetPosition(beat), list);
            }
            return true;
        }
    }
}
