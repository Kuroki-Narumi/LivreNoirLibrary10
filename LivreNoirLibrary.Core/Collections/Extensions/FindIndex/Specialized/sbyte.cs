using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<sbyte> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_int(), type);
        }

        public static int FindIndex(this Span<sbyte> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_int(), type);
        }

        public static int FindIndex(this sbyte[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_sbyte_int(), type);
        }

        public static int FindIndex(this List<sbyte> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_int(), type);
        }

        public static int FindIndex(this IList<sbyte> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<sbyte> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_int());
        }

        public static int FindNearestIndex(this Span<sbyte> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_int());
        }

        public static int FindNearestIndex(this sbyte[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_sbyte_int());
        }

        public static int FindNearestIndex(this List<sbyte> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_int());
        }

        public static int FindNearestIndex(this IList<sbyte> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_int());
        }

        public static ReadOnlySpan<sbyte> Range(this ReadOnlySpan<sbyte> span, Range<int> range)
        {
            return Range(span, range, new Comparer_sbyte_int());
        }

        public static ReadOnlySpan<sbyte> Range(this Span<sbyte> span, Range<int> range)
        {
            return Range(span, range, new Comparer_sbyte_int());
        }

        public static ReadOnlySpan<sbyte> Range(this sbyte[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_sbyte_int());
        }

        public static ReadOnlySpan<sbyte> Range(this List<sbyte> list, Range<int> range)
        {
            return Range(list, range, new Comparer_sbyte_int());
        }

        public static IEnumerable<sbyte> Range(this IList<sbyte> list, Range<int> range)
        {
            return Range(list, range, new Comparer_sbyte_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<sbyte> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_int());
        }

        public static (int Start, int Length) IndexRange(this Span<sbyte> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_int());
        }

        public static (int Start, int Length) IndexRange(this sbyte[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_sbyte_int());
        }

        public static (int Start, int Length) IndexRange(this List<sbyte> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_int());
        }

        public static (int Start, int Length) IndexRange(this IList<sbyte> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_int());
        }

        public static int FindIndex(this ReadOnlySpan<sbyte> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_long(), type);
        }

        public static int FindIndex(this Span<sbyte> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_long(), type);
        }

        public static int FindIndex(this sbyte[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_sbyte_long(), type);
        }

        public static int FindIndex(this List<sbyte> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_long(), type);
        }

        public static int FindIndex(this IList<sbyte> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<sbyte> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_long());
        }

        public static int FindNearestIndex(this Span<sbyte> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_long());
        }

        public static int FindNearestIndex(this sbyte[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_sbyte_long());
        }

        public static int FindNearestIndex(this List<sbyte> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_long());
        }

        public static int FindNearestIndex(this IList<sbyte> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_long());
        }

        public static ReadOnlySpan<sbyte> Range(this ReadOnlySpan<sbyte> span, Range<long> range)
        {
            return Range(span, range, new Comparer_sbyte_long());
        }

        public static ReadOnlySpan<sbyte> Range(this Span<sbyte> span, Range<long> range)
        {
            return Range(span, range, new Comparer_sbyte_long());
        }

        public static ReadOnlySpan<sbyte> Range(this sbyte[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_sbyte_long());
        }

        public static ReadOnlySpan<sbyte> Range(this List<sbyte> list, Range<long> range)
        {
            return Range(list, range, new Comparer_sbyte_long());
        }

        public static IEnumerable<sbyte> Range(this IList<sbyte> list, Range<long> range)
        {
            return Range(list, range, new Comparer_sbyte_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<sbyte> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_long());
        }

        public static (int Start, int Length) IndexRange(this Span<sbyte> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_long());
        }

        public static (int Start, int Length) IndexRange(this sbyte[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_sbyte_long());
        }

        public static (int Start, int Length) IndexRange(this List<sbyte> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_long());
        }

        public static (int Start, int Length) IndexRange(this IList<sbyte> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_long());
        }

        public static int FindIndex(this ReadOnlySpan<sbyte> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_ulong(), type);
        }

        public static int FindIndex(this Span<sbyte> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_ulong(), type);
        }

        public static int FindIndex(this sbyte[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_sbyte_ulong(), type);
        }

        public static int FindIndex(this List<sbyte> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_ulong(), type);
        }

        public static int FindIndex(this IList<sbyte> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<sbyte> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_ulong());
        }

        public static int FindNearestIndex(this Span<sbyte> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_ulong());
        }

        public static int FindNearestIndex(this sbyte[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_sbyte_ulong());
        }

        public static int FindNearestIndex(this List<sbyte> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_ulong());
        }

        public static int FindNearestIndex(this IList<sbyte> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_ulong());
        }

        public static ReadOnlySpan<sbyte> Range(this ReadOnlySpan<sbyte> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_sbyte_ulong());
        }

        public static ReadOnlySpan<sbyte> Range(this Span<sbyte> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_sbyte_ulong());
        }

        public static ReadOnlySpan<sbyte> Range(this sbyte[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_sbyte_ulong());
        }

        public static ReadOnlySpan<sbyte> Range(this List<sbyte> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_sbyte_ulong());
        }

        public static IEnumerable<sbyte> Range(this IList<sbyte> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_sbyte_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<sbyte> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<sbyte> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_ulong());
        }

        public static (int Start, int Length) IndexRange(this sbyte[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_sbyte_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<sbyte> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<sbyte> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<sbyte> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_float(), type);
        }

        public static int FindIndex(this Span<sbyte> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_float(), type);
        }

        public static int FindIndex(this sbyte[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_sbyte_float(), type);
        }

        public static int FindIndex(this List<sbyte> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_float(), type);
        }

        public static int FindIndex(this IList<sbyte> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<sbyte> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_float());
        }

        public static int FindNearestIndex(this Span<sbyte> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_float());
        }

        public static int FindNearestIndex(this sbyte[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_sbyte_float());
        }

        public static int FindNearestIndex(this List<sbyte> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_float());
        }

        public static int FindNearestIndex(this IList<sbyte> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_float());
        }

        public static ReadOnlySpan<sbyte> Range(this ReadOnlySpan<sbyte> span, Range<float> range)
        {
            return Range(span, range, new Comparer_sbyte_float());
        }

        public static ReadOnlySpan<sbyte> Range(this Span<sbyte> span, Range<float> range)
        {
            return Range(span, range, new Comparer_sbyte_float());
        }

        public static ReadOnlySpan<sbyte> Range(this sbyte[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_sbyte_float());
        }

        public static ReadOnlySpan<sbyte> Range(this List<sbyte> list, Range<float> range)
        {
            return Range(list, range, new Comparer_sbyte_float());
        }

        public static IEnumerable<sbyte> Range(this IList<sbyte> list, Range<float> range)
        {
            return Range(list, range, new Comparer_sbyte_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<sbyte> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_float());
        }

        public static (int Start, int Length) IndexRange(this Span<sbyte> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_float());
        }

        public static (int Start, int Length) IndexRange(this sbyte[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_sbyte_float());
        }

        public static (int Start, int Length) IndexRange(this List<sbyte> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_float());
        }

        public static (int Start, int Length) IndexRange(this IList<sbyte> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_float());
        }

        public static int FindIndex(this ReadOnlySpan<sbyte> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_double(), type);
        }

        public static int FindIndex(this Span<sbyte> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_double(), type);
        }

        public static int FindIndex(this sbyte[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_sbyte_double(), type);
        }

        public static int FindIndex(this List<sbyte> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_double(), type);
        }

        public static int FindIndex(this IList<sbyte> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<sbyte> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_double());
        }

        public static int FindNearestIndex(this Span<sbyte> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_double());
        }

        public static int FindNearestIndex(this sbyte[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_sbyte_double());
        }

        public static int FindNearestIndex(this List<sbyte> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_double());
        }

        public static int FindNearestIndex(this IList<sbyte> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_double());
        }

        public static ReadOnlySpan<sbyte> Range(this ReadOnlySpan<sbyte> span, Range<double> range)
        {
            return Range(span, range, new Comparer_sbyte_double());
        }

        public static ReadOnlySpan<sbyte> Range(this Span<sbyte> span, Range<double> range)
        {
            return Range(span, range, new Comparer_sbyte_double());
        }

        public static ReadOnlySpan<sbyte> Range(this sbyte[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_sbyte_double());
        }

        public static ReadOnlySpan<sbyte> Range(this List<sbyte> list, Range<double> range)
        {
            return Range(list, range, new Comparer_sbyte_double());
        }

        public static IEnumerable<sbyte> Range(this IList<sbyte> list, Range<double> range)
        {
            return Range(list, range, new Comparer_sbyte_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<sbyte> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_double());
        }

        public static (int Start, int Length) IndexRange(this Span<sbyte> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_double());
        }

        public static (int Start, int Length) IndexRange(this sbyte[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_sbyte_double());
        }

        public static (int Start, int Length) IndexRange(this List<sbyte> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_double());
        }

        public static (int Start, int Length) IndexRange(this IList<sbyte> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_double());
        }

        public static int FindIndex(this ReadOnlySpan<sbyte> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_decimal(), type);
        }

        public static int FindIndex(this Span<sbyte> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_decimal(), type);
        }

        public static int FindIndex(this sbyte[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_sbyte_decimal(), type);
        }

        public static int FindIndex(this List<sbyte> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_decimal(), type);
        }

        public static int FindIndex(this IList<sbyte> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<sbyte> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_decimal());
        }

        public static int FindNearestIndex(this Span<sbyte> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_decimal());
        }

        public static int FindNearestIndex(this sbyte[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_sbyte_decimal());
        }

        public static int FindNearestIndex(this List<sbyte> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_decimal());
        }

        public static int FindNearestIndex(this IList<sbyte> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_decimal());
        }

        public static ReadOnlySpan<sbyte> Range(this ReadOnlySpan<sbyte> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_sbyte_decimal());
        }

        public static ReadOnlySpan<sbyte> Range(this Span<sbyte> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_sbyte_decimal());
        }

        public static ReadOnlySpan<sbyte> Range(this sbyte[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_sbyte_decimal());
        }

        public static ReadOnlySpan<sbyte> Range(this List<sbyte> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_sbyte_decimal());
        }

        public static IEnumerable<sbyte> Range(this IList<sbyte> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_sbyte_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<sbyte> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<sbyte> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_decimal());
        }

        public static (int Start, int Length) IndexRange(this sbyte[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_sbyte_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<sbyte> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<sbyte> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<sbyte> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_Rational(), type);
        }

        public static int FindIndex(this Span<sbyte> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_sbyte_Rational(), type);
        }

        public static int FindIndex(this sbyte[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_sbyte_Rational(), type);
        }

        public static int FindIndex(this List<sbyte> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_Rational(), type);
        }

        public static int FindIndex(this IList<sbyte> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_sbyte_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<sbyte> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_Rational());
        }

        public static int FindNearestIndex(this Span<sbyte> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_sbyte_Rational());
        }

        public static int FindNearestIndex(this sbyte[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_sbyte_Rational());
        }

        public static int FindNearestIndex(this List<sbyte> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_Rational());
        }

        public static int FindNearestIndex(this IList<sbyte> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_sbyte_Rational());
        }

        public static ReadOnlySpan<sbyte> Range(this ReadOnlySpan<sbyte> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_sbyte_Rational());
        }

        public static ReadOnlySpan<sbyte> Range(this Span<sbyte> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_sbyte_Rational());
        }

        public static ReadOnlySpan<sbyte> Range(this sbyte[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_sbyte_Rational());
        }

        public static ReadOnlySpan<sbyte> Range(this List<sbyte> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_sbyte_Rational());
        }

        public static IEnumerable<sbyte> Range(this IList<sbyte> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_sbyte_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<sbyte> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<sbyte> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_sbyte_Rational());
        }

        public static (int Start, int Length) IndexRange(this sbyte[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_sbyte_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<sbyte> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<sbyte> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_sbyte_Rational());
        }

    }
}