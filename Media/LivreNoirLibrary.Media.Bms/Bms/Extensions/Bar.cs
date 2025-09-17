using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

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

        public static bool ResizeBar(this IBmsData data, BarResizeOptions options)
        {
            var value = options.Length;
            var ratioMode = options.RatioMode;
            var numbers = options._numbers;
            if (numbers.Count is 0 || value.IsNegativeOrZero() || (ratioMode && value == Rational.One))
            {
                return false;
            }
            var modified = false;
            var timeline = data.Timeline;
            switch (options.Mode)
            {
                case BarResizeMode.Trim:
                case BarResizeMode.Overlap:
                    var overlap = options.Mode is BarResizeMode.Overlap;
                    Dictionary<Rational, List<INote>> moves = [];
                    foreach (var number in numbers)
                    {
                        var current = data.GetBarLength(number);
                        var (newLength, ratio) = ratioMode ? (current * value, value) : (value, value / current);
                        var c = current.CompareTo(newLength);
                        if (c is 0)
                        {
                            continue;
                        }
                        moves.Clear();
                        if (c is -1)
                        {
                            var range = RangeUtils.Get<BarPosition>(new(number, ratio), new(number + 1, 0), false);
                            if (overlap)
                            {
                                foreach (var (pos, note) in timeline.Range(range))
                                {
                                    moves.Add(data.GetAbsolutePosition(pos), note);
                                }
                            }
                            timeline.RemoveRange(range);
                            foreach (var (abs, list) in moves)
                            {
                                timeline.Add(data.GetBarPosition(abs), list);
                            }
                        }
                        modified = true;
                    }
                    break;
            }
            return modified;
        }
    }
}
