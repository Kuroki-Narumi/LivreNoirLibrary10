using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<ulong> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_int(), type);
        }

        public static int FindIndex(this Span<ulong> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_int(), type);
        }

        public static int FindIndex(this ulong[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ulong_int(), type);
        }

        public static int FindIndex(this List<ulong> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_int(), type);
        }

        public static int FindIndex(this IList<ulong> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ulong> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_int());
        }

        public static int FindNearestIndex(this Span<ulong> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_int());
        }

        public static int FindNearestIndex(this ulong[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_ulong_int());
        }

        public static int FindNearestIndex(this List<ulong> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_int());
        }

        public static int FindNearestIndex(this IList<ulong> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_int());
        }

        public static ReadOnlySpan<ulong> Range(this ReadOnlySpan<ulong> span, Range<int> range)
        {
            return Range(span, range, new Comparer_ulong_int());
        }

        public static ReadOnlySpan<ulong> Range(this Span<ulong> span, Range<int> range)
        {
            return Range(span, range, new Comparer_ulong_int());
        }

        public static ReadOnlySpan<ulong> Range(this ulong[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_ulong_int());
        }

        public static ReadOnlySpan<ulong> Range(this List<ulong> list, Range<int> range)
        {
            return Range(list, range, new Comparer_ulong_int());
        }

        public static IEnumerable<ulong> Range(this IList<ulong> list, Range<int> range)
        {
            return Range(list, range, new Comparer_ulong_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ulong> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_ulong_int());
        }

        public static (int Start, int Length) IndexRange(this Span<ulong> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_ulong_int());
        }

        public static (int Start, int Length) IndexRange(this ulong[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_ulong_int());
        }

        public static (int Start, int Length) IndexRange(this List<ulong> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_ulong_int());
        }

        public static (int Start, int Length) IndexRange(this IList<ulong> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_ulong_int());
        }

        public static int FindIndex(this ReadOnlySpan<ulong> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_long(), type);
        }

        public static int FindIndex(this Span<ulong> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_long(), type);
        }

        public static int FindIndex(this ulong[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ulong_long(), type);
        }

        public static int FindIndex(this List<ulong> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_long(), type);
        }

        public static int FindIndex(this IList<ulong> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ulong> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_long());
        }

        public static int FindNearestIndex(this Span<ulong> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_long());
        }

        public static int FindNearestIndex(this ulong[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_ulong_long());
        }

        public static int FindNearestIndex(this List<ulong> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_long());
        }

        public static int FindNearestIndex(this IList<ulong> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_long());
        }

        public static ReadOnlySpan<ulong> Range(this ReadOnlySpan<ulong> span, Range<long> range)
        {
            return Range(span, range, new Comparer_ulong_long());
        }

        public static ReadOnlySpan<ulong> Range(this Span<ulong> span, Range<long> range)
        {
            return Range(span, range, new Comparer_ulong_long());
        }

        public static ReadOnlySpan<ulong> Range(this ulong[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_ulong_long());
        }

        public static ReadOnlySpan<ulong> Range(this List<ulong> list, Range<long> range)
        {
            return Range(list, range, new Comparer_ulong_long());
        }

        public static IEnumerable<ulong> Range(this IList<ulong> list, Range<long> range)
        {
            return Range(list, range, new Comparer_ulong_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ulong> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_ulong_long());
        }

        public static (int Start, int Length) IndexRange(this Span<ulong> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_ulong_long());
        }

        public static (int Start, int Length) IndexRange(this ulong[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_ulong_long());
        }

        public static (int Start, int Length) IndexRange(this List<ulong> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_ulong_long());
        }

        public static (int Start, int Length) IndexRange(this IList<ulong> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_ulong_long());
        }

        public static int FindIndex(this ReadOnlySpan<ulong> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_ulong(), type);
        }

        public static int FindIndex(this Span<ulong> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_ulong(), type);
        }

        public static int FindIndex(this ulong[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ulong_ulong(), type);
        }

        public static int FindIndex(this List<ulong> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_ulong(), type);
        }

        public static int FindIndex(this IList<ulong> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ulong> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_ulong());
        }

        public static int FindNearestIndex(this Span<ulong> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_ulong());
        }

        public static int FindNearestIndex(this ulong[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_ulong_ulong());
        }

        public static int FindNearestIndex(this List<ulong> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_ulong());
        }

        public static int FindNearestIndex(this IList<ulong> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_ulong());
        }

        public static ReadOnlySpan<ulong> Range(this ReadOnlySpan<ulong> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_ulong_ulong());
        }

        public static ReadOnlySpan<ulong> Range(this Span<ulong> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_ulong_ulong());
        }

        public static ReadOnlySpan<ulong> Range(this ulong[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_ulong_ulong());
        }

        public static ReadOnlySpan<ulong> Range(this List<ulong> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_ulong_ulong());
        }

        public static IEnumerable<ulong> Range(this IList<ulong> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_ulong_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ulong> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_ulong_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<ulong> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_ulong_ulong());
        }

        public static (int Start, int Length) IndexRange(this ulong[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_ulong_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<ulong> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_ulong_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<ulong> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_ulong_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<ulong> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_float(), type);
        }

        public static int FindIndex(this Span<ulong> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_float(), type);
        }

        public static int FindIndex(this ulong[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ulong_float(), type);
        }

        public static int FindIndex(this List<ulong> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_float(), type);
        }

        public static int FindIndex(this IList<ulong> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ulong> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_float());
        }

        public static int FindNearestIndex(this Span<ulong> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_float());
        }

        public static int FindNearestIndex(this ulong[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_ulong_float());
        }

        public static int FindNearestIndex(this List<ulong> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_float());
        }

        public static int FindNearestIndex(this IList<ulong> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_float());
        }

        public static ReadOnlySpan<ulong> Range(this ReadOnlySpan<ulong> span, Range<float> range)
        {
            return Range(span, range, new Comparer_ulong_float());
        }

        public static ReadOnlySpan<ulong> Range(this Span<ulong> span, Range<float> range)
        {
            return Range(span, range, new Comparer_ulong_float());
        }

        public static ReadOnlySpan<ulong> Range(this ulong[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_ulong_float());
        }

        public static ReadOnlySpan<ulong> Range(this List<ulong> list, Range<float> range)
        {
            return Range(list, range, new Comparer_ulong_float());
        }

        public static IEnumerable<ulong> Range(this IList<ulong> list, Range<float> range)
        {
            return Range(list, range, new Comparer_ulong_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ulong> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_ulong_float());
        }

        public static (int Start, int Length) IndexRange(this Span<ulong> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_ulong_float());
        }

        public static (int Start, int Length) IndexRange(this ulong[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_ulong_float());
        }

        public static (int Start, int Length) IndexRange(this List<ulong> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_ulong_float());
        }

        public static (int Start, int Length) IndexRange(this IList<ulong> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_ulong_float());
        }

        public static int FindIndex(this ReadOnlySpan<ulong> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_double(), type);
        }

        public static int FindIndex(this Span<ulong> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_double(), type);
        }

        public static int FindIndex(this ulong[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ulong_double(), type);
        }

        public static int FindIndex(this List<ulong> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_double(), type);
        }

        public static int FindIndex(this IList<ulong> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ulong> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_double());
        }

        public static int FindNearestIndex(this Span<ulong> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_double());
        }

        public static int FindNearestIndex(this ulong[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_ulong_double());
        }

        public static int FindNearestIndex(this List<ulong> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_double());
        }

        public static int FindNearestIndex(this IList<ulong> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_double());
        }

        public static ReadOnlySpan<ulong> Range(this ReadOnlySpan<ulong> span, Range<double> range)
        {
            return Range(span, range, new Comparer_ulong_double());
        }

        public static ReadOnlySpan<ulong> Range(this Span<ulong> span, Range<double> range)
        {
            return Range(span, range, new Comparer_ulong_double());
        }

        public static ReadOnlySpan<ulong> Range(this ulong[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_ulong_double());
        }

        public static ReadOnlySpan<ulong> Range(this List<ulong> list, Range<double> range)
        {
            return Range(list, range, new Comparer_ulong_double());
        }

        public static IEnumerable<ulong> Range(this IList<ulong> list, Range<double> range)
        {
            return Range(list, range, new Comparer_ulong_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ulong> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_ulong_double());
        }

        public static (int Start, int Length) IndexRange(this Span<ulong> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_ulong_double());
        }

        public static (int Start, int Length) IndexRange(this ulong[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_ulong_double());
        }

        public static (int Start, int Length) IndexRange(this List<ulong> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_ulong_double());
        }

        public static (int Start, int Length) IndexRange(this IList<ulong> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_ulong_double());
        }

        public static int FindIndex(this ReadOnlySpan<ulong> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_decimal(), type);
        }

        public static int FindIndex(this Span<ulong> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_decimal(), type);
        }

        public static int FindIndex(this ulong[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ulong_decimal(), type);
        }

        public static int FindIndex(this List<ulong> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_decimal(), type);
        }

        public static int FindIndex(this IList<ulong> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ulong> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_decimal());
        }

        public static int FindNearestIndex(this Span<ulong> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_decimal());
        }

        public static int FindNearestIndex(this ulong[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_ulong_decimal());
        }

        public static int FindNearestIndex(this List<ulong> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_decimal());
        }

        public static int FindNearestIndex(this IList<ulong> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_decimal());
        }

        public static ReadOnlySpan<ulong> Range(this ReadOnlySpan<ulong> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_ulong_decimal());
        }

        public static ReadOnlySpan<ulong> Range(this Span<ulong> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_ulong_decimal());
        }

        public static ReadOnlySpan<ulong> Range(this ulong[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_ulong_decimal());
        }

        public static ReadOnlySpan<ulong> Range(this List<ulong> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_ulong_decimal());
        }

        public static IEnumerable<ulong> Range(this IList<ulong> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_ulong_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ulong> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_ulong_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<ulong> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_ulong_decimal());
        }

        public static (int Start, int Length) IndexRange(this ulong[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_ulong_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<ulong> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_ulong_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<ulong> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_ulong_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<ulong> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_Rational(), type);
        }

        public static int FindIndex(this Span<ulong> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ulong_Rational(), type);
        }

        public static int FindIndex(this ulong[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ulong_Rational(), type);
        }

        public static int FindIndex(this List<ulong> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_Rational(), type);
        }

        public static int FindIndex(this IList<ulong> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ulong_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ulong> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_Rational());
        }

        public static int FindNearestIndex(this Span<ulong> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_ulong_Rational());
        }

        public static int FindNearestIndex(this ulong[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_ulong_Rational());
        }

        public static int FindNearestIndex(this List<ulong> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_Rational());
        }

        public static int FindNearestIndex(this IList<ulong> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_ulong_Rational());
        }

        public static ReadOnlySpan<ulong> Range(this ReadOnlySpan<ulong> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_ulong_Rational());
        }

        public static ReadOnlySpan<ulong> Range(this Span<ulong> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_ulong_Rational());
        }

        public static ReadOnlySpan<ulong> Range(this ulong[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_ulong_Rational());
        }

        public static ReadOnlySpan<ulong> Range(this List<ulong> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_ulong_Rational());
        }

        public static IEnumerable<ulong> Range(this IList<ulong> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_ulong_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ulong> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_ulong_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<ulong> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_ulong_Rational());
        }

        public static (int Start, int Length) IndexRange(this ulong[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_ulong_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<ulong> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_ulong_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<ulong> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_ulong_Rational());
        }

    }
}