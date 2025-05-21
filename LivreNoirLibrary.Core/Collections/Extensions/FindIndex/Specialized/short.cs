using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<short> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_int(), type);
        }

        public static int FindIndex(this Span<short> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_int(), type);
        }

        public static int FindIndex(this short[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_short_int(), type);
        }

        public static int FindIndex(this List<short> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_int(), type);
        }

        public static int FindIndex(this IList<short> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<short> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_short_int());
        }

        public static int FindNearestIndex(this Span<short> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_short_int());
        }

        public static int FindNearestIndex(this short[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_short_int());
        }

        public static int FindNearestIndex(this List<short> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_short_int());
        }

        public static int FindNearestIndex(this IList<short> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_short_int());
        }

        public static ReadOnlySpan<short> Range(this ReadOnlySpan<short> span, Range<int> range)
        {
            return Range(span, range, new Comparer_short_int());
        }

        public static ReadOnlySpan<short> Range(this Span<short> span, Range<int> range)
        {
            return Range(span, range, new Comparer_short_int());
        }

        public static ReadOnlySpan<short> Range(this short[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_short_int());
        }

        public static ReadOnlySpan<short> Range(this List<short> list, Range<int> range)
        {
            return Range(list, range, new Comparer_short_int());
        }

        public static IEnumerable<short> Range(this IList<short> list, Range<int> range)
        {
            return Range(list, range, new Comparer_short_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<short> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_short_int());
        }

        public static (int Start, int Length) IndexRange(this Span<short> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_short_int());
        }

        public static (int Start, int Length) IndexRange(this short[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_short_int());
        }

        public static (int Start, int Length) IndexRange(this List<short> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_short_int());
        }

        public static (int Start, int Length) IndexRange(this IList<short> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_short_int());
        }

        public static int FindIndex(this ReadOnlySpan<short> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_long(), type);
        }

        public static int FindIndex(this Span<short> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_long(), type);
        }

        public static int FindIndex(this short[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_short_long(), type);
        }

        public static int FindIndex(this List<short> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_long(), type);
        }

        public static int FindIndex(this IList<short> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<short> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_short_long());
        }

        public static int FindNearestIndex(this Span<short> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_short_long());
        }

        public static int FindNearestIndex(this short[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_short_long());
        }

        public static int FindNearestIndex(this List<short> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_short_long());
        }

        public static int FindNearestIndex(this IList<short> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_short_long());
        }

        public static ReadOnlySpan<short> Range(this ReadOnlySpan<short> span, Range<long> range)
        {
            return Range(span, range, new Comparer_short_long());
        }

        public static ReadOnlySpan<short> Range(this Span<short> span, Range<long> range)
        {
            return Range(span, range, new Comparer_short_long());
        }

        public static ReadOnlySpan<short> Range(this short[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_short_long());
        }

        public static ReadOnlySpan<short> Range(this List<short> list, Range<long> range)
        {
            return Range(list, range, new Comparer_short_long());
        }

        public static IEnumerable<short> Range(this IList<short> list, Range<long> range)
        {
            return Range(list, range, new Comparer_short_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<short> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_short_long());
        }

        public static (int Start, int Length) IndexRange(this Span<short> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_short_long());
        }

        public static (int Start, int Length) IndexRange(this short[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_short_long());
        }

        public static (int Start, int Length) IndexRange(this List<short> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_short_long());
        }

        public static (int Start, int Length) IndexRange(this IList<short> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_short_long());
        }

        public static int FindIndex(this ReadOnlySpan<short> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_ulong(), type);
        }

        public static int FindIndex(this Span<short> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_ulong(), type);
        }

        public static int FindIndex(this short[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_short_ulong(), type);
        }

        public static int FindIndex(this List<short> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_ulong(), type);
        }

        public static int FindIndex(this IList<short> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<short> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_short_ulong());
        }

        public static int FindNearestIndex(this Span<short> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_short_ulong());
        }

        public static int FindNearestIndex(this short[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_short_ulong());
        }

        public static int FindNearestIndex(this List<short> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_short_ulong());
        }

        public static int FindNearestIndex(this IList<short> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_short_ulong());
        }

        public static ReadOnlySpan<short> Range(this ReadOnlySpan<short> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_short_ulong());
        }

        public static ReadOnlySpan<short> Range(this Span<short> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_short_ulong());
        }

        public static ReadOnlySpan<short> Range(this short[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_short_ulong());
        }

        public static ReadOnlySpan<short> Range(this List<short> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_short_ulong());
        }

        public static IEnumerable<short> Range(this IList<short> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_short_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<short> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_short_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<short> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_short_ulong());
        }

        public static (int Start, int Length) IndexRange(this short[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_short_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<short> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_short_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<short> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_short_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<short> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_float(), type);
        }

        public static int FindIndex(this Span<short> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_float(), type);
        }

        public static int FindIndex(this short[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_short_float(), type);
        }

        public static int FindIndex(this List<short> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_float(), type);
        }

        public static int FindIndex(this IList<short> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<short> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_short_float());
        }

        public static int FindNearestIndex(this Span<short> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_short_float());
        }

        public static int FindNearestIndex(this short[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_short_float());
        }

        public static int FindNearestIndex(this List<short> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_short_float());
        }

        public static int FindNearestIndex(this IList<short> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_short_float());
        }

        public static ReadOnlySpan<short> Range(this ReadOnlySpan<short> span, Range<float> range)
        {
            return Range(span, range, new Comparer_short_float());
        }

        public static ReadOnlySpan<short> Range(this Span<short> span, Range<float> range)
        {
            return Range(span, range, new Comparer_short_float());
        }

        public static ReadOnlySpan<short> Range(this short[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_short_float());
        }

        public static ReadOnlySpan<short> Range(this List<short> list, Range<float> range)
        {
            return Range(list, range, new Comparer_short_float());
        }

        public static IEnumerable<short> Range(this IList<short> list, Range<float> range)
        {
            return Range(list, range, new Comparer_short_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<short> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_short_float());
        }

        public static (int Start, int Length) IndexRange(this Span<short> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_short_float());
        }

        public static (int Start, int Length) IndexRange(this short[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_short_float());
        }

        public static (int Start, int Length) IndexRange(this List<short> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_short_float());
        }

        public static (int Start, int Length) IndexRange(this IList<short> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_short_float());
        }

        public static int FindIndex(this ReadOnlySpan<short> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_double(), type);
        }

        public static int FindIndex(this Span<short> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_double(), type);
        }

        public static int FindIndex(this short[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_short_double(), type);
        }

        public static int FindIndex(this List<short> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_double(), type);
        }

        public static int FindIndex(this IList<short> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<short> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_short_double());
        }

        public static int FindNearestIndex(this Span<short> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_short_double());
        }

        public static int FindNearestIndex(this short[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_short_double());
        }

        public static int FindNearestIndex(this List<short> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_short_double());
        }

        public static int FindNearestIndex(this IList<short> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_short_double());
        }

        public static ReadOnlySpan<short> Range(this ReadOnlySpan<short> span, Range<double> range)
        {
            return Range(span, range, new Comparer_short_double());
        }

        public static ReadOnlySpan<short> Range(this Span<short> span, Range<double> range)
        {
            return Range(span, range, new Comparer_short_double());
        }

        public static ReadOnlySpan<short> Range(this short[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_short_double());
        }

        public static ReadOnlySpan<short> Range(this List<short> list, Range<double> range)
        {
            return Range(list, range, new Comparer_short_double());
        }

        public static IEnumerable<short> Range(this IList<short> list, Range<double> range)
        {
            return Range(list, range, new Comparer_short_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<short> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_short_double());
        }

        public static (int Start, int Length) IndexRange(this Span<short> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_short_double());
        }

        public static (int Start, int Length) IndexRange(this short[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_short_double());
        }

        public static (int Start, int Length) IndexRange(this List<short> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_short_double());
        }

        public static (int Start, int Length) IndexRange(this IList<short> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_short_double());
        }

        public static int FindIndex(this ReadOnlySpan<short> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_decimal(), type);
        }

        public static int FindIndex(this Span<short> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_decimal(), type);
        }

        public static int FindIndex(this short[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_short_decimal(), type);
        }

        public static int FindIndex(this List<short> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_decimal(), type);
        }

        public static int FindIndex(this IList<short> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<short> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_short_decimal());
        }

        public static int FindNearestIndex(this Span<short> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_short_decimal());
        }

        public static int FindNearestIndex(this short[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_short_decimal());
        }

        public static int FindNearestIndex(this List<short> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_short_decimal());
        }

        public static int FindNearestIndex(this IList<short> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_short_decimal());
        }

        public static ReadOnlySpan<short> Range(this ReadOnlySpan<short> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_short_decimal());
        }

        public static ReadOnlySpan<short> Range(this Span<short> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_short_decimal());
        }

        public static ReadOnlySpan<short> Range(this short[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_short_decimal());
        }

        public static ReadOnlySpan<short> Range(this List<short> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_short_decimal());
        }

        public static IEnumerable<short> Range(this IList<short> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_short_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<short> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_short_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<short> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_short_decimal());
        }

        public static (int Start, int Length) IndexRange(this short[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_short_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<short> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_short_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<short> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_short_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<short> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_Rational(), type);
        }

        public static int FindIndex(this Span<short> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_short_Rational(), type);
        }

        public static int FindIndex(this short[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_short_Rational(), type);
        }

        public static int FindIndex(this List<short> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_Rational(), type);
        }

        public static int FindIndex(this IList<short> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_short_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<short> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_short_Rational());
        }

        public static int FindNearestIndex(this Span<short> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_short_Rational());
        }

        public static int FindNearestIndex(this short[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_short_Rational());
        }

        public static int FindNearestIndex(this List<short> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_short_Rational());
        }

        public static int FindNearestIndex(this IList<short> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_short_Rational());
        }

        public static ReadOnlySpan<short> Range(this ReadOnlySpan<short> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_short_Rational());
        }

        public static ReadOnlySpan<short> Range(this Span<short> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_short_Rational());
        }

        public static ReadOnlySpan<short> Range(this short[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_short_Rational());
        }

        public static ReadOnlySpan<short> Range(this List<short> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_short_Rational());
        }

        public static IEnumerable<short> Range(this IList<short> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_short_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<short> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_short_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<short> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_short_Rational());
        }

        public static (int Start, int Length) IndexRange(this short[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_short_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<short> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_short_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<short> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_short_Rational());
        }

    }
}