using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class IBmsDataExtensions
    {
        public static BarPosition DefaultLength { get; } = new(4);

        public static BarPosition GetFirstPosition<T>(this T data) where T : IBmsData => data.Timeline.FirstPosition;
        public static int GetFirstBar<T>(this T data) where T : IBmsData => GetFirstPosition(data).Bar;
        public static Rational GetFirstBeat<T>(this T data) where T : IBmsData => GetHead(data, GetFirstPosition(data));

        public static BarPosition GetLastPosition<T>(this T data) where T : IBmsData => BarPosition.Max(data.Timeline.LastPosition, DefaultLength);
        public static int GetLastBar<T>(this T data) where T : IBmsData => GetLastPosition(data).Bar;
        public static Rational GetLastBeat<T>(this T data) where T : IBmsData => GetTail(data, GetLastPosition(data));

        public static Rational GetHead<T>(this T data, int number) where T : IBmsData => data.Root.BarLengthCache.GetHead(number, data.Bars);
        public static Rational GetHead<T>(this T data, BarPosition position) where T : IBmsData => GetHead(data, position.Bar);
        public static Rational GetTail<T>(this T data, BarPosition position) where T : IBmsData => GetHead(data, position.Bar + 1);
        public static int GetNumber<T>(this T data, Rational beat) where T : IBmsData => data.GetBarPosition(beat).Bar;

        public static IEnumerable<BarInfo> EachBar<T>(this T data, int first, int last = 0)
            where T : IBmsData
        {
            if (last is <= 0)
            {
                last = data.Bars.LastNumber;
            }
            foreach (var item in data.Root.BarLengthCache.EnumBars(first, last, data.Bars))
            {
                yield return item;
            }
        }

        public static IEnumerable<BarInfo> EachBar<T>(this T data, Rational first, Rational last) where T : IBmsData => EachBar(data, data.GetNumber(first), data.GetNumber(last));
    }
}
