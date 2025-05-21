using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<int> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_int(), type);
        }

        public static int FindIndex(this Span<int> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_int(), type);
        }

        public static int FindIndex(this int[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_int_int(), type);
        }

        public static int FindIndex(this List<int> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_int(), type);
        }

        public static int FindIndex(this IList<int> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<int> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_int_int());
        }

        public static int FindNearestIndex(this Span<int> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_int_int());
        }

        public static int FindNearestIndex(this int[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_int_int());
        }

        public static int FindNearestIndex(this List<int> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_int_int());
        }

        public static int FindNearestIndex(this IList<int> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_int_int());
        }

        public static ReadOnlySpan<int> Range(this ReadOnlySpan<int> span, Range<int> range)
        {
            return Range(span, range, new Comparer_int_int());
        }

        public static ReadOnlySpan<int> Range(this Span<int> span, Range<int> range)
        {
            return Range(span, range, new Comparer_int_int());
        }

        public static ReadOnlySpan<int> Range(this int[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_int_int());
        }

        public static ReadOnlySpan<int> Range(this List<int> list, Range<int> range)
        {
            return Range(list, range, new Comparer_int_int());
        }

        public static IEnumerable<int> Range(this IList<int> list, Range<int> range)
        {
            return Range(list, range, new Comparer_int_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<int> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_int_int());
        }

        public static (int Start, int Length) IndexRange(this Span<int> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_int_int());
        }

        public static (int Start, int Length) IndexRange(this int[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_int_int());
        }

        public static (int Start, int Length) IndexRange(this List<int> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_int_int());
        }

        public static (int Start, int Length) IndexRange(this IList<int> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_int_int());
        }

        public static int FindIndex(this ReadOnlySpan<int> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_long(), type);
        }

        public static int FindIndex(this Span<int> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_long(), type);
        }

        public static int FindIndex(this int[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_int_long(), type);
        }

        public static int FindIndex(this List<int> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_long(), type);
        }

        public static int FindIndex(this IList<int> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<int> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_int_long());
        }

        public static int FindNearestIndex(this Span<int> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_int_long());
        }

        public static int FindNearestIndex(this int[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_int_long());
        }

        public static int FindNearestIndex(this List<int> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_int_long());
        }

        public static int FindNearestIndex(this IList<int> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_int_long());
        }

        public static ReadOnlySpan<int> Range(this ReadOnlySpan<int> span, Range<long> range)
        {
            return Range(span, range, new Comparer_int_long());
        }

        public static ReadOnlySpan<int> Range(this Span<int> span, Range<long> range)
        {
            return Range(span, range, new Comparer_int_long());
        }

        public static ReadOnlySpan<int> Range(this int[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_int_long());
        }

        public static ReadOnlySpan<int> Range(this List<int> list, Range<long> range)
        {
            return Range(list, range, new Comparer_int_long());
        }

        public static IEnumerable<int> Range(this IList<int> list, Range<long> range)
        {
            return Range(list, range, new Comparer_int_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<int> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_int_long());
        }

        public static (int Start, int Length) IndexRange(this Span<int> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_int_long());
        }

        public static (int Start, int Length) IndexRange(this int[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_int_long());
        }

        public static (int Start, int Length) IndexRange(this List<int> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_int_long());
        }

        public static (int Start, int Length) IndexRange(this IList<int> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_int_long());
        }

        public static int FindIndex(this ReadOnlySpan<int> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_ulong(), type);
        }

        public static int FindIndex(this Span<int> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_ulong(), type);
        }

        public static int FindIndex(this int[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_int_ulong(), type);
        }

        public static int FindIndex(this List<int> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_ulong(), type);
        }

        public static int FindIndex(this IList<int> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<int> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_int_ulong());
        }

        public static int FindNearestIndex(this Span<int> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_int_ulong());
        }

        public static int FindNearestIndex(this int[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_int_ulong());
        }

        public static int FindNearestIndex(this List<int> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_int_ulong());
        }

        public static int FindNearestIndex(this IList<int> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_int_ulong());
        }

        public static ReadOnlySpan<int> Range(this ReadOnlySpan<int> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_int_ulong());
        }

        public static ReadOnlySpan<int> Range(this Span<int> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_int_ulong());
        }

        public static ReadOnlySpan<int> Range(this int[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_int_ulong());
        }

        public static ReadOnlySpan<int> Range(this List<int> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_int_ulong());
        }

        public static IEnumerable<int> Range(this IList<int> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_int_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<int> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_int_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<int> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_int_ulong());
        }

        public static (int Start, int Length) IndexRange(this int[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_int_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<int> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_int_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<int> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_int_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<int> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_float(), type);
        }

        public static int FindIndex(this Span<int> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_float(), type);
        }

        public static int FindIndex(this int[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_int_float(), type);
        }

        public static int FindIndex(this List<int> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_float(), type);
        }

        public static int FindIndex(this IList<int> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<int> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_int_float());
        }

        public static int FindNearestIndex(this Span<int> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_int_float());
        }

        public static int FindNearestIndex(this int[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_int_float());
        }

        public static int FindNearestIndex(this List<int> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_int_float());
        }

        public static int FindNearestIndex(this IList<int> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_int_float());
        }

        public static ReadOnlySpan<int> Range(this ReadOnlySpan<int> span, Range<float> range)
        {
            return Range(span, range, new Comparer_int_float());
        }

        public static ReadOnlySpan<int> Range(this Span<int> span, Range<float> range)
        {
            return Range(span, range, new Comparer_int_float());
        }

        public static ReadOnlySpan<int> Range(this int[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_int_float());
        }

        public static ReadOnlySpan<int> Range(this List<int> list, Range<float> range)
        {
            return Range(list, range, new Comparer_int_float());
        }

        public static IEnumerable<int> Range(this IList<int> list, Range<float> range)
        {
            return Range(list, range, new Comparer_int_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<int> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_int_float());
        }

        public static (int Start, int Length) IndexRange(this Span<int> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_int_float());
        }

        public static (int Start, int Length) IndexRange(this int[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_int_float());
        }

        public static (int Start, int Length) IndexRange(this List<int> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_int_float());
        }

        public static (int Start, int Length) IndexRange(this IList<int> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_int_float());
        }

        public static int FindIndex(this ReadOnlySpan<int> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_double(), type);
        }

        public static int FindIndex(this Span<int> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_double(), type);
        }

        public static int FindIndex(this int[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_int_double(), type);
        }

        public static int FindIndex(this List<int> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_double(), type);
        }

        public static int FindIndex(this IList<int> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<int> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_int_double());
        }

        public static int FindNearestIndex(this Span<int> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_int_double());
        }

        public static int FindNearestIndex(this int[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_int_double());
        }

        public static int FindNearestIndex(this List<int> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_int_double());
        }

        public static int FindNearestIndex(this IList<int> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_int_double());
        }

        public static ReadOnlySpan<int> Range(this ReadOnlySpan<int> span, Range<double> range)
        {
            return Range(span, range, new Comparer_int_double());
        }

        public static ReadOnlySpan<int> Range(this Span<int> span, Range<double> range)
        {
            return Range(span, range, new Comparer_int_double());
        }

        public static ReadOnlySpan<int> Range(this int[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_int_double());
        }

        public static ReadOnlySpan<int> Range(this List<int> list, Range<double> range)
        {
            return Range(list, range, new Comparer_int_double());
        }

        public static IEnumerable<int> Range(this IList<int> list, Range<double> range)
        {
            return Range(list, range, new Comparer_int_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<int> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_int_double());
        }

        public static (int Start, int Length) IndexRange(this Span<int> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_int_double());
        }

        public static (int Start, int Length) IndexRange(this int[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_int_double());
        }

        public static (int Start, int Length) IndexRange(this List<int> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_int_double());
        }

        public static (int Start, int Length) IndexRange(this IList<int> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_int_double());
        }

        public static int FindIndex(this ReadOnlySpan<int> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_decimal(), type);
        }

        public static int FindIndex(this Span<int> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_decimal(), type);
        }

        public static int FindIndex(this int[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_int_decimal(), type);
        }

        public static int FindIndex(this List<int> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_decimal(), type);
        }

        public static int FindIndex(this IList<int> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<int> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_int_decimal());
        }

        public static int FindNearestIndex(this Span<int> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_int_decimal());
        }

        public static int FindNearestIndex(this int[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_int_decimal());
        }

        public static int FindNearestIndex(this List<int> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_int_decimal());
        }

        public static int FindNearestIndex(this IList<int> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_int_decimal());
        }

        public static ReadOnlySpan<int> Range(this ReadOnlySpan<int> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_int_decimal());
        }

        public static ReadOnlySpan<int> Range(this Span<int> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_int_decimal());
        }

        public static ReadOnlySpan<int> Range(this int[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_int_decimal());
        }

        public static ReadOnlySpan<int> Range(this List<int> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_int_decimal());
        }

        public static IEnumerable<int> Range(this IList<int> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_int_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<int> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_int_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<int> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_int_decimal());
        }

        public static (int Start, int Length) IndexRange(this int[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_int_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<int> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_int_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<int> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_int_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<int> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_Rational(), type);
        }

        public static int FindIndex(this Span<int> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_int_Rational(), type);
        }

        public static int FindIndex(this int[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_int_Rational(), type);
        }

        public static int FindIndex(this List<int> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_Rational(), type);
        }

        public static int FindIndex(this IList<int> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_int_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<int> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_int_Rational());
        }

        public static int FindNearestIndex(this Span<int> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_int_Rational());
        }

        public static int FindNearestIndex(this int[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_int_Rational());
        }

        public static int FindNearestIndex(this List<int> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_int_Rational());
        }

        public static int FindNearestIndex(this IList<int> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_int_Rational());
        }

        public static ReadOnlySpan<int> Range(this ReadOnlySpan<int> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_int_Rational());
        }

        public static ReadOnlySpan<int> Range(this Span<int> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_int_Rational());
        }

        public static ReadOnlySpan<int> Range(this int[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_int_Rational());
        }

        public static ReadOnlySpan<int> Range(this List<int> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_int_Rational());
        }

        public static IEnumerable<int> Range(this IList<int> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_int_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<int> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_int_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<int> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_int_Rational());
        }

        public static (int Start, int Length) IndexRange(this int[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_int_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<int> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_int_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<int> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_int_Rational());
        }

    }
}