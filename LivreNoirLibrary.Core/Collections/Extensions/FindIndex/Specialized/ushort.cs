using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<ushort> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_int(), type);
        }

        public static int FindIndex(this Span<ushort> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_int(), type);
        }

        public static int FindIndex(this ushort[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ushort_int(), type);
        }

        public static int FindIndex(this List<ushort> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_int(), type);
        }

        public static int FindIndex(this IList<ushort> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ushort> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_int());
        }

        public static int FindNearestIndex(this Span<ushort> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_int());
        }

        public static int FindNearestIndex(this ushort[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_ushort_int());
        }

        public static int FindNearestIndex(this List<ushort> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_int());
        }

        public static int FindNearestIndex(this IList<ushort> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_int());
        }

        public static ReadOnlySpan<ushort> Range(this ReadOnlySpan<ushort> span, Range<int> range)
        {
            return Range(span, range, new Comparer_ushort_int());
        }

        public static ReadOnlySpan<ushort> Range(this Span<ushort> span, Range<int> range)
        {
            return Range(span, range, new Comparer_ushort_int());
        }

        public static ReadOnlySpan<ushort> Range(this ushort[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_ushort_int());
        }

        public static ReadOnlySpan<ushort> Range(this List<ushort> list, Range<int> range)
        {
            return Range(list, range, new Comparer_ushort_int());
        }

        public static IEnumerable<ushort> Range(this IList<ushort> list, Range<int> range)
        {
            return Range(list, range, new Comparer_ushort_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ushort> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_ushort_int());
        }

        public static (int Start, int Length) IndexRange(this Span<ushort> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_ushort_int());
        }

        public static (int Start, int Length) IndexRange(this ushort[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_ushort_int());
        }

        public static (int Start, int Length) IndexRange(this List<ushort> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_ushort_int());
        }

        public static (int Start, int Length) IndexRange(this IList<ushort> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_ushort_int());
        }

        public static int FindIndex(this ReadOnlySpan<ushort> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_long(), type);
        }

        public static int FindIndex(this Span<ushort> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_long(), type);
        }

        public static int FindIndex(this ushort[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ushort_long(), type);
        }

        public static int FindIndex(this List<ushort> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_long(), type);
        }

        public static int FindIndex(this IList<ushort> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ushort> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_long());
        }

        public static int FindNearestIndex(this Span<ushort> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_long());
        }

        public static int FindNearestIndex(this ushort[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_ushort_long());
        }

        public static int FindNearestIndex(this List<ushort> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_long());
        }

        public static int FindNearestIndex(this IList<ushort> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_long());
        }

        public static ReadOnlySpan<ushort> Range(this ReadOnlySpan<ushort> span, Range<long> range)
        {
            return Range(span, range, new Comparer_ushort_long());
        }

        public static ReadOnlySpan<ushort> Range(this Span<ushort> span, Range<long> range)
        {
            return Range(span, range, new Comparer_ushort_long());
        }

        public static ReadOnlySpan<ushort> Range(this ushort[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_ushort_long());
        }

        public static ReadOnlySpan<ushort> Range(this List<ushort> list, Range<long> range)
        {
            return Range(list, range, new Comparer_ushort_long());
        }

        public static IEnumerable<ushort> Range(this IList<ushort> list, Range<long> range)
        {
            return Range(list, range, new Comparer_ushort_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ushort> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_ushort_long());
        }

        public static (int Start, int Length) IndexRange(this Span<ushort> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_ushort_long());
        }

        public static (int Start, int Length) IndexRange(this ushort[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_ushort_long());
        }

        public static (int Start, int Length) IndexRange(this List<ushort> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_ushort_long());
        }

        public static (int Start, int Length) IndexRange(this IList<ushort> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_ushort_long());
        }

        public static int FindIndex(this ReadOnlySpan<ushort> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_ulong(), type);
        }

        public static int FindIndex(this Span<ushort> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_ulong(), type);
        }

        public static int FindIndex(this ushort[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ushort_ulong(), type);
        }

        public static int FindIndex(this List<ushort> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_ulong(), type);
        }

        public static int FindIndex(this IList<ushort> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ushort> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_ulong());
        }

        public static int FindNearestIndex(this Span<ushort> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_ulong());
        }

        public static int FindNearestIndex(this ushort[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_ushort_ulong());
        }

        public static int FindNearestIndex(this List<ushort> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_ulong());
        }

        public static int FindNearestIndex(this IList<ushort> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_ulong());
        }

        public static ReadOnlySpan<ushort> Range(this ReadOnlySpan<ushort> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_ushort_ulong());
        }

        public static ReadOnlySpan<ushort> Range(this Span<ushort> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_ushort_ulong());
        }

        public static ReadOnlySpan<ushort> Range(this ushort[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_ushort_ulong());
        }

        public static ReadOnlySpan<ushort> Range(this List<ushort> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_ushort_ulong());
        }

        public static IEnumerable<ushort> Range(this IList<ushort> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_ushort_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ushort> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_ushort_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<ushort> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_ushort_ulong());
        }

        public static (int Start, int Length) IndexRange(this ushort[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_ushort_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<ushort> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_ushort_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<ushort> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_ushort_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<ushort> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_float(), type);
        }

        public static int FindIndex(this Span<ushort> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_float(), type);
        }

        public static int FindIndex(this ushort[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ushort_float(), type);
        }

        public static int FindIndex(this List<ushort> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_float(), type);
        }

        public static int FindIndex(this IList<ushort> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ushort> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_float());
        }

        public static int FindNearestIndex(this Span<ushort> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_float());
        }

        public static int FindNearestIndex(this ushort[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_ushort_float());
        }

        public static int FindNearestIndex(this List<ushort> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_float());
        }

        public static int FindNearestIndex(this IList<ushort> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_float());
        }

        public static ReadOnlySpan<ushort> Range(this ReadOnlySpan<ushort> span, Range<float> range)
        {
            return Range(span, range, new Comparer_ushort_float());
        }

        public static ReadOnlySpan<ushort> Range(this Span<ushort> span, Range<float> range)
        {
            return Range(span, range, new Comparer_ushort_float());
        }

        public static ReadOnlySpan<ushort> Range(this ushort[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_ushort_float());
        }

        public static ReadOnlySpan<ushort> Range(this List<ushort> list, Range<float> range)
        {
            return Range(list, range, new Comparer_ushort_float());
        }

        public static IEnumerable<ushort> Range(this IList<ushort> list, Range<float> range)
        {
            return Range(list, range, new Comparer_ushort_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ushort> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_ushort_float());
        }

        public static (int Start, int Length) IndexRange(this Span<ushort> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_ushort_float());
        }

        public static (int Start, int Length) IndexRange(this ushort[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_ushort_float());
        }

        public static (int Start, int Length) IndexRange(this List<ushort> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_ushort_float());
        }

        public static (int Start, int Length) IndexRange(this IList<ushort> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_ushort_float());
        }

        public static int FindIndex(this ReadOnlySpan<ushort> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_double(), type);
        }

        public static int FindIndex(this Span<ushort> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_double(), type);
        }

        public static int FindIndex(this ushort[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ushort_double(), type);
        }

        public static int FindIndex(this List<ushort> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_double(), type);
        }

        public static int FindIndex(this IList<ushort> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ushort> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_double());
        }

        public static int FindNearestIndex(this Span<ushort> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_double());
        }

        public static int FindNearestIndex(this ushort[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_ushort_double());
        }

        public static int FindNearestIndex(this List<ushort> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_double());
        }

        public static int FindNearestIndex(this IList<ushort> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_double());
        }

        public static ReadOnlySpan<ushort> Range(this ReadOnlySpan<ushort> span, Range<double> range)
        {
            return Range(span, range, new Comparer_ushort_double());
        }

        public static ReadOnlySpan<ushort> Range(this Span<ushort> span, Range<double> range)
        {
            return Range(span, range, new Comparer_ushort_double());
        }

        public static ReadOnlySpan<ushort> Range(this ushort[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_ushort_double());
        }

        public static ReadOnlySpan<ushort> Range(this List<ushort> list, Range<double> range)
        {
            return Range(list, range, new Comparer_ushort_double());
        }

        public static IEnumerable<ushort> Range(this IList<ushort> list, Range<double> range)
        {
            return Range(list, range, new Comparer_ushort_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ushort> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_ushort_double());
        }

        public static (int Start, int Length) IndexRange(this Span<ushort> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_ushort_double());
        }

        public static (int Start, int Length) IndexRange(this ushort[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_ushort_double());
        }

        public static (int Start, int Length) IndexRange(this List<ushort> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_ushort_double());
        }

        public static (int Start, int Length) IndexRange(this IList<ushort> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_ushort_double());
        }

        public static int FindIndex(this ReadOnlySpan<ushort> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_decimal(), type);
        }

        public static int FindIndex(this Span<ushort> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_decimal(), type);
        }

        public static int FindIndex(this ushort[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ushort_decimal(), type);
        }

        public static int FindIndex(this List<ushort> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_decimal(), type);
        }

        public static int FindIndex(this IList<ushort> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ushort> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_decimal());
        }

        public static int FindNearestIndex(this Span<ushort> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_decimal());
        }

        public static int FindNearestIndex(this ushort[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_ushort_decimal());
        }

        public static int FindNearestIndex(this List<ushort> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_decimal());
        }

        public static int FindNearestIndex(this IList<ushort> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_decimal());
        }

        public static ReadOnlySpan<ushort> Range(this ReadOnlySpan<ushort> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_ushort_decimal());
        }

        public static ReadOnlySpan<ushort> Range(this Span<ushort> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_ushort_decimal());
        }

        public static ReadOnlySpan<ushort> Range(this ushort[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_ushort_decimal());
        }

        public static ReadOnlySpan<ushort> Range(this List<ushort> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_ushort_decimal());
        }

        public static IEnumerable<ushort> Range(this IList<ushort> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_ushort_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ushort> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_ushort_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<ushort> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_ushort_decimal());
        }

        public static (int Start, int Length) IndexRange(this ushort[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_ushort_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<ushort> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_ushort_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<ushort> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_ushort_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<ushort> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_Rational(), type);
        }

        public static int FindIndex(this Span<ushort> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_ushort_Rational(), type);
        }

        public static int FindIndex(this ushort[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_ushort_Rational(), type);
        }

        public static int FindIndex(this List<ushort> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_Rational(), type);
        }

        public static int FindIndex(this IList<ushort> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_ushort_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<ushort> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_Rational());
        }

        public static int FindNearestIndex(this Span<ushort> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_ushort_Rational());
        }

        public static int FindNearestIndex(this ushort[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_ushort_Rational());
        }

        public static int FindNearestIndex(this List<ushort> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_Rational());
        }

        public static int FindNearestIndex(this IList<ushort> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_ushort_Rational());
        }

        public static ReadOnlySpan<ushort> Range(this ReadOnlySpan<ushort> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_ushort_Rational());
        }

        public static ReadOnlySpan<ushort> Range(this Span<ushort> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_ushort_Rational());
        }

        public static ReadOnlySpan<ushort> Range(this ushort[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_ushort_Rational());
        }

        public static ReadOnlySpan<ushort> Range(this List<ushort> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_ushort_Rational());
        }

        public static IEnumerable<ushort> Range(this IList<ushort> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_ushort_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<ushort> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_ushort_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<ushort> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_ushort_Rational());
        }

        public static (int Start, int Length) IndexRange(this ushort[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_ushort_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<ushort> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_ushort_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<ushort> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_ushort_Rational());
        }

    }
}