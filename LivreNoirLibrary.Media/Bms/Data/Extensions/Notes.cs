using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class IBmsDataExtensions
    {
        public static Int128 CalcResolution<T>(this T data)
            where T : IBmsData
        {
            var result = Int128.One;
            foreach (var (pos, _) in data.Timeline.EachList())
            {
                result = result.LCM(data.GetAbsolutePosition(pos).Denominator);
            }
            return result;
        }

        public static int GetNotesCount<T>(this T data, bool countEnd = false)
            where T : IBmsData
        {
            var result = 0;
            foreach (var (_, note) in data.Timeline)
            {
                if (note.IsVisibleKey(countEnd))
                {
                    result++;
                }
            }
            return result;
        }

        public static int GetNotesCount<T>(this T data, Predicate<Note> selector)
            where T : IBmsData
        {
            var result = 0;
            foreach (var (_, note) in data.Timeline)
            {
                if (selector(note))
                {
                    result++;
                }
            }
            return result;
        }

        public static double CalcTotal<T>(this T data, double defaultValue = 0)
            where T : IBmsData
        {
            var t = data.Headers.Total;
            if (t <= 0)
            {
                t = defaultValue;
            }
            if (t <= 0)
            {
                t = BmsUtils.CalcTotal(data.GetNotesCount());
            }
            return t;
        }

        public static int GetMaxBgmLane<T>(this T data)
            where T : IBmsData
        {
            int max = 0;
            foreach (var (_, note) in data.Timeline)
            {
                if (note.IsBgm() && note.Lane < max)
                {
                    max = note.Lane;
                }
            }
            return 1 - max;
        }
    }
}
