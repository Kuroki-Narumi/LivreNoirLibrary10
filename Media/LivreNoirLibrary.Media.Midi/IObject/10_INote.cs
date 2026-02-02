using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public interface INote : IObject
    {
        Rational Length { get; }
        void QuantizeVelocity(int q);
        void QuantizeLength(Rational q);
        SortKey GetSortKey(SortKeyType key1, SortKeyType key2, SortKeyType key3, int index);
        Rational[] GetMarkersArray(Rational offset = default);
        string GetMarkerName(string format);
        IEnumerable<(Rational, Note)> EachNote(Rational position);
        internal bool MatchesNumber(RangeSet<int> set);
        internal INote GetEdited(Rational lenQ, Func<Rational, Rational>? lenFunc, int velQ, Func<double, double>? velFunc, Func<double, double>? nnFunc);

        static int GetQuantized(int val, int q) => q is <= 0 ? val : (val + (q - 1)) / q * q;

        static Rational GetQuantized(Rational val, Rational q)
        {
            if (q.IsNegativeOrZero())
            {
                return val;
            }
            var v = Math.Round((double)(val / q), MidpointRounding.ToEven);
            return new((long)v * q.Numerator, q.Denominator);
        }

        static double GetQuantized(double val, double q)
        {
            if (q is <= 0)
            {
                return val;
            }
            return Math.Round(val / q, MidpointRounding.ToEven) * q;
        }

        static Rational GetEdit(Rational value, Rational q, Func<Rational, Rational>? func)
        {
            var result = GetQuantized(func is not null ? func(value) : value, q);
            if (result.IsNegative())
            {
                result = Rational.Zero;
            }
            return result;
        }

        static int GetIntEdit(int value, int q, Func<double, double>? func) => GetQuantized(func is not null ? func(value).RoundToInt() : value, q);
        static byte GetByteEdit(int value, int q, Func<double, double>? func, int min = 0) => Events.Event.GetMax127(GetIntEdit(value, q, func), min);

        static string GetLengthText(Rational length) => $"{length.Numerator}-{length.Denominator}";
    }
}
