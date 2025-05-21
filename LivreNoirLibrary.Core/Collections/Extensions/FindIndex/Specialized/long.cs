using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<long> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_int(), type);
        }

        public static int FindIndex(this Span<long> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_int(), type);
        }

        public static int FindIndex(this long[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_long_int(), type);
        }

        public static int FindIndex(this List<long> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_int(), type);
        }

        public static int FindIndex(this IList<long> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<long> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_long_int());
        }

        public static int FindNearestIndex(this Span<long> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_long_int());
        }

        public static int FindNearestIndex(this long[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_long_int());
        }

        public static int FindNearestIndex(this List<long> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_long_int());
        }

        public static int FindNearestIndex(this IList<long> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_long_int());
        }

        public static ReadOnlySpan<long> Range(this ReadOnlySpan<long> span, Range<int> range)
        {
            return Range(span, range, new Comparer_long_int());
        }

        public static ReadOnlySpan<long> Range(this Span<long> span, Range<int> range)
        {
            return Range(span, range, new Comparer_long_int());
        }

        public static ReadOnlySpan<long> Range(this long[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_long_int());
        }

        public static ReadOnlySpan<long> Range(this List<long> list, Range<int> range)
        {
            return Range(list, range, new Comparer_long_int());
        }

        public static IEnumerable<long> Range(this IList<long> list, Range<int> range)
        {
            return Range(list, range, new Comparer_long_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<long> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_long_int());
        }

        public static (int Start, int Length) IndexRange(this Span<long> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_long_int());
        }

        public static (int Start, int Length) IndexRange(this long[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_long_int());
        }

        public static (int Start, int Length) IndexRange(this List<long> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_long_int());
        }

        public static (int Start, int Length) IndexRange(this IList<long> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_long_int());
        }

        public static int FindIndex(this ReadOnlySpan<long> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_long(), type);
        }

        public static int FindIndex(this Span<long> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_long(), type);
        }

        public static int FindIndex(this long[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_long_long(), type);
        }

        public static int FindIndex(this List<long> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_long(), type);
        }

        public static int FindIndex(this IList<long> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<long> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_long_long());
        }

        public static int FindNearestIndex(this Span<long> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_long_long());
        }

        public static int FindNearestIndex(this long[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_long_long());
        }

        public static int FindNearestIndex(this List<long> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_long_long());
        }

        public static int FindNearestIndex(this IList<long> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_long_long());
        }

        public static ReadOnlySpan<long> Range(this ReadOnlySpan<long> span, Range<long> range)
        {
            return Range(span, range, new Comparer_long_long());
        }

        public static ReadOnlySpan<long> Range(this Span<long> span, Range<long> range)
        {
            return Range(span, range, new Comparer_long_long());
        }

        public static ReadOnlySpan<long> Range(this long[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_long_long());
        }

        public static ReadOnlySpan<long> Range(this List<long> list, Range<long> range)
        {
            return Range(list, range, new Comparer_long_long());
        }

        public static IEnumerable<long> Range(this IList<long> list, Range<long> range)
        {
            return Range(list, range, new Comparer_long_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<long> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_long_long());
        }

        public static (int Start, int Length) IndexRange(this Span<long> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_long_long());
        }

        public static (int Start, int Length) IndexRange(this long[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_long_long());
        }

        public static (int Start, int Length) IndexRange(this List<long> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_long_long());
        }

        public static (int Start, int Length) IndexRange(this IList<long> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_long_long());
        }

        public static int FindIndex(this ReadOnlySpan<long> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_ulong(), type);
        }

        public static int FindIndex(this Span<long> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_ulong(), type);
        }

        public static int FindIndex(this long[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_long_ulong(), type);
        }

        public static int FindIndex(this List<long> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_ulong(), type);
        }

        public static int FindIndex(this IList<long> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<long> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_long_ulong());
        }

        public static int FindNearestIndex(this Span<long> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_long_ulong());
        }

        public static int FindNearestIndex(this long[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_long_ulong());
        }

        public static int FindNearestIndex(this List<long> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_long_ulong());
        }

        public static int FindNearestIndex(this IList<long> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_long_ulong());
        }

        public static ReadOnlySpan<long> Range(this ReadOnlySpan<long> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_long_ulong());
        }

        public static ReadOnlySpan<long> Range(this Span<long> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_long_ulong());
        }

        public static ReadOnlySpan<long> Range(this long[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_long_ulong());
        }

        public static ReadOnlySpan<long> Range(this List<long> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_long_ulong());
        }

        public static IEnumerable<long> Range(this IList<long> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_long_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<long> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_long_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<long> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_long_ulong());
        }

        public static (int Start, int Length) IndexRange(this long[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_long_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<long> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_long_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<long> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_long_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<long> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_float(), type);
        }

        public static int FindIndex(this Span<long> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_float(), type);
        }

        public static int FindIndex(this long[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_long_float(), type);
        }

        public static int FindIndex(this List<long> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_float(), type);
        }

        public static int FindIndex(this IList<long> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<long> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_long_float());
        }

        public static int FindNearestIndex(this Span<long> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_long_float());
        }

        public static int FindNearestIndex(this long[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_long_float());
        }

        public static int FindNearestIndex(this List<long> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_long_float());
        }

        public static int FindNearestIndex(this IList<long> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_long_float());
        }

        public static ReadOnlySpan<long> Range(this ReadOnlySpan<long> span, Range<float> range)
        {
            return Range(span, range, new Comparer_long_float());
        }

        public static ReadOnlySpan<long> Range(this Span<long> span, Range<float> range)
        {
            return Range(span, range, new Comparer_long_float());
        }

        public static ReadOnlySpan<long> Range(this long[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_long_float());
        }

        public static ReadOnlySpan<long> Range(this List<long> list, Range<float> range)
        {
            return Range(list, range, new Comparer_long_float());
        }

        public static IEnumerable<long> Range(this IList<long> list, Range<float> range)
        {
            return Range(list, range, new Comparer_long_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<long> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_long_float());
        }

        public static (int Start, int Length) IndexRange(this Span<long> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_long_float());
        }

        public static (int Start, int Length) IndexRange(this long[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_long_float());
        }

        public static (int Start, int Length) IndexRange(this List<long> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_long_float());
        }

        public static (int Start, int Length) IndexRange(this IList<long> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_long_float());
        }

        public static int FindIndex(this ReadOnlySpan<long> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_double(), type);
        }

        public static int FindIndex(this Span<long> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_double(), type);
        }

        public static int FindIndex(this long[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_long_double(), type);
        }

        public static int FindIndex(this List<long> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_double(), type);
        }

        public static int FindIndex(this IList<long> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<long> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_long_double());
        }

        public static int FindNearestIndex(this Span<long> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_long_double());
        }

        public static int FindNearestIndex(this long[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_long_double());
        }

        public static int FindNearestIndex(this List<long> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_long_double());
        }

        public static int FindNearestIndex(this IList<long> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_long_double());
        }

        public static ReadOnlySpan<long> Range(this ReadOnlySpan<long> span, Range<double> range)
        {
            return Range(span, range, new Comparer_long_double());
        }

        public static ReadOnlySpan<long> Range(this Span<long> span, Range<double> range)
        {
            return Range(span, range, new Comparer_long_double());
        }

        public static ReadOnlySpan<long> Range(this long[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_long_double());
        }

        public static ReadOnlySpan<long> Range(this List<long> list, Range<double> range)
        {
            return Range(list, range, new Comparer_long_double());
        }

        public static IEnumerable<long> Range(this IList<long> list, Range<double> range)
        {
            return Range(list, range, new Comparer_long_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<long> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_long_double());
        }

        public static (int Start, int Length) IndexRange(this Span<long> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_long_double());
        }

        public static (int Start, int Length) IndexRange(this long[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_long_double());
        }

        public static (int Start, int Length) IndexRange(this List<long> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_long_double());
        }

        public static (int Start, int Length) IndexRange(this IList<long> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_long_double());
        }

        public static int FindIndex(this ReadOnlySpan<long> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_decimal(), type);
        }

        public static int FindIndex(this Span<long> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_decimal(), type);
        }

        public static int FindIndex(this long[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_long_decimal(), type);
        }

        public static int FindIndex(this List<long> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_decimal(), type);
        }

        public static int FindIndex(this IList<long> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<long> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_long_decimal());
        }

        public static int FindNearestIndex(this Span<long> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_long_decimal());
        }

        public static int FindNearestIndex(this long[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_long_decimal());
        }

        public static int FindNearestIndex(this List<long> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_long_decimal());
        }

        public static int FindNearestIndex(this IList<long> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_long_decimal());
        }

        public static ReadOnlySpan<long> Range(this ReadOnlySpan<long> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_long_decimal());
        }

        public static ReadOnlySpan<long> Range(this Span<long> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_long_decimal());
        }

        public static ReadOnlySpan<long> Range(this long[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_long_decimal());
        }

        public static ReadOnlySpan<long> Range(this List<long> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_long_decimal());
        }

        public static IEnumerable<long> Range(this IList<long> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_long_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<long> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_long_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<long> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_long_decimal());
        }

        public static (int Start, int Length) IndexRange(this long[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_long_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<long> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_long_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<long> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_long_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<long> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_Rational(), type);
        }

        public static int FindIndex(this Span<long> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_long_Rational(), type);
        }

        public static int FindIndex(this long[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_long_Rational(), type);
        }

        public static int FindIndex(this List<long> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_Rational(), type);
        }

        public static int FindIndex(this IList<long> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_long_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<long> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_long_Rational());
        }

        public static int FindNearestIndex(this Span<long> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_long_Rational());
        }

        public static int FindNearestIndex(this long[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_long_Rational());
        }

        public static int FindNearestIndex(this List<long> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_long_Rational());
        }

        public static int FindNearestIndex(this IList<long> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_long_Rational());
        }

        public static ReadOnlySpan<long> Range(this ReadOnlySpan<long> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_long_Rational());
        }

        public static ReadOnlySpan<long> Range(this Span<long> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_long_Rational());
        }

        public static ReadOnlySpan<long> Range(this long[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_long_Rational());
        }

        public static ReadOnlySpan<long> Range(this List<long> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_long_Rational());
        }

        public static IEnumerable<long> Range(this IList<long> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_long_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<long> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_long_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<long> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_long_Rational());
        }

        public static (int Start, int Length) IndexRange(this long[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_long_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<long> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_long_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<long> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_long_Rational());
        }

    }
}