using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<Rational> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_int(), type);
        }

        public static int FindIndex(this Span<Rational> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_int(), type);
        }

        public static int FindIndex(this Rational[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_Rational_int(), type);
        }

        public static int FindIndex(this List<Rational> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_int(), type);
        }

        public static int FindIndex(this IList<Rational> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<Rational> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_int());
        }

        public static int FindNearestIndex(this Span<Rational> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_int());
        }

        public static int FindNearestIndex(this Rational[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_Rational_int());
        }

        public static int FindNearestIndex(this List<Rational> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_int());
        }

        public static int FindNearestIndex(this IList<Rational> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_int());
        }

        public static ReadOnlySpan<Rational> Range(this ReadOnlySpan<Rational> span, Range<int> range)
        {
            return Range(span, range, new Comparer_Rational_int());
        }

        public static ReadOnlySpan<Rational> Range(this Span<Rational> span, Range<int> range)
        {
            return Range(span, range, new Comparer_Rational_int());
        }

        public static ReadOnlySpan<Rational> Range(this Rational[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_Rational_int());
        }

        public static ReadOnlySpan<Rational> Range(this List<Rational> list, Range<int> range)
        {
            return Range(list, range, new Comparer_Rational_int());
        }

        public static IEnumerable<Rational> Range(this IList<Rational> list, Range<int> range)
        {
            return Range(list, range, new Comparer_Rational_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<Rational> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_Rational_int());
        }

        public static (int Start, int Length) IndexRange(this Span<Rational> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_Rational_int());
        }

        public static (int Start, int Length) IndexRange(this Rational[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_Rational_int());
        }

        public static (int Start, int Length) IndexRange(this List<Rational> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_Rational_int());
        }

        public static (int Start, int Length) IndexRange(this IList<Rational> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_Rational_int());
        }

        public static int FindIndex(this ReadOnlySpan<Rational> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_long(), type);
        }

        public static int FindIndex(this Span<Rational> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_long(), type);
        }

        public static int FindIndex(this Rational[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_Rational_long(), type);
        }

        public static int FindIndex(this List<Rational> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_long(), type);
        }

        public static int FindIndex(this IList<Rational> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<Rational> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_long());
        }

        public static int FindNearestIndex(this Span<Rational> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_long());
        }

        public static int FindNearestIndex(this Rational[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_Rational_long());
        }

        public static int FindNearestIndex(this List<Rational> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_long());
        }

        public static int FindNearestIndex(this IList<Rational> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_long());
        }

        public static ReadOnlySpan<Rational> Range(this ReadOnlySpan<Rational> span, Range<long> range)
        {
            return Range(span, range, new Comparer_Rational_long());
        }

        public static ReadOnlySpan<Rational> Range(this Span<Rational> span, Range<long> range)
        {
            return Range(span, range, new Comparer_Rational_long());
        }

        public static ReadOnlySpan<Rational> Range(this Rational[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_Rational_long());
        }

        public static ReadOnlySpan<Rational> Range(this List<Rational> list, Range<long> range)
        {
            return Range(list, range, new Comparer_Rational_long());
        }

        public static IEnumerable<Rational> Range(this IList<Rational> list, Range<long> range)
        {
            return Range(list, range, new Comparer_Rational_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<Rational> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_Rational_long());
        }

        public static (int Start, int Length) IndexRange(this Span<Rational> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_Rational_long());
        }

        public static (int Start, int Length) IndexRange(this Rational[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_Rational_long());
        }

        public static (int Start, int Length) IndexRange(this List<Rational> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_Rational_long());
        }

        public static (int Start, int Length) IndexRange(this IList<Rational> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_Rational_long());
        }

        public static int FindIndex(this ReadOnlySpan<Rational> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_ulong(), type);
        }

        public static int FindIndex(this Span<Rational> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_ulong(), type);
        }

        public static int FindIndex(this Rational[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_Rational_ulong(), type);
        }

        public static int FindIndex(this List<Rational> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_ulong(), type);
        }

        public static int FindIndex(this IList<Rational> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<Rational> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_ulong());
        }

        public static int FindNearestIndex(this Span<Rational> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_ulong());
        }

        public static int FindNearestIndex(this Rational[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_Rational_ulong());
        }

        public static int FindNearestIndex(this List<Rational> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_ulong());
        }

        public static int FindNearestIndex(this IList<Rational> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_ulong());
        }

        public static ReadOnlySpan<Rational> Range(this ReadOnlySpan<Rational> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_Rational_ulong());
        }

        public static ReadOnlySpan<Rational> Range(this Span<Rational> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_Rational_ulong());
        }

        public static ReadOnlySpan<Rational> Range(this Rational[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_Rational_ulong());
        }

        public static ReadOnlySpan<Rational> Range(this List<Rational> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_Rational_ulong());
        }

        public static IEnumerable<Rational> Range(this IList<Rational> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_Rational_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<Rational> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_Rational_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<Rational> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_Rational_ulong());
        }

        public static (int Start, int Length) IndexRange(this Rational[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_Rational_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<Rational> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_Rational_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<Rational> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_Rational_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<Rational> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_float(), type);
        }

        public static int FindIndex(this Span<Rational> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_float(), type);
        }

        public static int FindIndex(this Rational[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_Rational_float(), type);
        }

        public static int FindIndex(this List<Rational> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_float(), type);
        }

        public static int FindIndex(this IList<Rational> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<Rational> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_float());
        }

        public static int FindNearestIndex(this Span<Rational> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_float());
        }

        public static int FindNearestIndex(this Rational[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_Rational_float());
        }

        public static int FindNearestIndex(this List<Rational> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_float());
        }

        public static int FindNearestIndex(this IList<Rational> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_float());
        }

        public static ReadOnlySpan<Rational> Range(this ReadOnlySpan<Rational> span, Range<float> range)
        {
            return Range(span, range, new Comparer_Rational_float());
        }

        public static ReadOnlySpan<Rational> Range(this Span<Rational> span, Range<float> range)
        {
            return Range(span, range, new Comparer_Rational_float());
        }

        public static ReadOnlySpan<Rational> Range(this Rational[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_Rational_float());
        }

        public static ReadOnlySpan<Rational> Range(this List<Rational> list, Range<float> range)
        {
            return Range(list, range, new Comparer_Rational_float());
        }

        public static IEnumerable<Rational> Range(this IList<Rational> list, Range<float> range)
        {
            return Range(list, range, new Comparer_Rational_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<Rational> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_Rational_float());
        }

        public static (int Start, int Length) IndexRange(this Span<Rational> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_Rational_float());
        }

        public static (int Start, int Length) IndexRange(this Rational[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_Rational_float());
        }

        public static (int Start, int Length) IndexRange(this List<Rational> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_Rational_float());
        }

        public static (int Start, int Length) IndexRange(this IList<Rational> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_Rational_float());
        }

        public static int FindIndex(this ReadOnlySpan<Rational> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_double(), type);
        }

        public static int FindIndex(this Span<Rational> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_double(), type);
        }

        public static int FindIndex(this Rational[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_Rational_double(), type);
        }

        public static int FindIndex(this List<Rational> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_double(), type);
        }

        public static int FindIndex(this IList<Rational> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<Rational> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_double());
        }

        public static int FindNearestIndex(this Span<Rational> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_double());
        }

        public static int FindNearestIndex(this Rational[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_Rational_double());
        }

        public static int FindNearestIndex(this List<Rational> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_double());
        }

        public static int FindNearestIndex(this IList<Rational> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_double());
        }

        public static ReadOnlySpan<Rational> Range(this ReadOnlySpan<Rational> span, Range<double> range)
        {
            return Range(span, range, new Comparer_Rational_double());
        }

        public static ReadOnlySpan<Rational> Range(this Span<Rational> span, Range<double> range)
        {
            return Range(span, range, new Comparer_Rational_double());
        }

        public static ReadOnlySpan<Rational> Range(this Rational[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_Rational_double());
        }

        public static ReadOnlySpan<Rational> Range(this List<Rational> list, Range<double> range)
        {
            return Range(list, range, new Comparer_Rational_double());
        }

        public static IEnumerable<Rational> Range(this IList<Rational> list, Range<double> range)
        {
            return Range(list, range, new Comparer_Rational_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<Rational> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_Rational_double());
        }

        public static (int Start, int Length) IndexRange(this Span<Rational> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_Rational_double());
        }

        public static (int Start, int Length) IndexRange(this Rational[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_Rational_double());
        }

        public static (int Start, int Length) IndexRange(this List<Rational> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_Rational_double());
        }

        public static (int Start, int Length) IndexRange(this IList<Rational> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_Rational_double());
        }

        public static int FindIndex(this ReadOnlySpan<Rational> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_decimal(), type);
        }

        public static int FindIndex(this Span<Rational> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_decimal(), type);
        }

        public static int FindIndex(this Rational[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_Rational_decimal(), type);
        }

        public static int FindIndex(this List<Rational> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_decimal(), type);
        }

        public static int FindIndex(this IList<Rational> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<Rational> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_decimal());
        }

        public static int FindNearestIndex(this Span<Rational> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_decimal());
        }

        public static int FindNearestIndex(this Rational[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_Rational_decimal());
        }

        public static int FindNearestIndex(this List<Rational> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_decimal());
        }

        public static int FindNearestIndex(this IList<Rational> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_decimal());
        }

        public static ReadOnlySpan<Rational> Range(this ReadOnlySpan<Rational> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_Rational_decimal());
        }

        public static ReadOnlySpan<Rational> Range(this Span<Rational> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_Rational_decimal());
        }

        public static ReadOnlySpan<Rational> Range(this Rational[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_Rational_decimal());
        }

        public static ReadOnlySpan<Rational> Range(this List<Rational> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_Rational_decimal());
        }

        public static IEnumerable<Rational> Range(this IList<Rational> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_Rational_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<Rational> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_Rational_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<Rational> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_Rational_decimal());
        }

        public static (int Start, int Length) IndexRange(this Rational[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_Rational_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<Rational> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_Rational_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<Rational> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_Rational_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<Rational> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_Rational(), type);
        }

        public static int FindIndex(this Span<Rational> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_Rational_Rational(), type);
        }

        public static int FindIndex(this Rational[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_Rational_Rational(), type);
        }

        public static int FindIndex(this List<Rational> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_Rational(), type);
        }

        public static int FindIndex(this IList<Rational> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_Rational_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<Rational> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_Rational());
        }

        public static int FindNearestIndex(this Span<Rational> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_Rational_Rational());
        }

        public static int FindNearestIndex(this Rational[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_Rational_Rational());
        }

        public static int FindNearestIndex(this List<Rational> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_Rational());
        }

        public static int FindNearestIndex(this IList<Rational> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_Rational_Rational());
        }

        public static ReadOnlySpan<Rational> Range(this ReadOnlySpan<Rational> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_Rational_Rational());
        }

        public static ReadOnlySpan<Rational> Range(this Span<Rational> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_Rational_Rational());
        }

        public static ReadOnlySpan<Rational> Range(this Rational[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_Rational_Rational());
        }

        public static ReadOnlySpan<Rational> Range(this List<Rational> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_Rational_Rational());
        }

        public static IEnumerable<Rational> Range(this IList<Rational> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_Rational_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<Rational> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_Rational_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<Rational> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_Rational_Rational());
        }

        public static (int Start, int Length) IndexRange(this Rational[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_Rational_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<Rational> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_Rational_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<Rational> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_Rational_Rational());
        }

    }
}