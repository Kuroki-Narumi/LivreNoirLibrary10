using System;
using System.Collections.Generic;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public interface INote : IObject
    {
        public Rational Length { get; }
        public void QuantizeVelocity(int q);
        public void QuantizeLength(Rational q);
        public SortKey GetSortKey(SortKeyType key1, SortKeyType key2, SortKeyType key3, int index);
        public Rational[] GetMarkersArray(Rational offset = default);
        public string GetMarkerName(string format);
        public IEnumerable<(Rational, Note)> EachNote(Rational position);
        internal bool MatchesNumber(SortedSet<int> set);
        internal INote GetEdited(Rational lenQ, Func<Rational, Rational>? lenFunc, int velQ, Func<double, double>? velFunc, Func<double, double>? nnFunc);

        public static int GetQuantized(int val, int q) => q is <= 0 ? val : (val + (q - 1)) / q * q;

        public static Rational GetQuantized(Rational val, Rational q)
        {
            if (q.IsNegativeOrZero())
            {
                return val;
            }
            var v = Math.Round((double)(val / q), MidpointRounding.ToEven);
            return new((long)v * q.Numerator, q.Denominator);
        }

        public static double GetQuantized(double val, double q)
        {
            if (q is <= 0)
            {
                return val;
            }
            return Math.Round(val / q, MidpointRounding.ToEven) * q;
        }

        public static Rational GetEdit(Rational value, Rational q, Func<Rational, Rational>? func)
        {
            var result = GetQuantized(func is not null ? func(value) : value, q);
            if (result.IsNegative())
            {
                result = Rational.Zero;
            }
            return result;
        }

        public static int GetIntEdit(int value, int q, Func<double, double>? func) => GetQuantized(func is not null ? func(value).RoundToInt() : value, q);
        public static byte GetByteEdit(int value, int q, Func<double, double>? func, int min = 0) => Event.GetMax127(GetIntEdit(value, q, func), min);

        public static string GetLengthText(Rational length) => $"{length.Numerator}-{length.Denominator}";
    }
}
