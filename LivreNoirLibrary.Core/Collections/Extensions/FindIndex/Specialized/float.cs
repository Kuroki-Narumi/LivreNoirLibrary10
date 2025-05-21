using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<float> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_int(), type);
        }

        public static int FindIndex(this Span<float> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_int(), type);
        }

        public static int FindIndex(this float[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_float_int(), type);
        }

        public static int FindIndex(this List<float> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_int(), type);
        }

        public static int FindIndex(this IList<float> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<float> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_float_int());
        }

        public static int FindNearestIndex(this Span<float> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_float_int());
        }

        public static int FindNearestIndex(this float[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_float_int());
        }

        public static int FindNearestIndex(this List<float> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_float_int());
        }

        public static int FindNearestIndex(this IList<float> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_float_int());
        }

        public static ReadOnlySpan<float> Range(this ReadOnlySpan<float> span, Range<int> range)
        {
            return Range(span, range, new Comparer_float_int());
        }

        public static ReadOnlySpan<float> Range(this Span<float> span, Range<int> range)
        {
            return Range(span, range, new Comparer_float_int());
        }

        public static ReadOnlySpan<float> Range(this float[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_float_int());
        }

        public static ReadOnlySpan<float> Range(this List<float> list, Range<int> range)
        {
            return Range(list, range, new Comparer_float_int());
        }

        public static IEnumerable<float> Range(this IList<float> list, Range<int> range)
        {
            return Range(list, range, new Comparer_float_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<float> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_float_int());
        }

        public static (int Start, int Length) IndexRange(this Span<float> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_float_int());
        }

        public static (int Start, int Length) IndexRange(this float[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_float_int());
        }

        public static (int Start, int Length) IndexRange(this List<float> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_float_int());
        }

        public static (int Start, int Length) IndexRange(this IList<float> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_float_int());
        }

        public static int FindIndex(this ReadOnlySpan<float> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_long(), type);
        }

        public static int FindIndex(this Span<float> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_long(), type);
        }

        public static int FindIndex(this float[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_float_long(), type);
        }

        public static int FindIndex(this List<float> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_long(), type);
        }

        public static int FindIndex(this IList<float> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<float> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_float_long());
        }

        public static int FindNearestIndex(this Span<float> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_float_long());
        }

        public static int FindNearestIndex(this float[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_float_long());
        }

        public static int FindNearestIndex(this List<float> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_float_long());
        }

        public static int FindNearestIndex(this IList<float> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_float_long());
        }

        public static ReadOnlySpan<float> Range(this ReadOnlySpan<float> span, Range<long> range)
        {
            return Range(span, range, new Comparer_float_long());
        }

        public static ReadOnlySpan<float> Range(this Span<float> span, Range<long> range)
        {
            return Range(span, range, new Comparer_float_long());
        }

        public static ReadOnlySpan<float> Range(this float[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_float_long());
        }

        public static ReadOnlySpan<float> Range(this List<float> list, Range<long> range)
        {
            return Range(list, range, new Comparer_float_long());
        }

        public static IEnumerable<float> Range(this IList<float> list, Range<long> range)
        {
            return Range(list, range, new Comparer_float_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<float> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_float_long());
        }

        public static (int Start, int Length) IndexRange(this Span<float> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_float_long());
        }

        public static (int Start, int Length) IndexRange(this float[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_float_long());
        }

        public static (int Start, int Length) IndexRange(this List<float> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_float_long());
        }

        public static (int Start, int Length) IndexRange(this IList<float> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_float_long());
        }

        public static int FindIndex(this ReadOnlySpan<float> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_ulong(), type);
        }

        public static int FindIndex(this Span<float> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_ulong(), type);
        }

        public static int FindIndex(this float[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_float_ulong(), type);
        }

        public static int FindIndex(this List<float> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_ulong(), type);
        }

        public static int FindIndex(this IList<float> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<float> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_float_ulong());
        }

        public static int FindNearestIndex(this Span<float> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_float_ulong());
        }

        public static int FindNearestIndex(this float[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_float_ulong());
        }

        public static int FindNearestIndex(this List<float> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_float_ulong());
        }

        public static int FindNearestIndex(this IList<float> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_float_ulong());
        }

        public static ReadOnlySpan<float> Range(this ReadOnlySpan<float> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_float_ulong());
        }

        public static ReadOnlySpan<float> Range(this Span<float> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_float_ulong());
        }

        public static ReadOnlySpan<float> Range(this float[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_float_ulong());
        }

        public static ReadOnlySpan<float> Range(this List<float> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_float_ulong());
        }

        public static IEnumerable<float> Range(this IList<float> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_float_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<float> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_float_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<float> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_float_ulong());
        }

        public static (int Start, int Length) IndexRange(this float[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_float_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<float> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_float_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<float> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_float_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<float> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_float(), type);
        }

        public static int FindIndex(this Span<float> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_float(), type);
        }

        public static int FindIndex(this float[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_float_float(), type);
        }

        public static int FindIndex(this List<float> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_float(), type);
        }

        public static int FindIndex(this IList<float> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<float> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_float_float());
        }

        public static int FindNearestIndex(this Span<float> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_float_float());
        }

        public static int FindNearestIndex(this float[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_float_float());
        }

        public static int FindNearestIndex(this List<float> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_float_float());
        }

        public static int FindNearestIndex(this IList<float> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_float_float());
        }

        public static ReadOnlySpan<float> Range(this ReadOnlySpan<float> span, Range<float> range)
        {
            return Range(span, range, new Comparer_float_float());
        }

        public static ReadOnlySpan<float> Range(this Span<float> span, Range<float> range)
        {
            return Range(span, range, new Comparer_float_float());
        }

        public static ReadOnlySpan<float> Range(this float[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_float_float());
        }

        public static ReadOnlySpan<float> Range(this List<float> list, Range<float> range)
        {
            return Range(list, range, new Comparer_float_float());
        }

        public static IEnumerable<float> Range(this IList<float> list, Range<float> range)
        {
            return Range(list, range, new Comparer_float_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<float> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_float_float());
        }

        public static (int Start, int Length) IndexRange(this Span<float> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_float_float());
        }

        public static (int Start, int Length) IndexRange(this float[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_float_float());
        }

        public static (int Start, int Length) IndexRange(this List<float> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_float_float());
        }

        public static (int Start, int Length) IndexRange(this IList<float> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_float_float());
        }

        public static int FindIndex(this ReadOnlySpan<float> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_double(), type);
        }

        public static int FindIndex(this Span<float> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_double(), type);
        }

        public static int FindIndex(this float[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_float_double(), type);
        }

        public static int FindIndex(this List<float> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_double(), type);
        }

        public static int FindIndex(this IList<float> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<float> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_float_double());
        }

        public static int FindNearestIndex(this Span<float> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_float_double());
        }

        public static int FindNearestIndex(this float[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_float_double());
        }

        public static int FindNearestIndex(this List<float> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_float_double());
        }

        public static int FindNearestIndex(this IList<float> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_float_double());
        }

        public static ReadOnlySpan<float> Range(this ReadOnlySpan<float> span, Range<double> range)
        {
            return Range(span, range, new Comparer_float_double());
        }

        public static ReadOnlySpan<float> Range(this Span<float> span, Range<double> range)
        {
            return Range(span, range, new Comparer_float_double());
        }

        public static ReadOnlySpan<float> Range(this float[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_float_double());
        }

        public static ReadOnlySpan<float> Range(this List<float> list, Range<double> range)
        {
            return Range(list, range, new Comparer_float_double());
        }

        public static IEnumerable<float> Range(this IList<float> list, Range<double> range)
        {
            return Range(list, range, new Comparer_float_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<float> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_float_double());
        }

        public static (int Start, int Length) IndexRange(this Span<float> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_float_double());
        }

        public static (int Start, int Length) IndexRange(this float[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_float_double());
        }

        public static (int Start, int Length) IndexRange(this List<float> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_float_double());
        }

        public static (int Start, int Length) IndexRange(this IList<float> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_float_double());
        }

        public static int FindIndex(this ReadOnlySpan<float> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_decimal(), type);
        }

        public static int FindIndex(this Span<float> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_decimal(), type);
        }

        public static int FindIndex(this float[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_float_decimal(), type);
        }

        public static int FindIndex(this List<float> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_decimal(), type);
        }

        public static int FindIndex(this IList<float> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<float> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_float_decimal());
        }

        public static int FindNearestIndex(this Span<float> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_float_decimal());
        }

        public static int FindNearestIndex(this float[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_float_decimal());
        }

        public static int FindNearestIndex(this List<float> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_float_decimal());
        }

        public static int FindNearestIndex(this IList<float> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_float_decimal());
        }

        public static ReadOnlySpan<float> Range(this ReadOnlySpan<float> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_float_decimal());
        }

        public static ReadOnlySpan<float> Range(this Span<float> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_float_decimal());
        }

        public static ReadOnlySpan<float> Range(this float[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_float_decimal());
        }

        public static ReadOnlySpan<float> Range(this List<float> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_float_decimal());
        }

        public static IEnumerable<float> Range(this IList<float> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_float_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<float> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_float_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<float> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_float_decimal());
        }

        public static (int Start, int Length) IndexRange(this float[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_float_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<float> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_float_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<float> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_float_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<float> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_Rational(), type);
        }

        public static int FindIndex(this Span<float> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_float_Rational(), type);
        }

        public static int FindIndex(this float[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_float_Rational(), type);
        }

        public static int FindIndex(this List<float> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_Rational(), type);
        }

        public static int FindIndex(this IList<float> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_float_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<float> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_float_Rational());
        }

        public static int FindNearestIndex(this Span<float> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_float_Rational());
        }

        public static int FindNearestIndex(this float[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_float_Rational());
        }

        public static int FindNearestIndex(this List<float> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_float_Rational());
        }

        public static int FindNearestIndex(this IList<float> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_float_Rational());
        }

        public static ReadOnlySpan<float> Range(this ReadOnlySpan<float> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_float_Rational());
        }

        public static ReadOnlySpan<float> Range(this Span<float> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_float_Rational());
        }

        public static ReadOnlySpan<float> Range(this float[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_float_Rational());
        }

        public static ReadOnlySpan<float> Range(this List<float> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_float_Rational());
        }

        public static IEnumerable<float> Range(this IList<float> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_float_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<float> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_float_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<float> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_float_Rational());
        }

        public static (int Start, int Length) IndexRange(this float[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_float_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<float> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_float_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<float> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_float_Rational());
        }

    }
}