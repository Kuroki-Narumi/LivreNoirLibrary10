using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<double> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_int(), type);
        }

        public static int FindIndex(this Span<double> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_int(), type);
        }

        public static int FindIndex(this double[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_double_int(), type);
        }

        public static int FindIndex(this List<double> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_int(), type);
        }

        public static int FindIndex(this IList<double> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<double> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_double_int());
        }

        public static int FindNearestIndex(this Span<double> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_double_int());
        }

        public static int FindNearestIndex(this double[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_double_int());
        }

        public static int FindNearestIndex(this List<double> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_double_int());
        }

        public static int FindNearestIndex(this IList<double> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_double_int());
        }

        public static ReadOnlySpan<double> Range(this ReadOnlySpan<double> span, Range<int> range)
        {
            return Range(span, range, new Comparer_double_int());
        }

        public static ReadOnlySpan<double> Range(this Span<double> span, Range<int> range)
        {
            return Range(span, range, new Comparer_double_int());
        }

        public static ReadOnlySpan<double> Range(this double[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_double_int());
        }

        public static ReadOnlySpan<double> Range(this List<double> list, Range<int> range)
        {
            return Range(list, range, new Comparer_double_int());
        }

        public static IEnumerable<double> Range(this IList<double> list, Range<int> range)
        {
            return Range(list, range, new Comparer_double_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<double> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_double_int());
        }

        public static (int Start, int Length) IndexRange(this Span<double> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_double_int());
        }

        public static (int Start, int Length) IndexRange(this double[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_double_int());
        }

        public static (int Start, int Length) IndexRange(this List<double> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_double_int());
        }

        public static (int Start, int Length) IndexRange(this IList<double> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_double_int());
        }

        public static int FindIndex(this ReadOnlySpan<double> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_long(), type);
        }

        public static int FindIndex(this Span<double> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_long(), type);
        }

        public static int FindIndex(this double[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_double_long(), type);
        }

        public static int FindIndex(this List<double> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_long(), type);
        }

        public static int FindIndex(this IList<double> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<double> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_double_long());
        }

        public static int FindNearestIndex(this Span<double> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_double_long());
        }

        public static int FindNearestIndex(this double[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_double_long());
        }

        public static int FindNearestIndex(this List<double> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_double_long());
        }

        public static int FindNearestIndex(this IList<double> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_double_long());
        }

        public static ReadOnlySpan<double> Range(this ReadOnlySpan<double> span, Range<long> range)
        {
            return Range(span, range, new Comparer_double_long());
        }

        public static ReadOnlySpan<double> Range(this Span<double> span, Range<long> range)
        {
            return Range(span, range, new Comparer_double_long());
        }

        public static ReadOnlySpan<double> Range(this double[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_double_long());
        }

        public static ReadOnlySpan<double> Range(this List<double> list, Range<long> range)
        {
            return Range(list, range, new Comparer_double_long());
        }

        public static IEnumerable<double> Range(this IList<double> list, Range<long> range)
        {
            return Range(list, range, new Comparer_double_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<double> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_double_long());
        }

        public static (int Start, int Length) IndexRange(this Span<double> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_double_long());
        }

        public static (int Start, int Length) IndexRange(this double[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_double_long());
        }

        public static (int Start, int Length) IndexRange(this List<double> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_double_long());
        }

        public static (int Start, int Length) IndexRange(this IList<double> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_double_long());
        }

        public static int FindIndex(this ReadOnlySpan<double> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_ulong(), type);
        }

        public static int FindIndex(this Span<double> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_ulong(), type);
        }

        public static int FindIndex(this double[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_double_ulong(), type);
        }

        public static int FindIndex(this List<double> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_ulong(), type);
        }

        public static int FindIndex(this IList<double> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<double> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_double_ulong());
        }

        public static int FindNearestIndex(this Span<double> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_double_ulong());
        }

        public static int FindNearestIndex(this double[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_double_ulong());
        }

        public static int FindNearestIndex(this List<double> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_double_ulong());
        }

        public static int FindNearestIndex(this IList<double> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_double_ulong());
        }

        public static ReadOnlySpan<double> Range(this ReadOnlySpan<double> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_double_ulong());
        }

        public static ReadOnlySpan<double> Range(this Span<double> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_double_ulong());
        }

        public static ReadOnlySpan<double> Range(this double[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_double_ulong());
        }

        public static ReadOnlySpan<double> Range(this List<double> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_double_ulong());
        }

        public static IEnumerable<double> Range(this IList<double> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_double_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<double> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_double_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<double> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_double_ulong());
        }

        public static (int Start, int Length) IndexRange(this double[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_double_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<double> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_double_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<double> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_double_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<double> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_float(), type);
        }

        public static int FindIndex(this Span<double> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_float(), type);
        }

        public static int FindIndex(this double[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_double_float(), type);
        }

        public static int FindIndex(this List<double> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_float(), type);
        }

        public static int FindIndex(this IList<double> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<double> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_double_float());
        }

        public static int FindNearestIndex(this Span<double> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_double_float());
        }

        public static int FindNearestIndex(this double[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_double_float());
        }

        public static int FindNearestIndex(this List<double> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_double_float());
        }

        public static int FindNearestIndex(this IList<double> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_double_float());
        }

        public static ReadOnlySpan<double> Range(this ReadOnlySpan<double> span, Range<float> range)
        {
            return Range(span, range, new Comparer_double_float());
        }

        public static ReadOnlySpan<double> Range(this Span<double> span, Range<float> range)
        {
            return Range(span, range, new Comparer_double_float());
        }

        public static ReadOnlySpan<double> Range(this double[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_double_float());
        }

        public static ReadOnlySpan<double> Range(this List<double> list, Range<float> range)
        {
            return Range(list, range, new Comparer_double_float());
        }

        public static IEnumerable<double> Range(this IList<double> list, Range<float> range)
        {
            return Range(list, range, new Comparer_double_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<double> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_double_float());
        }

        public static (int Start, int Length) IndexRange(this Span<double> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_double_float());
        }

        public static (int Start, int Length) IndexRange(this double[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_double_float());
        }

        public static (int Start, int Length) IndexRange(this List<double> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_double_float());
        }

        public static (int Start, int Length) IndexRange(this IList<double> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_double_float());
        }

        public static int FindIndex(this ReadOnlySpan<double> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_double(), type);
        }

        public static int FindIndex(this Span<double> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_double(), type);
        }

        public static int FindIndex(this double[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_double_double(), type);
        }

        public static int FindIndex(this List<double> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_double(), type);
        }

        public static int FindIndex(this IList<double> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<double> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_double_double());
        }

        public static int FindNearestIndex(this Span<double> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_double_double());
        }

        public static int FindNearestIndex(this double[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_double_double());
        }

        public static int FindNearestIndex(this List<double> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_double_double());
        }

        public static int FindNearestIndex(this IList<double> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_double_double());
        }

        public static ReadOnlySpan<double> Range(this ReadOnlySpan<double> span, Range<double> range)
        {
            return Range(span, range, new Comparer_double_double());
        }

        public static ReadOnlySpan<double> Range(this Span<double> span, Range<double> range)
        {
            return Range(span, range, new Comparer_double_double());
        }

        public static ReadOnlySpan<double> Range(this double[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_double_double());
        }

        public static ReadOnlySpan<double> Range(this List<double> list, Range<double> range)
        {
            return Range(list, range, new Comparer_double_double());
        }

        public static IEnumerable<double> Range(this IList<double> list, Range<double> range)
        {
            return Range(list, range, new Comparer_double_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<double> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_double_double());
        }

        public static (int Start, int Length) IndexRange(this Span<double> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_double_double());
        }

        public static (int Start, int Length) IndexRange(this double[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_double_double());
        }

        public static (int Start, int Length) IndexRange(this List<double> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_double_double());
        }

        public static (int Start, int Length) IndexRange(this IList<double> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_double_double());
        }

        public static int FindIndex(this ReadOnlySpan<double> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_decimal(), type);
        }

        public static int FindIndex(this Span<double> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_decimal(), type);
        }

        public static int FindIndex(this double[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_double_decimal(), type);
        }

        public static int FindIndex(this List<double> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_decimal(), type);
        }

        public static int FindIndex(this IList<double> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<double> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_double_decimal());
        }

        public static int FindNearestIndex(this Span<double> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_double_decimal());
        }

        public static int FindNearestIndex(this double[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_double_decimal());
        }

        public static int FindNearestIndex(this List<double> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_double_decimal());
        }

        public static int FindNearestIndex(this IList<double> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_double_decimal());
        }

        public static ReadOnlySpan<double> Range(this ReadOnlySpan<double> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_double_decimal());
        }

        public static ReadOnlySpan<double> Range(this Span<double> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_double_decimal());
        }

        public static ReadOnlySpan<double> Range(this double[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_double_decimal());
        }

        public static ReadOnlySpan<double> Range(this List<double> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_double_decimal());
        }

        public static IEnumerable<double> Range(this IList<double> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_double_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<double> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_double_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<double> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_double_decimal());
        }

        public static (int Start, int Length) IndexRange(this double[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_double_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<double> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_double_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<double> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_double_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<double> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_Rational(), type);
        }

        public static int FindIndex(this Span<double> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_double_Rational(), type);
        }

        public static int FindIndex(this double[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_double_Rational(), type);
        }

        public static int FindIndex(this List<double> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_Rational(), type);
        }

        public static int FindIndex(this IList<double> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_double_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<double> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_double_Rational());
        }

        public static int FindNearestIndex(this Span<double> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_double_Rational());
        }

        public static int FindNearestIndex(this double[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_double_Rational());
        }

        public static int FindNearestIndex(this List<double> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_double_Rational());
        }

        public static int FindNearestIndex(this IList<double> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_double_Rational());
        }

        public static ReadOnlySpan<double> Range(this ReadOnlySpan<double> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_double_Rational());
        }

        public static ReadOnlySpan<double> Range(this Span<double> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_double_Rational());
        }

        public static ReadOnlySpan<double> Range(this double[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_double_Rational());
        }

        public static ReadOnlySpan<double> Range(this List<double> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_double_Rational());
        }

        public static IEnumerable<double> Range(this IList<double> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_double_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<double> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_double_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<double> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_double_Rational());
        }

        public static (int Start, int Length) IndexRange(this double[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_double_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<double> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_double_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<double> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_double_Rational());
        }

    }
}